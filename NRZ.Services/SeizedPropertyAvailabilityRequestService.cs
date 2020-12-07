using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Data.Extensions;
using NRZ.Models.EPayment;
using NRZ.Models.Role;
using NRZ.Models.SeizedPropertyAvailability;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRZ.Services
{
    public class SeizedPropertyAvailabilityRequestService : BaseService, ISeizedPropertyAvailabilityRequestService
    {
        private readonly IEPaymentService _ePaymentService;

        public SeizedPropertyAvailabilityRequestService(NRZContext context,
            IEPaymentService ePaymentService,
            IStringLocalizer<SharedResources> localizer) : base(context, localizer)
        {
            _ePaymentService = ePaymentService;
        }

        public async Task<IEnumerable<SeizedPropertyAvailabilityListItemModel>> GetAll()
        {
            var requests = await _context.SeizedPropertyAvailabilityRequest
                .Include(x => x.RequestorPerson)
                .Include(x => x.RequesterCompany)
                .Include(x => x.CheckedPerson)
                .Select(x => new SeizedPropertyAvailabilityListItemModel
                {
                    Id = x.Id,
                    Date = x.Date,
                    InTheQualityOf = x.InTheQualityOfPersonTypeCodeNavigation.Name,
                    //RequestorPersonFullName = $"{x.RequestorPerson.FirstName} {x.RequestorPerson.MiddleName} {x.RequestorPerson.LastName}",
                    RequestorPersonFullName = $"{x.RequestorUser.PersonUser.FirstOrDefault().FirstName} {x.RequestorUser.PersonUser.FirstOrDefault().MiddleName} {x.RequestorUser.PersonUser.FirstOrDefault().LastName}",
                    RequestorCompanyName = x.RequesterCompany.Name,
                    RequestorCompanyEik = x.RequesterCompany.Eik,
                    CheckedPersonFullName = $"{x.CheckedPerson.FirstName} {x.CheckedPerson.MiddleName} {x.CheckedPerson.LastName}"
                }).ToListAsync();

            return requests;
        }

        public async Task<SeizedAvailabilityRequestReportModel> GetReport(int distraintId)
        {
            var distraint = await _context.Distraint
                .Include(x => x.DebtorCompany)
                .Include(x => x.DebtorPerson)
                .Include(x => x.PropertyTypeCodeNavigation)
                .Include(x => x.EnforcedByNavigation)
                .Include(x => x.InFavourOfCompany)
                .Include(x => x.InFavourOfPerson)
                .Include(x => x.PropertyIdVehicleNavigation)
                .Include(x => x.PropertyIdAircraftNavigation)
                .Include(x => x.PropertyIdAgriForMachineryNavigation)
                .Include(x => x.PropertyIdOtherPropertyNavigation)
                .Where(x => x.Id == distraintId)
                .FirstOrDefaultAsync();

            var report = new SeizedAvailabilityRequestReportModel
            {
                EnforcedAt = distraint.EnforcedAt.ToString(),
                // TODO: EnforcedBy = 
                PropertyType = string.IsNullOrEmpty(distraint.PropertyTypeCode) ? string.Empty
                    : distraint.PropertyTypeCodeNavigation.Name,
                PropertyTypeCode = string.IsNullOrEmpty(distraint.PropertyTypeCode) ? default 
                    : distraint.PropertyTypeCodeNavigation.Code.ToLower(),
                EnforcementDate = distraint.EnforcementDate.ToString()
            };

            if (distraint.DebtorPersonId.HasValue && distraint.IsDebtorPerson)
            {
                report.DebtorName = $"{distraint.DebtorPerson.FirstName} {distraint.DebtorPerson.LastName}";
            }
            else if (distraint.DebtorCompanyId.HasValue && !distraint.IsDebtorPerson)
            {
                report.DebtorName = distraint.DebtorCompany.Name;
            }

            if (!string.IsNullOrEmpty(distraint.EnforcedBy))
            {
                report.EnforcedBy = $"{distraint.EnforcedByNavigation.UserName}";
            }

            if (distraint.InFavourOfCompanyId.HasValue && !distraint.IsInFavourOfPerson)
            {
                report.InFavorOf = distraint.InFavourOfCompany.Name;
            }
            if (distraint.InFavourOfPersonId.HasValue && distraint.IsInFavourOfPerson)
            {
                report.InFavorOf = $"{distraint.InFavourOfPerson.FirstName} {distraint.InFavourOfPerson.LastName}";
            }

            //switch (distraint.PropertyTypeCode)
            //{
            //    case NRZ.Shared.Enums.PropertyType.VEHICLE:
            //        report.PropertyName = distraint.PropertyIdVehicleNavigation.RegistrationNumber
            //        break;
            //    default:
            //        break;
            //}

            // TODO: Save data and datetime for this report
            return report;
        }

        public async Task<(bool hasToPay, PaymentRequestSendResultModel paymentRequestModel, IEnumerable<SeizedPropertyAvailabilityResultModel> searchModels)> Search(SeizedPropertyAvailabilityRequestModel model, string currentUserId)
        {
            SeizedPropertyAvailabilityRequest entry = await SaveSeizedPropertyAvailabilityRequestAsync(model, currentUserId);

            bool hasToPay = HasToPay(model, currentUserId);
            if (!HasToPay(model, currentUserId))
            {
                // Не се изисква плащане. Връщаме резултат
                List<SeizedPropertyAvailabilityResultModel> searchModels = await CreateSearchResultAsync(model);
                return (hasToPay, paymentRequestModel: null, searchModels);
            }

            using (var tran = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    PaymentRequestSendResultModel paymentRequestModel = await CreateTaxMeRequest(entry.Id);
                    if (paymentRequestModel == null)
                    {
                        // Неуспешен запис на заявка за ел.плащане в системата за ел.плащане
                        await tran.RollbackAsync();
                        return (hasToPay, paymentRequestModel, null);
                    }

                    var eservicePaymentRequest = await _context.EservicePaymentRequest.Where(x => x.SeizedPropertyReportRequestId == entry.Id).FirstOrDefaultAsync();
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
                    return (hasToPay, paymentRequestModel, null);
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                    throw;
                }
            }

        }

        public async Task<AircraftDetailsModel> GetAircrafInfo(long distraintId)
        {
            var aircraftInfo = await _context.Distraint
                .Include(x => x.PropertyIdAircraftNavigation)
                .Where(x => x.Id == distraintId && x.PropertyIdAircraft.HasValue)
                .Select(x => new AircraftDetailsModel
                {
                    AircraftCategoryCode = x.PropertyIdAircraftNavigation.AirCategoryCode,
                    AircraftCategoryName = x.PropertyIdAircraftNavigation.AirCategoryName,
                    ICAO = x.PropertyIdAircraftNavigation.Icao,
                    ModelName = x.PropertyIdAircraftNavigation.ModelName,
                    ModelNameEn = x.PropertyIdAircraftNavigation.ModelNameEn,
                    MsnSerialNumber = x.PropertyIdAircraftNavigation.MsnserialNumber,
                    ProducerCountryCode = x.PropertyIdAircraftNavigation.ProducerCountryCode,
                    ProducerCountryName = x.PropertyIdAircraftNavigation.ProducerCountryName,
                    ProducerName = x.PropertyIdAircraftNavigation.ProducerName,
                    ProducerNameEn = x.PropertyIdAircraftNavigation.ProducerNameEn
                }).FirstOrDefaultAsync();

            return aircraftInfo;
        }

        public async Task<VehicleDetailsModel> GetVehicleInfo(int distraintId)
        {
            var vehicleInfo = await _context.Distraint
                .Include(x => x.PropertyIdVehicleNavigation)
                .Where(x => x.Id == distraintId && x.PropertyIdVehicle.HasValue)
                .Select(x => new VehicleDetailsModel
                {
                    Capacity = x.PropertyIdVehicleNavigation.Capacity,
                    Category = x.PropertyIdVehicleNavigation.Category,
                    Color = x.PropertyIdVehicleNavigation.Color,
                    EnvironmentalCategory = x.PropertyIdVehicleNavigation.EnvironmentalCategory,
                    FirstRegistrationDate = x.PropertyIdVehicleNavigation.FirstRegistrationDate,
                    Fuel = x.PropertyIdVehicleNavigation.Fuel,
                    MaxPower = x.PropertyIdVehicleNavigation.MaxPower,
                    Model = x.PropertyIdVehicleNavigation.Model,
                    RegistrationNumber = x.PropertyIdVehicleNavigation.RegistrationNumber,
                    VehicleDocumentDate = x.PropertyIdVehicleNavigation.VehicleDocumentDate,
                    VehicleDocumentNumber = x.PropertyIdVehicleNavigation.VehicleDocumentNumber,
                    VehicleType = x.PropertyIdVehicleNavigation.VehicleType,
                    Vin = x.PropertyIdVehicleNavigation.Vin
                }).FirstOrDefaultAsync();

            return vehicleInfo;
        }

        public async Task<VesselDetailsModel> GetVesselInfo(int distraintId)
        {
            var vesselInfo = await _context.Distraint
                .Include(x => x.PropertyIdVesselNavigation)
                .Where(x => x.Id == distraintId && x.PropertyIdVessel.HasValue)
                .Select(x => new VesselDetailsModel
                {
                    BodyNumber = x.PropertyIdVesselNavigation.BodyNumber,
                    Bt = x.PropertyIdVesselNavigation.Bt,
                    Nt = x.PropertyIdVesselNavigation.Nt,
                    DeadWight = x.PropertyIdVesselNavigation.Deadweight,
                    EnginesFuel = x.PropertyIdVesselNavigation.EnginesFuel,
                    LengthBetweenPerpendiculars = x.PropertyIdVesselNavigation.LengthBetweenPerpendiculars,
                    MaxLength = x.PropertyIdVesselNavigation.MaxLength,
                    MaxWidth = x.PropertyIdVesselNavigation.MaxWidth,
                    NumberOfEngines = x.PropertyIdVesselNavigation.NumberOfEngines,
                    ShipboardHeight = x.PropertyIdVesselNavigation.ShipboardHeight,
                    SumEnginePower = x.PropertyIdVesselNavigation.SumEnginePower,
                    Waterplane = x.PropertyIdVesselNavigation.Waterplane
                }).FirstOrDefaultAsync();

            return vesselInfo;
        }

        public async Task<RealEstateDetailsModel> GetRealEstateInfo(int distraintId)
        {
            // TODO: къде е navigation property-то?
            var realEstateInfo = await _context.Distraint
                //.Include(x => x.PropertyIdRealEstate)
                .Where(x => x.Id == distraintId && x.PropertyIdRealEstate.HasValue)
                .Select(x => new RealEstateDetailsModel
                {
                    //Area = x.proper
                }).FirstOrDefaultAsync();

            return realEstateInfo;
        }

        public async Task<AgriformachineryIDetailsModel> GetAgriformachineryInfo(int distraintId)
        {
            var model = await _context.Distraint
                .Include(x => x.PropertyIdAgriForMachineryNavigation)
                .Where(x => x.Id == distraintId && x.PropertyIdAgriForMachinery.HasValue)
                .Select(x => new AgriformachineryIDetailsModel
                {
                    FrameNumber = x.PropertyIdAgriForMachineryNavigation.FrameNumber,
                    Owner = x.PropertyIdAgriForMachineryNavigation.OwnerId.HasValue ?
                        $"{x.PropertyIdAgriForMachineryNavigation.Owner.FirstName} {x.PropertyIdAgriForMachineryNavigation.Owner.LastName}"
                        : string.Empty,
                    RegistrationNumber = x.PropertyIdAgriForMachineryNavigation.RegistrationNumber,
                    Type = x.PropertyIdAgriForMachineryNavigation.Type
                }).FirstOrDefaultAsync();

            return model;
        }

        public async Task<OtherDetailsModel> GetOtherInfo(int distraintId)
        {
            var other = await _context.Distraint
                .Include(x => x.PropertyIdOtherPropertyNavigation)
                .Where(x => x.Id == distraintId && x.PropertyIdOtherProperty.HasValue)
                .Select(x => new OtherDetailsModel
                {
                    Description = x.PropertyIdOtherPropertyNavigation.Description,
                    Identifier = x.PropertyIdOtherPropertyNavigation.Identifier,
                    Type = x.PropertyIdOtherPropertyNavigation.TypeNavigation.Name
                }).FirstOrDefaultAsync();

            return other;
        }

        public async Task<List<SeizedPropertyAvailabilityResultModel>> GetPaidReport(string paidGuid)
        {
            var paymentRequest = await _context.PaymentRequest
                .Include(x => x.EservicePaymentRequest).ThenInclude(x => x.SeizedPropertyReportRequest).ThenInclude(x => x.CheckedCompany)
                .Include(x => x.EservicePaymentRequest).ThenInclude(x => x.SeizedPropertyReportRequest).ThenInclude(x => x.CheckedPerson)
                .Where(x => x.PaymentReferenceNumber.Equals(paidGuid))
                .FirstOrDefaultAsync();
            if (paymentRequest == null)
            {
                throw new Exception($"There is no payment for guid: {paidGuid}");
                // TODO: Log
            }

            var eServicePayment = paymentRequest.EservicePaymentRequest.FirstOrDefault();
            if (eServicePayment == null)
            {
                throw new Exception($"There is no EservicePaymentRequest for guid: {paidGuid}");
                // TODO: Log
            }

            if (eServicePayment.SeizedPropertyReportRequestId.HasValue)
            {
                var request = eServicePayment.SeizedPropertyReportRequest;
                var model = new SeizedPropertyAvailabilityRequestModel
                {
                    IsCheckedPerson = request.IsCheckedPerson
                };
                if (request.IsCheckedPerson)
                {
                    model.CheckedPerson = new Models.Person.PersonModel
                    {
                        FirstName = request.CheckedPerson.FirstName,
                        MiddleName = request.CheckedPerson.MiddleName,
                        LastName = request.CheckedPerson.LastName,
                        Email = request.CheckedPerson.Email,
                        IdentificationType = request.CheckedPerson.IdentificationNumberType,
                        IdentificationNumber = request.CheckedPerson.IdentificationNumber
                    };
                }
                else
                {
                    model.CheckedCompany = new Models.Company.CompanyModel
                    {
                        Name = request.CheckedCompany.Name,
                        EIK = request.CheckedCompany.Eik,
                        CompanyCaseNumber = request.CheckedCompany.CompanyCaseNumber
                    };
                }

                var result = await CreateSearchResultAsync(model);
                return result;
            }
            else
            {
                throw new Exception($"There is no SeizedPropertyReportRequest for guid: {paidGuid}");
            }
        }

        /// <summary>
        /// Създаваме запис за заявлението за справка за наличие на запорирано имущество на лица в таблица SeizedPropertyAvailabilityRequest.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        private async Task<SeizedPropertyAvailabilityRequest> SaveSeizedPropertyAvailabilityRequestAsync(SeizedPropertyAvailabilityRequestModel model, string currentUserId)
        {
            var requestedPerson = new Person()
            {
                FirstName = model.Requester.FirstName,
                MiddleName = model.Requester.MiddleName,
                LastName = model.Requester.LastName,
                IdentificationNumberType = model.Requester.IdentificationType,
                IdentificationNumber = model.Requester.IdentificationNumber,
                Phone = model.Requester.Phone,
                Email = model.Requester.Email
            };

            if (model.Requester.Address != null)
            {
                requestedPerson.Address = new Address
                {
                    CityId = model.Requester.Address.CityId,
                    RegionId = model.Requester.Address.RegionId,
                    MunicipalityId = model.Requester.Address.MunicipalityId,
                    StreetAddress = model.Requester.Address.StreetAddress
                };
            }

            var checkedPerson = new Person();
            if (model.IsCheckedPerson)
            {
                checkedPerson.FirstName = model.CheckedPerson.FirstName;
                checkedPerson.MiddleName = model.CheckedPerson.MiddleName;
                checkedPerson.LastName = model.CheckedPerson.LastName;
                checkedPerson.IdentificationNumberType = model.CheckedPerson.IdentificationType;
                checkedPerson.IdentificationNumber = model.CheckedPerson.IdentificationNumber;
                checkedPerson.Phone = model.CheckedPerson.Phone;
                checkedPerson.Email = model.CheckedPerson.Email;

                if (model.CheckedPerson.Address != null)
                {
                    checkedPerson.Address = new Address
                    {
                        CityId = model.CheckedPerson.Address.CityId,
                        RegionId = model.CheckedPerson.Address.RegionId,
                        MunicipalityId = model.CheckedPerson.Address.MunicipalityId,
                        StreetAddress = model.CheckedPerson.Address.StreetAddress
                    };
                }
            }

            var checkedCompany = new Company();
            if (!model.IsCheckedPerson)
            {
                checkedCompany.Name = model.CheckedCompany.Name;
                checkedCompany.Eik = model.CheckedCompany.EIK;
                checkedCompany.CompanyCaseNumber = model.CheckedCompany.CompanyCaseNumber;

                if (model.CheckedCompany.Address != null)
                {
                    checkedCompany.Address = new Address
                    {
                        CityId = model.CheckedCompany.Address.CityId,
                        RegionId = model.CheckedCompany.Address.RegionId,
                        MunicipalityId = model.CheckedCompany.Address.MunicipalityId,
                        StreetAddress = model.CheckedCompany.Address.StreetAddress
                    };
                }
            }

            SeizedPropertyAvailabilityRequest toAdd = new SeizedPropertyAvailabilityRequest
            {
                RequestorPerson = string.IsNullOrEmpty(currentUserId) ? requestedPerson : null,
                RequestorUserId = currentUserId,
                InTheQualityOfPersonTypeCode = model.InTheQualityOfPersonTypeCode,
                RequesterCompanyRepresentative = model.RequesterCompanyRepresentative,
                RequesterCompanyEik = model.RequesterCompanyEik,
                RequesterCompanyCaseNumber = model.RequesterCompanyCaseNumber,
                Date = DateTime.UtcNow, // TODO: Да се взема от... някакъв сървис...
                IsCheckedPerson = model.IsCheckedPerson,
                CheckedPerson = model.IsCheckedPerson ? checkedPerson : null,
                CheckedCompany = model.IsCheckedPerson ? null : checkedCompany
            };

            _context.SeizedPropertyAvailabilityRequest.Add(toAdd);
            await _context.SaveChangesAsync();

            return toAdd;
        }

        private Task<List<SeizedPropertyAvailabilityResultModel>> CreateSearchResultAsync(SeizedPropertyAvailabilityRequestModel model)
        {
            var result = _context.Distraint
                    .Include(x => x.DebtorPerson)
                    .Include(x => x.DebtorCompany)
                    .AsQueryable();
            if (model.IsCheckedPerson)
            {
                return result.Where(x => x.DebtorPerson.Identifier == model.CheckedPerson.IdentificationNumber.Trim()
                        && (x.DebtorPerson.FirstName.ToLower().Contains(model.CheckedPerson.FirstName.Trim().ToLower())
                        || (!string.IsNullOrEmpty(model.CheckedPerson.MiddleName) && x.DebtorPerson.MiddleName.Contains(model.CheckedPerson.MiddleName.Trim().ToLower()))
                        || x.DebtorPerson.LastName.Contains(model.CheckedPerson.LastName.Trim().ToLower())))
                    .Select(x => new SeizedPropertyAvailabilityResultModel
                    {
                        DistraintId = x.Id,
                        Name = $"{x.DebtorPerson.FirstName} {x.DebtorPerson.MiddleName} {x.DebtorPerson.LastName}",
                        PropertyTypeName = x.PropertyTypeCodeNavigation.Name,
                        PropertyTypeNameEn = x.PropertyTypeCodeNavigation.NameEn,
                        StatusName = x.StatusCodeNavigation.Name,
                        StatusNameEn = x.StatusCodeNavigation.NameEn
                    })
                    .ToListAsync();
            }
            else
            {
                return result.Where(x => x.DebtorCompany.Uic == model.CheckedCompany.EIK
                        && x.DebtorCompany.Name.ToLower().Contains(model.CheckedCompany.Name.Trim().ToLower()))
                    .Select(x => new SeizedPropertyAvailabilityResultModel
                    {
                        DistraintId = x.Id,
                        Name = $"{x.DebtorCompany.Name}",
                        PropertyTypeName = x.PropertyTypeCodeNavigation.Name,
                        PropertyTypeNameEn = x.PropertyTypeCodeNavigation.NameEn,
                        StatusName = x.StatusCodeNavigation.Name,
                        StatusNameEn = x.StatusCodeNavigation.NameEn
                    })
                    .ToListAsync();
            }
        }

        private bool HasToPay(SeizedPropertyAvailabilityRequestModel model, string currentUserId)
        {
            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return true;
            }

            // Todo: Да се прецезира кога трябва да се плаща
            return false;
        }

        private async Task<PaymentRequestSendResultModel> CreateTaxMeRequest(int seizedPropertyAvailabilityRequestId)
        {
            EServicePaymentRequestCreateModel result = await _ePaymentService.GeneratePaymentRequestAsync(Shared.Enums.EServiceType.SEIZEDPROPERTYBYOWNERREPORT, seizedPropertyAvailabilityRequestId);

            PaymentRequest request = await _context.PaymentRequest.Where(x => x.AisPaymentId == result.PaymentRequestId).SingleOrDefaultAsync();
            EPaymentRequestModel model = request.ToEpaymentModel();
            PaymentRequestSendResultModel sendResultModel = await _ePaymentService.SendEpaymentRequestAsync(model ?? throw new ArgumentNullException(nameof(result)));

            return sendResultModel;
        }
    }
}
