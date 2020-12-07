using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NRZ.Data;
using NRZ.Data.Extensions;
using NRZ.Models.EPayment;
using NRZ.Models.Settings;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NRZ.Services.EPayments
{
    public class EPaymentService : BaseService, IEPaymentService
    {
        private readonly IEServicesSettingsService _eServiceSettingsService;
        private readonly IEmailService _emailService;
        private readonly EPaymentSettings _settings;

        public EPaymentService(NRZContext context,
            IEServicesSettingsService eServiceSettingsService,
            IEmailService emailService,
            IOptions<EPaymentSettings> config,
            IStringLocalizer<SharedResources> localizer = null)
            : base(context, localizer)
        {
            _eServiceSettingsService = eServiceSettingsService;
            _emailService = emailService;
            _settings = config.Value;
        }


        public async Task<EServicesSettingsModel> LoadSettingsAsync()
        {
            EServicesSettingsModel entity = await _context.EservicesSettings
                .Select(x => x.ToModel())
                .FirstOrDefaultAsync();
            
            return entity;
        }

        public async Task ChangeSettingsAsync(EServicesSettingsModel model)
        {
            EservicesSettings entity = await _context.EservicesSettings
                .FirstOrDefaultAsync();

            if (entity == null) 
                throw new ArgumentNullException(nameof(EservicesSettings), $"EservicesSettings not found for update!");

            entity.Update(model);

            // TODO: updatedAt, updatedBy

            _context.EservicesSettings.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<EServicePaymentRequestCreateModel> GeneratePaymentRequestAsync(Shared.Enums.EServiceType serviceType, int requestId)
        {
            // Взима данни на заявителя.
            ApplicantData applicantData = await GetApplicantDataAsync(serviceType, requestId);

            // Взеима данните за доставчик на ЕАУ, в случая Министерство на правосъдието.
            EServicesSettingsModel settings = await _eServiceSettingsService.GetEServiceSettingsAsync();

            PaymentRequestModel paymentRequest = CreatePaymentRequest(serviceType, settings, applicantData);
            paymentRequest = await SavePaymentRequestAsync(paymentRequest);

            EServicePaymentRequestCreateModel servicePaymentRequest = new EServicePaymentRequestCreateModel();
            servicePaymentRequest.EserviceTypeCode = serviceType.ToString();
            if (serviceType == Shared.Enums.EServiceType.SEIZEDPROPERTYCERTIFICATE)
                servicePaymentRequest.SeizedPropertyCertificateRequestId = requestId;
            else if (serviceType == Shared.Enums.EServiceType.SEIZEDPROPERTYBYOWNERREPORT)
                servicePaymentRequest.SeizedPropertyReportRequestId = requestId;
            servicePaymentRequest.PaymentRequestId = paymentRequest.AisPaymentId;
            servicePaymentRequest.StatusCode = Shared.Enums.PaymentRequestStatus.NEW.ToString();

            EservicePaymentRequest entity = servicePaymentRequest.ToEntity();

            await _context.EservicePaymentRequest.AddAsync(entity);
            await _context.SaveChangesAsync();

            await AddEServicePaymentRequestStatusHistory(entity.Id, entity.StatusCode, entity.CreatedAt, null, null);

            servicePaymentRequest.Id = entity.Id;
            return servicePaymentRequest;
        }

        private async Task AddEServicePaymentRequestStatusHistory(int requestId, string status, DateTime date, DateTime? serviceTime, string errors)
        {
            EservicePaymentRequestStatusHistory entity = new EservicePaymentRequestStatusHistory();
            entity.RequestId = requestId;
            entity.StatusCode = status;
            entity.UpdatedAt = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            entity.EserviceTime = serviceTime;
            entity.Errors = errors;
            await _context.EservicePaymentRequestStatusHistory.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        private async Task<ApplicantData> GetApplicantDataAsync(Shared.Enums.EServiceType serviceType, int requestId)
        {
            Shared.Enums.IdentificationType applicantIdentifierType = Shared.Enums.IdentificationType.EGN;
            string applicantIdentifier = "";
            string applicantName = "";

            if (serviceType == Shared.Enums.EServiceType.SEIZEDPROPERTYCERTIFICATE)
            {
                RequestForCertificateOfDistraintOfProperty req = await _context.RequestForCertificateOfDistraintOfProperty
                    .Where(x => x.Id == requestId)
                    .FirstOrDefaultAsync();

                if (req != null)
                {
                    bool isCompany = String.IsNullOrWhiteSpace(req.PersonalIdentifier);
                    if (isCompany)
                    {
                        applicantIdentifierType = Shared.Enums.IdentificationType.BULSTAT;
                        applicantIdentifier = req.IdentifierOfLegalEntity;
                        applicantName = req.NameOfLegalEntity;
                    }
                    else
                    {
                        if (req.IsPersonalIdentifierTypeLnch == true)
                            applicantIdentifierType = Shared.Enums.IdentificationType.LNCH;
                        applicantIdentifier = req.PersonalIdentifier;
                        applicantName = $"{req.FirstName ?? ""} {req.MiddleName ?? ""} {req.LastName ?? ""}";
                    }
                }
            }
            else if (serviceType == Shared.Enums.EServiceType.SEIZEDPROPERTYBYOWNERREPORT)
            {
                SeizedPropertyAvailabilityRequest req = await _context.SeizedPropertyAvailabilityRequest
                    .Include(x => x.RequestorPerson)
                    .Include(x => x.RequesterCompany)
                    .Where(x => x.Id == requestId)
                    .FirstOrDefaultAsync();

                if (req != null)
                {
                    if (req.RequestorPerson != null)
                    {
                        applicantIdentifier = req.RequestorPerson.IdentificationNumber;
                        applicantName = $"{req.RequestorPerson.FirstName ?? ""} {req.RequestorPerson.MiddleName ?? ""} {req.RequestorPerson.LastName ?? ""}";
                    }
                    else if (req.RequesterCompany != null)
                    {
                        applicantIdentifierType = Shared.Enums.IdentificationType.BULSTAT;
                        applicantIdentifier = req.RequesterCompany.Eik;
                        applicantName = req.RequesterCompany.Name;
                    }

                }
            }

            ApplicantData data = new ApplicantData(applicantIdentifierType, applicantIdentifier, applicantName);
            return data;
        }

        private PaymentRequestModel CreatePaymentRequest(Shared.Enums.EServiceType serviceType, EServicesSettingsModel settings, ApplicantData applicantData)
        {
            int expirationDays = 0;
            decimal fee = 0m;
            string reason = "";
            if (serviceType == Shared.Enums.EServiceType.SEIZEDPROPERTYCERTIFICATE)
            {
                expirationDays = settings.SeizedPropertyCertificateRequestExpirationDays.HasValue ? settings.SeizedPropertyCertificateRequestExpirationDays.Value : 0;
                fee = settings.SeizedPropertyCertificateFee.HasValue ? settings.SeizedPropertyCertificateFee.Value : 0m;
                reason = settings.SeizedPropertyCertificateReason;
            }
            else if (serviceType == Shared.Enums.EServiceType.SEIZEDPROPERTYBYOWNERREPORT)
            {
                expirationDays = settings.SeizedPropertyReportRequestExpirationDays.HasValue ? settings.SeizedPropertyReportRequestExpirationDays.Value : 0;
                fee = settings.SeizedPropertyByOwnerReportFee.HasValue ? settings.SeizedPropertyByOwnerReportFee.Value : 0m;
                reason = settings.SeizedPropertyReportReason;
            }

            PaymentRequestModel request = new PaymentRequestModel();
            request.ServiceProviderName = settings.ServiceProviderName;
            request.ServiceProviderBank = settings.ServiceProviderBank;
            request.ServiceProviderBic = settings.ServiceProviderBic;
            request.ServiceProviderIban = settings.ServiceProviderIban;
            request.Currency = settings.Currency;
            request.PaymentTypeCode = "1"; //Код на плащане. Да се разбере какъв може да бъде. Примерно "1"
            request.PaymentAmount = fee;
            request.PaymentReason = reason;
            request.ApplicantUinTypeId = (int)applicantData.applicantIdentifierType;
            request.ApplicantUin = applicantData.applicantIdentifier;
            request.ApplicantName = applicantData.applicantName;
            request.PaymentReferenceType = "1"; // Тип на документ (референтен документ за плащане). Да се разбере какъв може да бъде.
            request.PaymentReferenceNumber = Guid.NewGuid().ToString();
            request.PaymentReferenceDate = DateTime.Now;
            request.ExpirationDate = DateTime.Now.AddDays(expirationDays);
            request.AdditionalInformation = null;
            request.AdministrativeServiceUri = null;
            request.AdministrativeServiceSupplierUri = _settings.AdministrativeServiceSupplierUri;
            request.AdministrativeServiceNotificationUrl = string.IsNullOrWhiteSpace(settings.AdministrativeServiceNotificationUrl)
                ? _settings.AdministrativeServiceNotificationUrl
                : settings.AdministrativeServiceNotificationUrl;

            return request;
        }

        private async Task<PaymentRequestModel> SavePaymentRequestAsync(PaymentRequestModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException(nameof(model));
            }

            var request = model.ToEntity();

            await _context.PaymentRequest.AddAsync(request);
            await _context.SaveChangesAsync();

            model.AisPaymentId = request.AisPaymentId;
            return model;
        }

        public async Task<PaymentRequest> GetPaymentRequestAsync(int id)
        {
            PaymentRequest request = await _context.PaymentRequest
                .Where(x => x.AisPaymentId == id)
                .FirstOrDefaultAsync();

            return request;
        }

        public async Task<EservicePaymentRequest> GetServicePaymentRequestAsync(int paymentRequestId)
        {
            EservicePaymentRequest entity = await _context.EservicePaymentRequest
                .Include(x => x.PaymentRequest)
                .Where(x => x.PaymentRequest != null && x.PaymentRequest.AisPaymentId == paymentRequestId)
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task ProcessPaymentRequestSendResult(EservicePaymentRequest request, string jsonResult)
        {
            if (request == null)
                throw new Exception("EservicePaymentRequest not found for sent status update!");

            if (String.IsNullOrWhiteSpace(jsonResult))
                throw new Exception("EservicePaymentRequest received empty result!");

            Log.Information($"EPaymentService/ProcessPaymentRequestSendResult - jsonResult: {jsonResult}");

            object resultModel = JsonConvert.DeserializeObject(jsonResult, typeof(PaymentRequestSendResultModel));
            if (resultModel != null)
            {
                PaymentRequestSendResultModel model = resultModel as PaymentRequestSendResultModel;
                if (model.AcceptedReceipt != null && model.AcceptedReceipt.Id == request.PaymentRequest.AisPaymentId)
                {
                    DateTime timestamp = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    string status = Shared.Enums.PaymentRequestStatus.ACCEPTED.ToString();
                    request.UpdatedAt = timestamp;
                    request.StatusCode = status;

                    _context.EservicePaymentRequest.Update(request);
                    await _context.SaveChangesAsync();

                    DateTime serviceTimestamp = DateTime.SpecifyKind(model.AcceptedReceipt.RegistrationTime, DateTimeKind.Utc);
                    await AddEServicePaymentRequestStatusHistory(request.Id, status, timestamp, serviceTimestamp, null);
                }
                else
                {
                    if (model.UnacceptedReceipt != null)
                    {
                        DateTime timestamp = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        string status = Shared.Enums.PaymentRequestStatus.REJECTED.ToString();
                        request.UpdatedAt = timestamp;
                        request.StatusCode = status;

                        _context.EservicePaymentRequest.Update(request);
                        await _context.SaveChangesAsync();

                        DateTime serviceTimestamp = DateTime.SpecifyKind(model.UnacceptedReceipt.ValidationTime, DateTimeKind.Utc);
                        string errors = String.Join("; ", model.UnacceptedReceipt.Errors);
                        await AddEServicePaymentRequestStatusHistory(request.Id, status, timestamp, serviceTimestamp, errors);
                    }
                    else
                        throw new Exception("Json send result is not recognized as valid accepted/rejected model!");
                }
            }
            else
                throw new Exception("Json send result is not recognized as valid model!");

        }

        public async Task ChangePaymentRequestStatusAsync(PaymentRequestStatusChangeModel request)
        {
            int? paymentRequestId = request?.Id;
            EservicePaymentRequest entity = await _context.EservicePaymentRequest
                .Include(x => x.PaymentRequest)
                .Include(x => x.SeizedPropertyCertificateRequest)
                .Include(x => x.SeizedPropertyReportRequest).ThenInclude(x => x.RequestorUser)
                .Include(x => x.SeizedPropertyReportRequest).ThenInclude(x => x.RequestorPerson)
                .Include(x => x.SeizedPropertyReportRequest).ThenInclude(x => x.RequesterCompany)
                .Where(x => x.AisPaymentId == paymentRequestId)
                .FirstOrDefaultAsync();

            if (entity == null) throw new ArgumentNullException(nameof(EservicePaymentRequest), $"EservicePaymentRequest not found for status update! (id:{paymentRequestId})");

            string newStatus = (request?.Status ?? "").Trim().ToUpper();

            DateTime serviceTimestamp = request.ChangeTime.HasValue ? DateTime.SpecifyKind(request.ChangeTime.Value, DateTimeKind.Utc) : DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            DateTime timestamp = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            entity.UpdatedAt = timestamp;
            entity.StatusCode = newStatus;

            _context.EservicePaymentRequest.Update(entity);
            await _context.SaveChangesAsync();

            await AddEServicePaymentRequestStatusHistory(entity.Id, newStatus, timestamp, serviceTimestamp, null);

            //Изпрашане на email
            string email = entity.SeizedPropertyCertificateRequestId.HasValue
                ? entity.SeizedPropertyCertificateRequest.Email
                : (entity.SeizedPropertyReportRequestId.HasValue
                    ? (string.IsNullOrWhiteSpace(entity.SeizedPropertyReportRequest.RequestorUserId) == false
                        ? entity.SeizedPropertyReportRequest.RequestorUser.Email
                        : (entity.SeizedPropertyReportRequest.RequestorPersonId.HasValue
                            ? entity.SeizedPropertyReportRequest.RequestorPerson.Email
                            : ( entity.SeizedPropertyReportRequest.RequesterCompanyId.HasValue
                                ? "" // Как ще се вземе email
                                : ""
                            ))
                        )
                    : ""
                    );
            if (!string.IsNullOrWhiteSpace(email))
            {
                string subject = $"Промяна на статуса на заявка с ИД №: {paymentRequestId} в системата за електронно плащане";
                string body = $"Статусът на заявка с ИД №: {paymentRequestId} в системата за електронно плащане е променен на '{newStatus}'.{Environment.NewLine}" +
                    $"За детайли посетете платформата за електронно плащане на адрес: {(_settings.UseTestEnv ? "https://pay-test.egov.bg/" : "https://pay.egov.bg/")} .";

                if (newStatus.Equals(Shared.Enums.PaymentRequestStatus.PAID.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    // Платено да се мисли как ще си получи услугата
                    if (!string.IsNullOrWhiteSpace(entity.PaymentRequest?.PaymentReferenceNumber))
                    {
                        body += $"{Environment.NewLine} Връзка за получване на услугата: {_settings.AdministrativeServiceSupplierUri}#/seizedPropertyAvailability/paidReport?id={entity.PaymentRequest.PaymentReferenceNumber}";
                    }
                }

                _emailService.SendEmail(email, null, null, subject, body);
            }
        }

        public async Task SetPaymentRequestErrorAsync(EservicePaymentRequest request, string error)
        {
            if (request == null)
                throw new Exception($"EservicePaymentRequest missing for error update!");

            DateTime timestamp = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            string status = Shared.Enums.PaymentRequestStatus.ERROR.ToString();
            request.UpdatedAt = timestamp;
            request.StatusCode = status;

            _context.EservicePaymentRequest.Update(request);
            await _context.SaveChangesAsync();

            await AddEServicePaymentRequestStatusHistory(request.Id, status, timestamp, null, error);
        }

        public IQueryable<EServicePaymentRequestModel> GetAll()
        {
            var listQuery = All()
                .AsNoTracking()
                .Select(x => x.ToModel());

            return listQuery;
        }

        private IQueryable<EservicePaymentRequest> All()
        {
            var listQuery = _context.EservicePaymentRequest
                .Include(x => x.EserviceTypeCodeNavigation)
                .Include(x => x.SeizedPropertyCertificateRequest)
                .Include(x => x.SeizedPropertyReportRequest)
                .Include(x => x.PaymentRequest)
                .Include(x => x.StatusCodeNavigation)
                .OrderByDescending(x => x.Id);

            return listQuery;
        }

        public IQueryable<EServicePaymentStatusHistoryModel> GetPaymentRequestHistory(int requestId)
        {
            var listQuery = _context.EservicePaymentRequestStatusHistory
                .Include(x => x.StatusCodeNavigation)
                .Where(x => x.RequestId == requestId)
                .AsNoTracking()
                .Select(x => x.ToModel());

            return listQuery;
        }

        public async Task<PaymentRequestSendResultModel> SendEpaymentRequestAsync(EPaymentRequestModel dataModel)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                NullValueHandling = NullValueHandling.Include
            };

            string dataJson = JsonConvert.SerializeObject(dataModel, settings);
            (string data, string hmac) = ComputeBodyProps(dataJson, _settings.UseTestEnv);

            // Ако се извика услуга с несъществуващ clientId или ако при извикване на услуга калкулирания 
            // от средата HMAC-SHA256 не съответства на подадения в полето hmac се връща грешка HTTP 401 Unauthorized.
            using (HttpClient client = new HttpClient())
            {
                string serviceVerison = "v1";
                string postUri = $"api/{serviceVerison}/eService/paymentJson";
                client.BaseAddress = new Uri(_settings.UseTestEnv ? _settings.TestEnvUrl : _settings.Url);

                // Параметрите се подават в HTTPS POST заявката в Internet media type формат "application/x-www-form-urlencoded; charset=UTF-8".
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                // Извикването на услуги на средата се извършва чрез HTTP POST заявка на адрес посочен в полето URL на услугата. 
                // При всяко извикване на услуга се подават три параметъра:
                // 1.	clientId – уникален идентификатор на клиента на средата
                // 2.	hmac – hmac-sha256 код изчислен върху полето data
                // 3.	data – данни за предаваното съобщение.

                var encodedHttpBody = $"clientId={HttpUtility.UrlEncode(_settings.UseTestEnv ? _settings.TestEnvClientId : _settings.ClientId)}&hmac={HttpUtility.UrlEncode(hmac)}&data={HttpUtility.UrlEncode(data)}";
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateRemoteCertificate);

                var response = await client.PostAsync(postUri, new StringContent(encodedHttpBody, Encoding.UTF8, "application/x-www-form-urlencoded"));

                // При невалидна структура на документ "ЗАЯВКА ЗА ПЛАЩАНЕ" се връща грешка HTTP 400 BAD REQUEST

                PaymentRequestSendResultModel resultModel;
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    resultModel = GetResultModel(responseContent);
                }
                else
                {
                    resultModel = new PaymentRequestSendResultModel();
                    string responseContent = await response.Content.ReadAsStringAsync();
                    resultModel.ErrorMessage = responseContent;
                }

                resultModel.IsSuccessStatusCode = response.IsSuccessStatusCode;
                resultModel.StatusCode = (int)response.StatusCode;

                return resultModel;
            }
        }

        public async Task<PaymentRequestStatusCheckResultModel> PaymentRequestsStatusCheckAsync(IList<int> ids)
        {
            if (ids == null) throw new ArgumentNullException(nameof(ids));

            string dataJson = JsonConvert.SerializeObject(new { requestIds = ids });
            (string data, string hmac) = ComputeBodyProps(dataJson, _settings.UseTestEnv);

            // Ако се извика услуга с несъществуващ clientId или ако при извикване на услуга калкулирания 
            // от средата HMAC-SHA256 не съответства на подадения в полето hmac се връща грешка HTTP 401 Unauthorized.
            using (HttpClient client = new HttpClient())
            {
                string serviceVerison = "v1";
                string postUri = $"api/{serviceVerison}/eService/paymentsStatus";
                client.BaseAddress = new Uri(_settings.UseTestEnv ? _settings.TestEnvUrl : _settings.Url);

                // Параметрите се подават в HTTPS POST заявката в Internet media type формат "application/x-www-form-urlencoded; charset=UTF-8".
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                // Извикването на услуги на средата се извършва чрез HTTP POST заявка на адрес посочен в полето URL на услугата. 
                // При всяко извикване на услуга се подават три параметъра:
                // 1.	clientId – уникален идентификатор на клиента на средата
                // 2.	hmac – hmac-sha256 код изчислен върху полето data
                // 3.	data – данни за предаваното съобщение.

                var encodedHttpBody = $"clientId={HttpUtility.UrlEncode(_settings.UseTestEnv ? _settings.TestEnvClientId : _settings.ClientId)}&hmac={HttpUtility.UrlEncode(hmac)}&data={HttpUtility.UrlEncode(data)}";
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateRemoteCertificate);

                var response = await client.PostAsync(postUri, new StringContent(encodedHttpBody, Encoding.UTF8, "application/x-www-form-urlencoded"));

                PaymentRequestStatusCheckResultModel resultModel = new PaymentRequestStatusCheckResultModel(); ;
                resultModel.IsSuccessStatusCode = response.IsSuccessStatusCode;
                resultModel.StatusCode = (int)response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    JObject responseJson = JObject.Parse(responseContent);

                    // acceptedReceiptJson – JSON документ от тип "СЪОБЩЕНИЕ, ЧЕ ЗАЯВКАТА ЗА ПЛАЩАНЕ СЕ ПРИЕМА" (има стойност само ако заявката е успешно приета).
                    IList<PaymentRequestStatusChangeModel> paymentStatuses = responseJson.Properties().ToList().Single(e => e.Name.ToLower() == "paymentStatuses".ToLower()).Value.ToObject<IList<PaymentRequestStatusChangeModel>>();
                    resultModel.Statuses = paymentStatuses;
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    resultModel.ErrorMessage = responseContent;
                }

                return resultModel;
            }
        }

        public async Task<PaymentRequestRefusalResultModel> PaymentRequestRefusalAsync(int id)
        {
            string dataJson = JsonConvert.SerializeObject(new { id });
            (string data, string hmac) = ComputeBodyProps(dataJson, _settings.UseTestEnv);

            // Ако се извика услуга с несъществуващ clientId или ако при извикване на услуга калкулирания 
            // от средата HMAC-SHA256 не съответства на подадения в полето hmac се връща грешка HTTP 401 Unauthorized.
            using (HttpClient client = new HttpClient())
            {
                string serviceVerison = "v1";
                string postUri = $"api/{serviceVerison}/eService/suspendRequest";
                client.BaseAddress = new Uri(_settings.UseTestEnv ? _settings.TestEnvUrl : _settings.Url);

                // Параметрите се подават в HTTPS POST заявката в Internet media type формат "application/x-www-form-urlencoded; charset=UTF-8".
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                // Извикването на услуги на средата се извършва чрез HTTP POST заявка на адрес посочен в полето URL на услугата. 
                // При всяко извикване на услуга се подават три параметъра:
                // 1.	clientId – уникален идентификатор на клиента на средата
                // 2.	hmac – hmac-sha256 код изчислен върху полето data
                // 3.	data – данни за предаваното съобщение.

                var encodedHttpBody = $"clientId={HttpUtility.UrlEncode(_settings.UseTestEnv ? _settings.TestEnvClientId : _settings.ClientId)}&hmac={HttpUtility.UrlEncode(hmac)}&data={HttpUtility.UrlEncode(data)}";
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateRemoteCertificate);

                var response = await client.PostAsync(postUri, new StringContent(encodedHttpBody, Encoding.UTF8, "application/x-www-form-urlencoded"));

                PaymentRequestRefusalResultModel resultModel = new PaymentRequestRefusalResultModel(); ;
                resultModel.IsSuccessStatusCode = response.IsSuccessStatusCode;
                resultModel.StatusCode = (int)response.StatusCode;

                return resultModel;
            }
        }

        public async Task<PaymentRequestSendResultModel> TestPaymentRequestAsync()
        {
            EPaymentRequestModel dataModel = GetTestData();
            return await SendEpaymentRequestAsync(dataModel);
        }

        private (string data, string hmac) ComputeBodyProps(string dataJson, bool isTestEnv)
        {
            // Прави се битова репрезентация с енкодинг UTF-8 на JSON стринга на предаваното съобщение описан в поле Data Params на услугата.
            // Битовата поредица се конвертира като Base64 стринг, който се подава в полето "data"
            string data = Convert.ToBase64String(Encoding.UTF8.GetBytes(dataJson));

            // HMAC-SHA256 кода на предаваното съобщение (data) се изчислява по следния начин:
            // 1.	Прави се битова репрезентация с енкодинг UTF-8 на стринга "таен ключ" (secret) предоставен на клиента.
            // 2.	Прави се битова репрезентация с енкодинг UTF-8 на стринга в полето "data".
            // 3.	Използвайки битовите репрезентации на "тайният ключ" и на "data" се изчислява бинарния HMAC-SHA256 код, като се използва функция за хеширане SHA-256.
            // 4.	Битовата поредица генерирана от алгоритъма се конвертира като Base64 стринг, който се подава в полето "hmac"
            HMACSHA256 hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(isTestEnv ? _settings.TestEnvSecretKey : _settings.SecretKey));
            string hmac = Convert.ToBase64String(hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(data)));

            return (data, hmac);
        }

        private PaymentRequestSendResultModel GetResultModel(string responseContent)
        {
            try
            {
                JObject responseJson = JObject.Parse(responseContent);

                // acceptedReceiptJson – JSON документ от тип "СЪОБЩЕНИЕ, ЧЕ ЗАЯВКАТА ЗА ПЛАЩАНЕ СЕ ПРИЕМА" (има стойност само ако заявката е успешно приета).
                PaymentRequestRejectedModel unacceptedReceipt = responseJson.Properties().ToList().Single(e => e.Name.ToLower() == "unacceptedReceiptJson".ToLower()).Value.ToObject<PaymentRequestRejectedModel>();
                // unacceptedReceiptJson - JSON документ от тип "СЪОБЩЕНИЕ, ЧЕ ЗАЯВКАТА ЗА ПЛАЩАНЕ НЕ СЕ ПРИЕМА" (има стойност само ако заявката не е приета).
                PaymentRequestAcceptedModel acceptedReceipt = responseJson.Properties().ToList().Single(e => e.Name.ToLower() == "acceptedReceiptJson".ToLower()).Value.ToObject<PaymentRequestAcceptedModel>();

                return new PaymentRequestSendResultModel
                {
                    AcceptedReceipt = acceptedReceipt,
                    UnacceptedReceipt = unacceptedReceipt
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private EPaymentRequestModel GetTestData()
        {
            var model = new EPaymentRequestModel
            {
                AisPaymentId = 14,
                ServiceProviderName = "Министерство на правосъдието",
                ServiceProviderBank = "Българска народна банка",
                ServiceProviderBic = "BNBGBGSD",
                ServiceProviderIban = "BG09BNBG96613000173701",
                Currency = "BGN",
                PaymentTypeCode = 1,
                PaymentAmount = 49.00m,
                PaymentReason = "Заплащане на такса",
                ApplicantUinTypeId = (int)Shared.Enums.IdentificationType.EGN,
                ApplicantUin = "7105086301",
                ApplicantName = "Емил Дечев Денчовски",
                ExpirationDate = DateTime.Now.AddDays(5).Date,
                AdministrativeServiceSupplierUri = _settings.AdministrativeServiceSupplierUri,
                AdministrativeServiceNotificationUrl = _settings.AdministrativeServiceNotificationUrl,
                PaymentReferenceType = 1,
                PaymentReferenceNumber = "30",
                PaymentReferenceDate = DateTime.Now.Date,
            };

            return model;
        }

        // Валидираме всички SSL сертификати като валидни
        private bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }

    public class ApplicantData
    {
        public Shared.Enums.IdentificationType applicantIdentifierType { get; set; }
        public string applicantIdentifier { get; set; }
        public string applicantName { get; set; }

        public ApplicantData(Shared.Enums.IdentificationType applicantIdentifierType, string applicantIdentifier, string applicantName)
        {
            this.applicantIdentifierType = applicantIdentifierType;
            this.applicantIdentifier = applicantIdentifier;
            this.applicantName = applicantName;
        }
    }

    public class DecimalFormatConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal) || objectType == typeof(decimal?);
        }

        public override void WriteJson(JsonWriter writer, object value,
                                       JsonSerializer serializer)
        {
            writer.WriteValue(value != null ?
                string.Format("{0:0.00}", value)
                : null);
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType,
                                     object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}
