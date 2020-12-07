using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NRZ.Data;
using NRZ.Data.Extensions;
using NRZ.Models.Company;
using NRZ.Models.CSI;
using NRZ.Models.EPayment;
using NRZ.Models.Person;
using NRZ.Models.Property;
using NRZ.Models.RegiX;
using NRZ.Models.Settings;
using NRZ.RegiX.Client;
using NRZ.RegiX.Client.ResponseModels;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using RegiXServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Serilog;

namespace NRZ.Services.Integration
{
    public class IntegrationService : BaseService, IIntegrationService
    {
        private readonly RegiXCertificateSettings _regixCertificateSettings;
        private readonly IntegrationSettings _integrationSettings;


        public IntegrationService(NRZContext context,
            IOptions<RegiXCertificateSettings> regixSettings,
            IOptions<IntegrationSettings> integrationSettings,
            IStringLocalizer<SharedResources> localizer = null)
            : base(context, localizer)
        {
            _regixCertificateSettings = regixSettings.Value;
            _integrationSettings = integrationSettings.Value;
        }

        public async Task<PropertySearchResultModel> SearchPropertyAsync(Shared.Enums.PropertyType propertyType, PropertySearchRequestModel model)
        {
            PropertySearchResultModel result = new PropertySearchResultModel();
            switch (propertyType)
            {
                case Shared.Enums.PropertyType.AIRCRAFT:
                case Shared.Enums.PropertyType.VEHICLE:
                case Shared.Enums.PropertyType.VESSEL:
                    result = await SearchInRegiXAsync(propertyType, model);
                    break;
                case Shared.Enums.PropertyType.AGRIFORMACHINERY:
                    result = await SearchInAgriculturalRegisterAsync(model);
                    break;
                case Shared.Enums.PropertyType.REALESTATE:
                    // TODO:
                    result = await SearchInNAPAsync(model);
                    break;
                default:
                    break;
            }
            // TODO: handle errors 
            return result;
        }

        #region RegiX integration

        public async Task<PropertySearchResultModel> TestConnectionToRegiXVehicle()
        {
            PropertySearchRequestModel searchModel = new PropertySearchRequestModel()
            {
                Identifier = "test123",
                IdentifierTypeCode = "REGNUMBER",
                SuitNumber = "test suit 123"
            };

            Shared.Enums.PropertyType propertyType = Shared.Enums.PropertyType.VEHICLE;

            PropertySearchResultModel result = await SearchInRegiXAsync(propertyType, searchModel);

            return result;
        }

        public async Task<PropertySearchResultModel> TestConnectionToRegiXVessel()
        {
            PropertySearchRequestModel searchModel = new PropertySearchRequestModel()
            {
                Identifier = "8001010101",
                IdentifierTypeCode = "OWNER",
                SuitNumber = "test suit 123"
            };

            Shared.Enums.PropertyType propertyType = Shared.Enums.PropertyType.VESSEL;

            PropertySearchResultModel result = await SearchInRegiXAsync(propertyType, searchModel);

            return result;
        }

        public async Task<PropertySearchResultModel> TestConnectionToRegiXAircraft()
        {
            PropertySearchRequestModel searchModel = new PropertySearchRequestModel()
            {
                Identifier = "1234567",
                IdentifierTypeCode = "MSN",
                SuitNumber = "test suit 123"
            };

            Shared.Enums.PropertyType propertyType = Shared.Enums.PropertyType.AIRCRAFT;

            PropertySearchResultModel result = await SearchInRegiXAsync(propertyType, searchModel);

            return result;
        }

        private async Task<PropertySearchResultModel> SearchInRegiXAsync(Shared.Enums.PropertyType propertyType, PropertySearchRequestModel searchModel)
        {
            if (!_integrationSettings.UseRegiX)
            {
                throw new Exception("Integration with RegiX is not configured!");
            }

            if (searchModel == null || String.IsNullOrWhiteSpace(searchModel.IdentifierTypeCode) || String.IsNullOrWhiteSpace(searchModel.Identifier))
            {
                throw new Exception("Property search criteria is missing");
            }

            RegiXReportModel report = await GetRegiXReportForPropertyType(propertyType, searchModel);
            if (report == null)
            {
                throw new Exception("RegiX report configuration not found for property type: " + propertyType);
            }

            ServiceRequestData request = GetServiceRequestData(propertyType, searchModel, report);
            if (request == null)
            {
                throw new Exception("Property service request was not created");
            }

            long requestId = await SaveRegiXRequest(request, report);

            // TODO: is context needed for transfering eAuth and certificate info?
            CustomCallContext context = new CustomCallContext();
            RegiXResponse response = await CallRegiXAsync(context, request);

            await SaveRegiXResponse(requestId, response, "");

            BaseResponse parsedObject = GetResponseObject(propertyType, response);

            PropertySearchResultModel resultModel = new PropertySearchResultModel
            {
                PropertyIdentifier = searchModel.Identifier,
                RequestId = requestId,
                ResponseObject = parsedObject
            };

            return resultModel;
        }


        private async Task<RegiXReportModel> GetRegiXReportForPropertyType(Shared.Enums.PropertyType propertyType, PropertySearchRequestModel searchModel)
        {
            RegiXreportToPropertyType reportType = await _context.RegiXreportToPropertyType
                .Where(x => x.RegiXsearchCriteriaTypeCode == searchModel.IdentifierTypeCode.ToUpper() &&
                            x.PropertyTypeCode == propertyType.ToString())
                .FirstOrDefaultAsync();

            if (reportType == null)
                return null;

            RegiXReportModel report = await _context.RegiXreport
                            .Where(x => x.Id == reportType.RegiXreportId)
                            .Select(x => x.ToModel())
                            .FirstOrDefaultAsync();

            return report;
        }


        private ServiceRequestData GetServiceRequestData(Shared.Enums.PropertyType propertyType, PropertySearchRequestModel model, RegiXReportModel report)
        {
            ServiceRequestData request = null;
            if (propertyType == Shared.Enums.PropertyType.VEHICLE)
            {
                request = GetMotorVehicleRegistrationInfoV3Request(model.Identifier, report);
            }
            else if (propertyType == Shared.Enums.PropertyType.AIRCRAFT)
            {
                if (String.Equals(model.IdentifierTypeCode.ToUpper(), "MSN"))
                    request = GetAircraftsByMSNRequest(model.Identifier, report);
                else if (String.Equals(model.IdentifierTypeCode.ToUpper(), "OWNER"))
                    request = GetAircraftsByOwnerRequest(model.Identifier, report);
            }
            else if (propertyType == Shared.Enums.PropertyType.VESSEL)
            {
                request = GetVesselsByOwnerRequest(model.Identifier, report);
            }

            return request;
        }


        /// <summary>
        /// Разширена справка за МПС по регистрационен номер - V3 (Регистър на моторните превозни средства/МВР)
        /// </summary>
        /// <returns></returns>
        private ServiceRequestData GetMotorVehicleRegistrationInfoV3Request(string propertyIdentifier, RegiXReportModel report)
        {
            CallContext callContext = GetCallContext();

            // TODO: build xml
            XmlDocument doc = new XmlDocument();
            string xml = "";
            string operation = "";

            //xml = @"<GetMotorVehicleRegistrationInfoV2Request 
            //        xsi:schemaLocation=""http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV2Request GetMotorVehicleRegistrationInfoV2Request.xsd"" 
            //        xmlns=""http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV2Request"" 
            //        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
            //         <Identifier>CA0000CA</Identifier>
            //        </GetMotorVehicleRegistrationInfoV2Request>";
            if (_regixCertificateSettings.UseVehicleV3)
            {
                xml = @"<GetMotorVehicleRegistrationInfoV3Request 
                            xsi:schemaLocation=""http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Request GetMotorVehicleRegistrationInfoV3Request.xsd"" 
                            xmlns=""http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Request"" 
                            xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
                             <Identifier>" + propertyIdentifier + @"</Identifier>
                            </GetMotorVehicleRegistrationInfoV3Request>";
                operation = "TechnoLogica.RegiX.MVRMPSAdapter.APIService.IMVRMPSAPI.GetMotorVehicleRegistrationInfoV3";
            }
            else
            {
                xml = @"<MotorVehicleRegistrationRequest 
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                    xmlns=""http://egov.bg/RegiX/MVR/MPS/MotorVehicleRegistrationRequest"">
                    <Identifier>" + propertyIdentifier + @"</Identifier>
                  </MotorVehicleRegistrationRequest>
";
                operation = "TechnoLogica.RegiX.MVRMPSAdapter.APIService.IMVRMPSAPI.GetMotorVehicleRegistrationInfo";
            }

            doc.LoadXml(xml);
            XmlElement argument = doc.DocumentElement;

            ServiceRequestData request = GetServiceRequest(operation, argument, callContext);

            return request;
        }

        /// <summary>
        /// Справка по сериен номер на въздухоплавателно средство за вписани в регистъра обстоятелства (Регистър на гражданските въздухоплавателни средства на Република България/ГВА)
        /// </summary>
        /// <returns></returns>
        private ServiceRequestData GetAircraftsByMSNRequest(string propertyIdentifier, RegiXReportModel report)
        {
            CallContext callContext = GetCallContext();

            // TODO: build xml 
            XmlDocument doc = new XmlDocument();
            string xml = @"<AircraftsByMSNRequest 
                        xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                        xmlns=""http://egov.bg/RegiX/GVA/AircraftsByMSNRequest"">
                        <MSN>" + propertyIdentifier + @"</MSN>
                      </AircraftsByMSNRequest>";
            doc.LoadXml(xml);
            XmlElement argument = doc.DocumentElement;

            string operation = report.Operation;
            ServiceRequestData request = GetServiceRequest(operation, argument, callContext);

            return request;
        }

        /// <summary>
        /// Справка по ЕИК/БУЛСТАТ/ЕГН/ЛНЧ за вписани на името на лицето въздухоплавателни средства (Регистър на гражданските въздухоплавателни средства на Република България/ГВА)
        /// </summary>
        /// <returns></returns>
        private ServiceRequestData GetAircraftsByOwnerRequest(string ownerIdentifier, RegiXReportModel report)
        {
            CallContext callContext = GetCallContext();

            // TODO: build xml
            XmlDocument doc = new XmlDocument();
            string xml = @"<AircraftsByOwnerRequest 
                        xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                        xmlns=""http://egov.bg/RegiX/GVA/AircraftsByOwnerRequest"">
                        <OwnerID>" + ownerIdentifier + @"</OwnerID>
                        <DateFrom>2000-01-01</DateFrom>
                        <DateTo>2016-01-01</DateTo>
                      </AircraftsByOwnerRequest>";
            doc.LoadXml(xml);
            XmlElement argument = doc.DocumentElement;

            string operation = report.Operation;
            ServiceRequestData request = GetServiceRequest(operation, argument, callContext);


            return request;
        }

        private ServiceRequestData GetVesselsByOwnerRequest(string ownerIdentifier, RegiXReportModel report)
        {
            CallContext callContext = GetCallContext();

            // TODO: build xml
            XmlDocument doc = new XmlDocument();
            string xml = @"<RegistrationInfoByOwnerRequest 
                        xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                        xmlns=""http://egov.bg/RegiX/IAMA/RegistrationInfoByOwnerRequest"">
                        <Identifier>" + ownerIdentifier + @"</Identifier>
                        </RegistrationInfoByOwnerRequest> ";
            doc.LoadXml(xml);
            XmlElement argument = doc.DocumentElement;

            string operation = report.Operation;
            ServiceRequestData request = GetServiceRequest(operation, argument, callContext);


            return request;
        }

        private CallContext GetCallContext()
        {
            CallContext callContext = new CallContext
            {
                AdministrationName = "Администрация",
                AdministrationOId = "1.2.3.4.5.6.7.8.9",
                EmployeeIdentifier = "myusername@administration.domain",
                EmployeeNames = "Първо Второ Трето",
                EmployeePosition = "Експерт в отдел",
                LawReason = "На основание чл. X от Наредба/Закон/Нормативен акт",
                ServiceURI = "123-12345-01.01.2017",
                ServiceType = "За административна услуга",
                Remark = "За тестване на системата",
                EmployeeAditionalIdentifier = null,
                ResponsiblePersonIdentifier = null
            };

            return callContext;
        }

        private ServiceRequestData GetServiceRequest(string operation, XmlElement argument, CallContext callContext)
        {
            ServiceRequestData request = new ServiceRequestData
            {
                Operation = operation,
                Argument = argument,
                CallContext = callContext,
                CitizenEGN = null,
                EmployeeEGN = null,
                ReturnAccessMatrix = false,
                SignResult = true,
                CallbackURL = null,
                EIDToken = null
            };

            return request;
        }

        private async Task<long> SaveRegiXRequest(ServiceRequestData request, RegiXReportModel report)
        {
            RegiXrequest entity = new RegiXrequest
            {
                UserId = null,
                RegiXreportId = report.Id,
                Argument = request.Argument.OuterXml,
                CreatedAtUtc = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
            };

            await _context.RegiXrequest.AddAsync(entity);
            await _context.SaveChangesAsync();

            long requestId = entity.Id;
            return requestId;
        }

        private async Task SaveRegiXResponse(long requestId, RegiXResponse rawResponse, string errors)
        {
            RegiXrequest request = await _context.RegiXrequest
                .Where(x => x.Id == requestId)
                .FirstOrDefaultAsync();

            if (request == null)
                throw new Exception("No request found for response!");

            request.AnsweredAtUtc = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            request.Errors = errors;

            RegiXresponse response = new RegiXresponse
            {
                RequestId = requestId,
                RawContent = rawResponse.RawResponse.MessageContent
            };

            _context.RegiXrequest.Update(request);
            await _context.RegiXresponse.AddAsync(response);
            await _context.SaveChangesAsync();

        }

        private async Task<RegiXResponse> CallRegiXAsync(CustomCallContext context, ServiceRequestData request)
        {
            try
            {
                RegiXEntryPointClient client = CreateRegiXEntryPointClient();

                // Ако не е подаден сертификат, се използва този от конфигурационния файл.            
                X509CertificateInitiatorClientCredential clientCert = client.ClientCredentials.ClientCertificate;
                if (context.Certificate != null)
                {
                    clientCert.Certificate = context.Certificate;
                }
                else if (clientCert.Certificate == null)
                {
                    throw new Exception("Не е указан сертификат за достъп до RegiX.");
                }

                Log.Information($"IntegrationService/CallRegiXAsync - certificate thumbprint: {clientCert.Certificate?.Thumbprint}");

                RegiXResponse response = await RegiXUtility.CallAsync(client, request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private RegiXEntryPointClient CreateRegiXEntryPointClient()
        {
            WSHttpBinding binding = new WSHttpBinding(SecurityMode.Transport);
            binding.Name = _regixCertificateSettings.BindingName;
            binding.MaxReceivedMessageSize = _regixCertificateSettings.MaxReceivedMessageSize;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            EndpointAddress address = new EndpointAddress(_regixCertificateSettings.EndpointAddress);

            RegiXEntryPointClient client = new RegiXEntryPointClient(binding, address);
            client.ClientCredentials.ClientCertificate.SetCertificate(
                StoreLocation.LocalMachine,
                StoreName.My,
                X509FindType.FindByThumbprint,
                _regixCertificateSettings.CertificateThumbprint);

            RegiXEndpointBehavior behavior = new RegiXEndpointBehavior();
            client.Endpoint.EndpointBehaviors.Add(behavior);

            return client;

        }

        private BaseResponse GetResponseObject(Shared.Enums.PropertyType propertyType, RegiXResponse response)
        {
            BaseResponse parsedObject = null;
            switch (propertyType)
            {
                case Shared.Enums.PropertyType.AIRCRAFT:
                    parsedObject = XsdToObjectUtility.GetResponseObjectFromXsd<AircraftsResponse>(response);
                    break;
                case Shared.Enums.PropertyType.VEHICLE:
                    if (_regixCertificateSettings.UseVehicleV3)
                    {
                        #region VehicleV3Example
                        //    string content = @"<?xml version=""1.0"" encoding=""utf-16""?>
                        //<s:Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:a=""http://www.w3.org/2005/08/addressing"">
                        //  <s:Header>
                        //    <a:Action s:mustUnderstand=""1"">http://tempuri.org/IRegiXEntryPoint/ExecuteSynchronousResponse</a:Action>
                        //    <ActivityId CorrelationId=""ca165853-c30d-4f6c-a10b-97604a5c2ca8"" xmlns=""http://schemas.microsoft.com/2004/09/ServiceModel/Diagnostics"">00000000-0000-0000-0000-000000000000</ActivityId>
                        //    <a:RelatesTo>urn:uuid:973e940f-0520-4006-a53e-b21090539887</a:RelatesTo>
                        //  </s:Header>
                        //  <s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                        //    <ExecuteSynchronousResponse xmlns=""http://tempuri.org/"">
                        //      <ExecuteSynchronousResult>
                        //        <IsReady>true</IsReady>
                        //        <Data id=""data"">
                        //          <Request id=""request"">
                        //            <GetMotorVehicleRegistrationInfoV3Request xmlns=""http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Request"">
                        //              <Identifier>test123</Identifier>
                        //            </GetMotorVehicleRegistrationInfoV3Request>
                        //          </Request>
                        //          <Response id=""response"">
                        //            <GetMotorVehicleRegistrationInfoV3Response xmlns=""http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response"">
                        //              <Header>
                        //                <MessageID>7e5361c4-3867-403d-9158-02aa848cf562</MessageID>
                        //                <MessageRefID>c384287d-19fb-4512-b64c-7b2062360a4a</MessageRefID>
                        //                <DateTime>2020-01-27T16:47:12.795</DateTime>
                        //                <Operation>003</Operation>
                        //              </Header>
                        //              <Response>
                        //                <Results>
                        //                  <Result>
                        //                    <VehicleData>
                        //                      <RegistrationNumber>СТ0503СК</RegistrationNumber>
                        //                      <FirstRegistrationDate>1986-11-10</FirstRegistrationDate>
                        //                      <VIN>WVWZZZ19ZHW316647</VIN>
                        //                      <EngineNumber>JP466028</EngineNumber>
                        //                      <VehicleType>ЛЕК АВТОМОБИЛ</VehicleType>
                        //                      <Model>ФОЛКСВАГЕН ГОЛФ</Model>
                        //                      <Color>МАСЛЕНО ЗЕЛЕН</Color>
                        //                      <Category>M1</Category>
                        //                      <MassG>980</MassG>
                        //                      <MassF1>1430</MassF1>
                        //                      <MassF2>1430</MassF2>
                        //                      <MassF3>2780</MassF3>
                        //                      <VehNumOfAxles>2</VehNumOfAxles>
                        //                      <VehMassO1>2812</VehMassO1>
                        //                      <VehMassO2>2823</VehMassO2>
                        //                      <Capacity>1600</Capacity>
                        //                      <MaxPower>40</MaxPower>
                        //                      <Fuel>ДИЗЕЛ</Fuel>
                        //                      <EnvironmentalCategory>EURO 1</EnvironmentalCategory>
                        //                      <VehicleDocument>
                        //                        <VehDocumentNumber>008072092</VehDocumentNumber>
                        //                        <VehDocumentDate>2018-01-02</VehDocumentDate>
                        //                      </VehicleDocument>
                        //                    </VehicleData>
                        //                    <OwnersData>
                        //                      <Owner>
                        //                        <BulgarianCitizen>
                        //                          <PIN>5409234132</PIN>
                        //                          <Names>
                        //                            <First>МАРИЯНА</First>
                        //                            <Surname>СТОЯНОВА</Surname>
                        //                            <Family>АРНАУДОВА</Family>
                        //                          </Names>
                        //                        </BulgarianCitizen>
                        //                        <ForeignCitizen />
                        //                        <Company />
                        //                      </Owner>
                        //                    </OwnersData>
                        //                  </Result>
                        //                </Results>
                        //                <ReturnInformation>
                        //                  <ReturnCode>0</ReturnCode>
                        //                  <Info>OK</Info>
                        //                </ReturnInformation>
                        //              </Response>
                        //            </GetMotorVehicleRegistrationInfoV3Response>
                        //          </Response>
                        //          <Matrix xsi:nil=""true"" />
                        //        </Data>
                        //        <HasError>false</HasError>
                        //        <Error xsi:nil=""true"" />
                        //      </ExecuteSynchronousResult>
                        //    </ExecuteSynchronousResponse>
                        //  </s:Body>
                        //</s:Envelope>";

                        #endregion
                        parsedObject = XsdToObjectUtility.GetResponseObjectFromXsd<GetMotorVehicleRegistrationInfoV3Response>(response);
                    }
                    else
                    {
                        parsedObject = XsdToObjectUtility.GetResponseObjectFromXsd<MotorVehicleRegistrationResponse>(response);
                    }

                    break;
                case Shared.Enums.PropertyType.VESSEL:
                    parsedObject = XsdToObjectUtility.GetResponseObjectFromXsd<RegistrationInfoByOwnerResponse>(response);
                    break;
                default:
                    break;
            }

            return parsedObject;
        }

        public async Task<PersonSearchResultModel> GetPersonFromRegiXAsync(string identifier)
        {
            if (!_integrationSettings.UseRegiX)
            {
                throw new Exception("Integration with RegiX is not configured!");
            }

            if (String.IsNullOrWhiteSpace(identifier))
            {
                throw new Exception("Person identifier is missing");
            }

            RegiXReportModel report = await GetRegiXReportForPersonValidation();
            if (report == null)
            {
                throw new Exception("RegiX report configuration not found for person validation");
            }

            ServiceRequestData request = GetServiceRequestDataForPerson(report, identifier);
            if (request == null)
            {
                throw new Exception("Person service request was not created");
            }

            // TODO: user, timestamp
            long requestId = await SaveRegiXRequest(request, report);

            // TODO: is context needed for transfering eAuth and certificate info?
            CustomCallContext context = new CustomCallContext();
            RegiXResponse response = await CallRegiXAsync(context, request);

            // TODO: errors 
            await SaveRegiXResponse(requestId, response, "");

            BaseResponse parsedObject = XsdToObjectUtility.GetResponseObjectFromXsd<ValidPersonResponse>(response);


            PersonSearchResultModel resultModel = new PersonSearchResultModel
            {
                PersonIdentifier = identifier,
                RequestId = requestId,
                ResponseObject = parsedObject
            };

            return resultModel;
        }

        private async Task<RegiXReportModel> GetRegiXReportForPersonValidation()
        {
            // TODO: configure operation name?
            RegiXReportModel report = await _context.RegiXreport
                            .Where(x => x.OperationName == "ValidPersonSearch")
                            .Select(x => x.ToModel())
                            .FirstOrDefaultAsync();

            return report;
        }

        private ServiceRequestData GetServiceRequestDataForPerson(RegiXReportModel report, string identifier)
        {
            CallContext callContext = GetCallContext();

            // TODO: build xml
            XmlDocument doc = new XmlDocument();
            string xml = @"<ValidPersonRequest xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                        xmlns=""http://egov.bg/RegiX/GRAO/NBD/ValidPersonRequest"">
                        <EGN>" + identifier + @"</EGN>
                        </ValidPersonRequest>";

            doc.LoadXml(xml);
            XmlElement argument = doc.DocumentElement;

            string operation = report.Operation;
            ServiceRequestData request = GetServiceRequest(operation, argument, callContext);

            return request;
        }


        public async Task<CompanySearchResultModel> GetCompanyFromRegiXAsync(string identifier)
        {
            if (!_integrationSettings.UseRegiX)
            {
                throw new Exception("Integration with RegiX is not configured!");
            }

            if (String.IsNullOrWhiteSpace(identifier))
            {
                throw new Exception("Company identifier is missing");
            }

            RegiXReportModel report = await GetRegiXReportForCompanyValidation();
            if (report == null)
            {
                throw new Exception("RegiX report configuration not found for company validation");
            }

            ServiceRequestData request = GetServiceRequestDataForCompany(report, identifier);
            if (request == null)
            {
                throw new Exception("Company service request was not created");
            }

            // TODO: user, timestamp
            long requestId = await SaveRegiXRequest(request, report);

            // TODO: is context needed for transfering eAuth and certificate info?
            CustomCallContext context = new CustomCallContext();
            RegiXResponse response = await CallRegiXAsync(context, request);

            // TODO: errors 
            await SaveRegiXResponse(requestId, response, "");

            BaseResponse parsedObject = XsdToObjectUtility.GetResponseObjectFromXsd<ValidUICResponse>(response);


            CompanySearchResultModel resultModel = new CompanySearchResultModel
            {
                CompanyIdentifier = identifier,
                RequestId = requestId,
                ResponseObject = parsedObject
            };

            return resultModel;
        }

        private async Task<RegiXReportModel> GetRegiXReportForCompanyValidation()
        {
            // TODO: configure operation name?
            RegiXReportModel report = await _context.RegiXreport
                            .Where(x => x.OperationName == "GetValidUICInfo")
                            .Select(x => x.ToModel())
                            .FirstOrDefaultAsync();

            return report;
        }

        private ServiceRequestData GetServiceRequestDataForCompany(RegiXReportModel report, string identifier)
        {
            CallContext callContext = GetCallContext();

            // TODO: build xml
            XmlDocument doc = new XmlDocument();
            string xml = @"<ValidUICRequest xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                            xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                            xmlns=""http://egov.bg/RegiX/AV/TR/ValidUICRequest"">
                            <UIC>" + identifier + @"</UIC>
                            </ValidUICRequest>";

            doc.LoadXml(xml);
            XmlElement argument = doc.DocumentElement;

            string operation = report.Operation;
            ServiceRequestData request = GetServiceRequest(operation, argument, callContext);

            return request;
        }

        #endregion


        #region Agriculture integration

        private async Task<PropertySearchResultModel> SearchInAgriculturalRegisterAsync(PropertySearchRequestModel model)
        {
            AgriculturalMachineryCollectionModel list = new AgriculturalMachineryCollectionModel();
            var entities = await _context.AgriculturalMachinery
                        .Include(x => x.Owner)
                        .Include(x => x.Company)
                        .Where(x => x.RegistrationNumber.ToLower().Contains(model.Identifier.Trim().ToLower()))
                        .Select(x => x.ToViewModel())
                        .ToListAsync();

            list.Machines = entities?.ToList();

            PropertySearchResultModel result = new PropertySearchResultModel();
            result.PropertyIdentifier = model.Identifier;
            result.ResponseObject = list;
            return result;
        }
        #endregion

        #region NAP integration

        private async Task<PropertySearchResultModel> SearchInNAPAsync(PropertySearchRequestModel model)
        {
            // TODO: call NAP?
            return new PropertySearchResultModel();
        }
        #endregion

        #region CSI

        //Търсене на информация за ЧСИ по регистрационен номер и дата
        public async Task<CSIModel> GetCSIByNumberAndDate(string csiNumber, DateTime date)
        {
            var httpClient = new HttpClient();

            UriBuilder builder = new UriBuilder("https://register.bcpea.org/api/findAgentByDateAndNumber");
            System.Collections.Specialized.NameValueCollection collection = System.Web.HttpUtility.ParseQueryString(string.Empty);

            collection.Add("number", csiNumber);
            collection.Add("date", date.ToString("dd-MM-yyyy"));

            builder.Query = collection.ToString();

            var response = await httpClient.GetAsync(builder.Uri);

            if (response.IsSuccessStatusCode)
            {
                var contents = await response.Content.ReadAsStringAsync();
                CSIModel csi = JsonConvert.DeserializeObject<CSIModel>(contents);
                return csi;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            else {
                throw new Exception("Error searching in CHSI register");
            }            
        }

        //Търсене на информация за ЧСИ по регистрационен номер и помощници
        public async Task<CSIModel> GetCSIByNumber(string csiNumber, bool helpers = false)
        {
            var httpClient = new HttpClient();

            UriBuilder builder = new UriBuilder("https://register.bcpea.org/api/findAgentByNumber");
            System.Collections.Specialized.NameValueCollection collection = System.Web.HttpUtility.ParseQueryString(string.Empty);

            collection.Add("number", csiNumber);

            if (helpers == true)
            {
                collection.Add("helpers", "true");
            }

            builder.Query = collection.ToString();

            var response = await httpClient.GetAsync(builder.Uri);
            var contents = await response.Content.ReadAsStringAsync();

            CSIModel csi = JsonConvert.DeserializeObject<CSIModel>(contents);

            return csi;
        }

        #endregion

        #region EPayments


        #endregion


    }
}
