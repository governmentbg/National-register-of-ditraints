using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NRZ.Models.RequestForCertificateOfDistraintOfProperty;
using NRZ.Models.EPayment;
using NRZ.Data.Extensions;

namespace NRZ.Services
{
    public class RequestForCertificateOfDistraintOfPropertyService : BaseService, IRequestForCertificateOfDistraintOfPropertyService
    {
        private readonly IEPaymentService _ePaymentService;

        public RequestForCertificateOfDistraintOfPropertyService(NRZContext context,
            IEPaymentService ePaymentService,
            IStringLocalizer<SharedResources> localizer = null)
            : base(context, localizer)
        {
            _ePaymentService = ePaymentService;
        }

        private const string UserTypeRequester = "REQUESTER";

        public async Task<(bool hasToPay, PaymentRequestSendResultModel paymentRequestModel, RequestForCertificateOfDistraintOfProperty request)> CreateAsync(RequestModel model, string currentUserId)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    RequestForCertificateOfDistraintOfProperty entry = await SaveRequestForCertificateOfDistraintOfProperty(model);
                    bool hasToPay = HasToPay(model, currentUserId);
                    if (!HasToPay(model, currentUserId))
                    {
                        // Не се изисква плащане. Връщаме резултат
                        return (hasToPay, paymentRequestModel: null, entry);
                    }

                    PaymentRequestSendResultModel paymentRequestModel = await CreateTaxMeRequest(entry.Id);
                    if (paymentRequestModel == null)
                    {
                        // Неуспешено изпращане на заявка за ел.плащане в системата за ел.плащане.
                        await tran.RollbackAsync();
                        return (hasToPay, paymentRequestModel, entry);
                    }

                    var eservicePaymentRequest = await _context.EservicePaymentRequest.Where(x => x.SeizedPropertyCertificateRequestId == entry.Id).FirstOrDefaultAsync();
                    if (eservicePaymentRequest != null)
                    {
                        // Запис на резултат от изпращането на заявка за ел.плащане.
                        DateTime utcNow = DateTime.UtcNow;
                        EservicePaymentRequestStatusHistory historyRecord = new EservicePaymentRequestStatusHistory
                        {
                            RequestId = eservicePaymentRequest.Id,
                            UpdatedAt = utcNow
                        };
                        eservicePaymentRequest.UpdatedAt = utcNow;

                        if (!paymentRequestModel.IsSuccessStatusCode)
                        {
                            eservicePaymentRequest.StatusCode = Shared.Enums.PaymentRequestStatus.ERROR.ToString();
                            historyRecord.StatusCode = Shared.Enums.PaymentRequestStatus.ERROR.ToString();
                            historyRecord.Errors = paymentRequestModel.ErrorMessage;
                        } else
                        {

                            if (paymentRequestModel.UnacceptedReceipt != null)
                            {
                                eservicePaymentRequest.StatusCode = Shared.Enums.PaymentRequestStatus.REJECTED.ToString();
                                historyRecord.StatusCode = Shared.Enums.PaymentRequestStatus.REJECTED.ToString();
                                historyRecord.EserviceTime = paymentRequestModel.UnacceptedReceipt.ValidationTime;
                            }
                            else
                            {
                                eservicePaymentRequest.StatusCode = Shared.Enums.PaymentRequestStatus.PENDING.ToString();
                                eservicePaymentRequest.AisPaymentId = paymentRequestModel.AcceptedReceipt.Id;
                                historyRecord.StatusCode = Shared.Enums.PaymentRequestStatus.PENDING.ToString();
                                historyRecord.EserviceTime = paymentRequestModel.AcceptedReceipt.RegistrationTime;
                            }
                        }

                        _context.EservicePaymentRequestStatusHistory.Add(historyRecord);
                        await _context.SaveChangesAsync();
                    }

                    await tran.CommitAsync();
                    return (hasToPay, paymentRequestModel, entry);
                }
                catch (Exception e)
                {
                    await tran.RollbackAsync();
                    throw e;
                }
            }

        }

        private async Task<PaymentRequestSendResultModel> CreateTaxMeRequest(int certificateOfDistraintOfPropertyRequestId)
        {
            EServicePaymentRequestCreateModel result = await _ePaymentService.GeneratePaymentRequestAsync(Shared.Enums.EServiceType.SEIZEDPROPERTYCERTIFICATE, certificateOfDistraintOfPropertyRequestId);

            PaymentRequest request = await _context.PaymentRequest.Where(x => x.AisPaymentId == result.PaymentRequestId).SingleOrDefaultAsync();
            EPaymentRequestModel model = request.ToEpaymentModel();
            PaymentRequestSendResultModel sendResultModel = await _ePaymentService.SendEpaymentRequestAsync(model ?? throw new ArgumentNullException(nameof(result)));

            return sendResultModel;
        }

        private async Task<RequestForCertificateOfDistraintOfProperty> SaveRequestForCertificateOfDistraintOfProperty(RequestModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var arePersonFieldsReceived = !(model.FirstName == null || model.LastName == null || model.IsPersonalIdentifierTypeLNCh == null || model.PersonalIdentifier == null
                || model.RegionId == null || model.MunicipalityId == null || model.CityId == null || model.StreetAddress == null
                || model.Email == null);
            var areLegalEntityFieldsReceived = !(model.NameOfLegalEntity == null || model.IdentifierOfLegalEntity == null
                || model.RegionIdOfLegalEntity == null || model.MunicipalityIdOfLegalEntity == null || model.CityIdOfLegalEntity == null || model.StreetAddressOfLegalEntity == null
                || model.StreetAddressOfLegalEntity == null || model.PropertyTypeCode == null);

            if (!arePersonFieldsReceived && (model.UserTypeCode == Shared.Enums.UserType.AUCPAR.ToString() || model.UserTypeCode == UserTypeRequester))
            {
                throw new Exception("Not all fields related to person received");
            }
            if (!areLegalEntityFieldsReceived && (model.UserTypeCode != Shared.Enums.UserType.AUCPAR.ToString() && model.UserTypeCode != UserTypeRequester))
            {
                throw new Exception("Not all fields related to legal entity received");
            }

            int? propertyIdVehicle = null;
            int? propertyIdAircraft = null;
            int? propertyIdVessel = null;
            int? propertyIdAgriForMachinery = null;
            int? propertyIdRealEstate = null;
            int? propertyIdOtherProperty = null;
            switch ((Shared.Enums.PropertyType)Enum.Parse(typeof(Shared.Enums.PropertyType), model.PropertyTypeCode, true))
            {
                case Shared.Enums.PropertyType.OTHER:
                    propertyIdOtherProperty = model.PropertyId;
                    break;
                case Shared.Enums.PropertyType.VEHICLE:
                    propertyIdVehicle = model.PropertyId;
                    break;
                case Shared.Enums.PropertyType.AIRCRAFT:
                    propertyIdAircraft = model.PropertyId;
                    break;
                case Shared.Enums.PropertyType.VESSEL:
                case Shared.Enums.PropertyType.REALESTATE:
                case Shared.Enums.PropertyType.AGRIFORMACHINERY:
                    break;
                default:
                    break;
            }

            RequestForCertificateOfDistraintOfProperty newRequest = new RequestForCertificateOfDistraintOfProperty
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                IsPersonalIdentifierTypeLnch = model.IsPersonalIdentifierTypeLNCh,
                PersonalIdentifier = model.PersonalIdentifier,
                RegionId = model.RegionId,
                MunicipalityId = model.MunicipalityId,
                CityId = model.CityId,
                StreetAddress = model.StreetAddress,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                UserTypeCode = model.UserTypeCode,
                NameOfLegalEntity = model.NameOfLegalEntity,
                IdentifierOfLegalEntity = model.IdentifierOfLegalEntity,
                RegionIdOfLegalEntity = model.RegionIdOfLegalEntity,
                MunicipalityIdOfLegalEntity = model.MunicipalityIdOfLegalEntity,
                CityIdOfLegalEntity = model.CityIdOfLegalEntity,
                StreetAddressOfLegalEntity = model.StreetAddressOfLegalEntity,
                CompanyCaseNumber = model.CompanyCaseNumber,
                PropertyTypeCode = model.PropertyTypeCode,
                PropertyIdVehicle = propertyIdVehicle,
                PropertyIdAircraft = propertyIdAircraft,
                PropertyIdVessel = propertyIdVessel,
                PropertyIdAgriForMachinery = propertyIdAgriForMachinery,
                PropertyIdRealEstate = propertyIdRealEstate,
                PropertyIdOtherProperty = propertyIdOtherProperty,
                CreatedOn = DateTime.UtcNow
            };

            _context.RequestForCertificateOfDistraintOfProperty.Add(newRequest);
            await _context.SaveChangesAsync();

            return newRequest;
        }

        public IQueryable<RequestViewModel> GetAll()
        {
            var listQuery = _context.RequestForCertificateOfDistraintOfProperty
                .Include(x => x.Region)
                .Include(x => x.Municipality)
                .Include(x => x.City)
                .Include(x => x.RegionIdOfLegalEntityNavigation)
                .Include(x => x.MunicipalityIdOfLegalEntityNavigation)
                .Include(x => x.CityIdOfLegalEntityNavigation)
                .Include(x => x.PropertyIdOtherPropertyNavigation)
                .Include(x => x.PropertyIdVehicleNavigation)
                .Include(x => x.PropertyIdAircraftNavigation)
                .OrderByDescending(x => x.Id)
                .AsNoTracking()
                .Select(x => new RequestViewModel(
                    x.Id,
                    x.FirstName,
                    x.MiddleName,
                    x.LastName,
                    x.IsPersonalIdentifierTypeLnch,
                    x.PersonalIdentifier,
                    x.Region.Name,
                    x.Municipality.Name,
                    x.City.Name,
                    x.StreetAddress,
                    x.PhoneNumber,
                    x.Email,
                    x.UserTypeCode,
                    x.NameOfLegalEntity,
                    x.IdentifierOfLegalEntity,
                    x.RegionIdOfLegalEntityNavigation.Name,
                    x.MunicipalityIdOfLegalEntityNavigation.Name,
                    x.CityIdOfLegalEntityNavigation.Name,
                    x.StreetAddressOfLegalEntity,
                    x.CompanyCaseNumber,
                    x.PropertyTypeCodeNavigation.Name,
                    x.PropertyTypeCodeNavigation.NameEn,
                    x.PropertyIdOtherProperty,
                    x.PropertyIdVehicle,
                    x.PropertyIdAircraft,
                    x.PropertyIdOtherPropertyNavigation.Identifier,
                    x.PropertyIdVehicleNavigation.RegistrationNumber,
                    x.PropertyIdAircraftNavigation.MsnserialNumber,
                    x.CreatedOn,
                    x.EservicePaymentRequest.Any() ? x.EservicePaymentRequest.FirstOrDefault().StatusCode : "",
                    x.EservicePaymentRequest.Any() && x.EservicePaymentRequest.FirstOrDefault().StatusCodeNavigation != null ? x.EservicePaymentRequest.FirstOrDefault().StatusCodeNavigation.Name : "",
                    x.EservicePaymentRequest.Any() && x.EservicePaymentRequest.FirstOrDefault().StatusCodeNavigation != null ? x.EservicePaymentRequest.FirstOrDefault().StatusCodeNavigation.NameEn : ""
                ));

            return listQuery;
        }

        private bool HasToPay(RequestModel model, string currentUserId)
        {
            if (string.IsNullOrWhiteSpace(currentUserId)) return true;

            // Todo: Да се прецезира кога трябва да се плаща
            return false;
        }
    }
}
