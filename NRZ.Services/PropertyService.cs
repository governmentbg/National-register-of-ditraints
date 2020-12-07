using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Data.Extensions;
using NRZ.Models.Property;
using NRZ.RegiX.Client.ResponseModels;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NRZ.Models.Settings;
using Microsoft.Extensions.Options;
using NRZ.Models;

namespace NRZ.Services
{
    public class PropertyService : BaseService, IPropertyService
    {
        private readonly IIntegrationService _integrationService;
        private readonly RegiXCertificateSettings regixCertificateSettings;

        public PropertyService(NRZContext context,
            IIntegrationService integrationService,
            IOptions<RegiXCertificateSettings> regixSettings,
            IStringLocalizer<SharedResources> localizer = null)
            : base(context, localizer)
        {
            _integrationService = integrationService;
            regixCertificateSettings = regixSettings.Value;
        }


        public async Task<OtherPropertyModel> AddOtherPropertyAsync(OtherPropertyModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("OtherPropertyModel is null");
            }
            OtherProperty entry = new OtherProperty
            {
                Identifier = model.Identifier,
                Description = model.Description,
                Type = model.Type,
                IsManuallyAdded = model.IsManuallyAdded
                // Todo: implement ICreatable, change related props type to non-nullable
            };

            await _context.OtherProperty.AddAsync(entry);
            await _context.SaveChangesAsync();

            return entry.ToViewModel();
        }

        public async Task<Data.Vehicle> AddOrUpdateVehicleAsync(VehicleViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("VehicleViewModel is null");
            }

            Data.Vehicle entity = null;

            var existing = await _context.Vehicle
                .Include(x => x.VehicleExtension)
                .Include(x => x.VehicleOwner)
                .Where(x => x.RegistrationNumber.ToLower() == model.RegistrationNumber.ToLower() &&
                    x.VehicleExtension != null &&
                    x.VehicleExtension.Deactivated == false)
                .ToListAsync();

            if (!existing.Any())
            {
                entity = model.ToEntity();
                entity.Id = 0;
                entity.VehicleExtension.VehicleId = entity.Id;
                await _context.Vehicle.AddAsync(entity);
            }
            else
            {
                foreach (Data.Vehicle item in existing)
                {
                    item.VehicleExtension.Deactivated = true;
                    _context.VehicleOwner.RemoveRange(item.VehicleOwner);
                    _context.VehicleUser.RemoveRange(item.VehicleUser);
                }

                existing[0].UpdateEntity(model);
                existing[0].VehicleExtension.Deactivated = false;
                existing[0].VehicleExtension.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                existing[0].VehicleExtension.RequestId = model.ExtensionRequestId;

                _context.Vehicle.Update(existing[0]);
                entity = existing[0];
            }


            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Data.Aircraft> AddOrUpdateAircraftAsync(AircraftViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("AircraftViewModel is null");
            }

            Data.Aircraft entity = null;

            var existing = await _context.Aircraft
                .Include(x => x.AircraftDebt)
                .Include(x => x.AircraftRegistration)
                .Include(x => x.AircraftRegistration).ThenInclude(x => x.AircraftRegistrationOwnerPerson)
                .Include(x => x.AircraftRegistration).ThenInclude(x => x.AircraftRegistrationOwnerEntity)
                .Include(x => x.AircraftRegistration).ThenInclude(x => x.AircraftRegistrationOperatorPerson)
                .Include(x => x.AircraftRegistration).ThenInclude(x => x.AircraftRegistrationOperatorEntity)
                .Where(x => x.MsnserialNumber.ToLower() == model.MsnserialNumber.ToLower() &&
                    x.AircraftExtension != null &&
                    x.AircraftExtension.Deactivated == false)
                .ToListAsync();

            if (!existing.Any())
            {
                entity = model.ToEntity();
                entity.Id = 0;
                entity.AircraftExtension.AircraftId = entity.Id;
                await _context.Aircraft.AddAsync(entity);
            }
            else
            {
                foreach (Data.Aircraft item in existing)
                {
                    item.AircraftExtension.Deactivated = true;
                    _context.AircraftDebt.RemoveRange(item.AircraftDebt);

                    foreach (Data.AircraftRegistration reg in item.AircraftRegistration)
                    {
                        _context.AircraftRegistrationOperatorPerson.RemoveRange(reg.AircraftRegistrationOperatorPerson);
                        _context.AircraftRegistrationOperatorEntity.RemoveRange(reg.AircraftRegistrationOperatorEntity);
                        _context.AircraftRegistrationOwnerPerson.RemoveRange(reg.AircraftRegistrationOwnerPerson);
                        _context.AircraftRegistrationOwnerEntity.RemoveRange(reg.AircraftRegistrationOwnerEntity);
                    }

                    _context.AircraftRegistration.RemoveRange(item.AircraftRegistration);
                }

                existing[0].UpdateEntity(model);
                existing[0].AircraftExtension.Deactivated = false;
                existing[0].AircraftExtension.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                existing[0].AircraftExtension.RequestId = model.ExtensionRequestId;

                _context.Aircraft.Update(existing[0]);
                entity = existing[0];
            }

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Data.Vessel> AddOrUpdateVesselAsync(VesselViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("VesselViewModel is null");
            }

            Data.Vessel entity = null;
            var existing = new List<Data.Vessel>();

            // няма търсене по уникален номер на кораб в Regix
            //string identifier = model.RegistrationData != null ? model.RegistrationData.RegistrationNumber : "";
            //var existing = await _context.Vessel
            //    .Include(x => x.VesselEngine)
            //    .Include(x => x.VesselOwner)
            //    .Include(x => x.VesselRegistrationData)
            //    .Include(x => x.VesselRegistrationData).ThenInclude(x => x.StatusNavigation)
            //    .Where(x => x.VesselRegistrationData.Any() && 
            //        x.VesselRegistrationData.Where(a => a.RegistrationNumber.ToLower() == identifier.ToLower()).Any() &&
            //        x.VesselExtension != null &&
            //        x.VesselExtension.Deactivated == false)
            //    .ToListAsync();

            if (!existing.Any())
            {
                entity = model.ToEntity();
                entity.Id = 0;
                entity.VesselExtension.VesselId = entity.Id;
                await _context.Vessel.AddAsync(entity);
            }
            else
            {
                foreach (Data.Vessel item in existing)
                {
                    item.VesselExtension.Deactivated = true;
                    _context.VesselEngine.RemoveRange(item.VesselEngine);
                    _context.VesselOwner.RemoveRange(item.VesselOwner);
                    _context.VesselRegistrationData.RemoveRange(item.VesselRegistrationData);
                }

                existing[0].UpdateEntity(model);
                existing[0].VesselExtension.Deactivated = false;
                existing[0].VesselExtension.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                existing[0].VesselExtension.RequestId = model.ExtensionRequestId;

                _context.Vessel.Update(existing[0]);
                entity = existing[0];
            }

            await _context.SaveChangesAsync();

            return entity;
        }


        public async Task<List<OtherPropertyModel>> SearchOtherPropertyInDBAsync(PropertySearchRequestModel model)
        {
            var entities = await _context.OtherProperty
                        .Include(x => x.Distraint)
                        .Where(x => x.Distraint.Any() && x.Identifier.ToLower().Contains(model.Identifier.Trim().ToLower()))
                        .Select(x => x.ToViewModel())
                        .ToListAsync();

            return entities?.ToList();
        }

        public async Task<List<VehicleViewModel>> SearchVehicleInDBAsync(PropertySearchRequestModel model)
        {
            var entities = await _context.Vehicle
                        .Include(x => x.Distraint)
                        .Include(x => x.VehicleOwner)
                        .Include(x => x.VehicleUser)
                        .Where(x => x.Distraint.Any() && x.RegistrationNumber.ToLower() == model.Identifier.Trim().ToLower())
                        .Select(x => x.ToViewModel())
                        .ToListAsync();

            return entities?.ToList();
        }

        public async Task<List<AircraftViewModel>> SearchAircraftInDBAsync(PropertySearchRequestModel model)
        {
            var entities = await _context.Aircraft
                        .Include(x => x.Distraint)
                        .Include(x => x.AircraftDebt)
                        .Include(x => x.AircraftRegistration)
                        .Include(x => x.AircraftRegistration).ThenInclude(x => x.AircraftRegistrationOwnerPerson)
                        .Include(x => x.AircraftRegistration).ThenInclude(x => x.AircraftRegistrationOwnerEntity)
                        .Include(x => x.AircraftRegistration).ThenInclude(x => x.AircraftRegistrationOperatorPerson)
                        .Include(x => x.AircraftRegistration).ThenInclude(x => x.AircraftRegistrationOperatorEntity)
                        .Where(x => x.Distraint.Any() && x.MsnserialNumber.ToLower() == model.Identifier.Trim().ToLower())
                        .Select(x => x.ToViewModel())
                        .ToListAsync();

            return entities?.ToList();
        }

        public async Task<List<VesselViewModel>> SearchVesselInDBAsync(PropertySearchRequestModel model)
        {
            var entities = await _context.Vessel
                        .Include(x => x.VesselEngine)
                        .Include(x => x.VesselRegistrationData)
                        .Include(x => x.VesselRegistrationData).ThenInclude(x => x.StatusNavigation)
                        .Include(x => x.VesselOwner)
                        .Where(x => x.Distraint.Any() && 
                            x.VesselOwner.Any() &&
                            x.VesselOwner.Where(a => a.Eik.ToLower().Contains(model.Identifier.Trim().ToLower()) ||
                                a.Egn.ToLower().Contains(model.Identifier.Trim().ToLower())).Any())
                        .Select(x => x.ToViewModel())
                        .ToListAsync();

            return entities?.ToList();
        }

        public async Task<List<AgriculturalMachineryViewModel>> SearchAgriculturalMachineryInDBAsync(PropertySearchRequestModel model)
        {
            var entities = await _context.AgriculturalMachinery
                        .Include(x => x.Distraint)
                        .Include(x => x.Owner)
                        .Include(x => x.Company)
                        .Where(x => x.Distraint.Any() && x.RegistrationNumber.ToLower().Contains(model.Identifier.Trim().ToLower()))
                        .Select(x => x.ToViewModel())
                        .ToListAsync();

            return entities?.ToList();
        }



        public async Task<List<object>> SearchInDataAdministratorAsync(Shared.Enums.PropertyType propertyType, PropertySearchRequestModel searchModel)
        {
            PropertySearchResultModel result = await _integrationService.SearchPropertyAsync(propertyType, searchModel);
            List<object> entities = GetPropertyViewModelsFromResponse(propertyType, result, searchModel);

            return entities;
        }

        private List<object> GetPropertyViewModelsFromResponse(Shared.Enums.PropertyType propertyType, PropertySearchResultModel result, PropertySearchRequestModel searchModel)
        {
            if (result == null || result.ResponseObject == null)
                return null;

            List<object> entities = null;

            switch (propertyType)
            {
                case Shared.Enums.PropertyType.AIRCRAFT:
                    AircraftsResponse aircraftsResponse = result.ResponseObject as AircraftsResponse;
                    if (aircraftsResponse == null)
                        throw new Exception("Could not convert response to AircraftsResponse");
                    if (aircraftsResponse.Aircraft != null && aircraftsResponse.Aircraft.Length > 0)
                    {
                        //List<AircraftViewModel> aircrafts = SaveAircrafts(aircraftsResponse, result.RequestId, searchModel);
                        List<AircraftViewModel> aircrafts = new List<AircraftViewModel>();

                        for (int i = 0; i < aircraftsResponse.Aircraft.Length; i++)
                        {
                            AircraftViewModel airViewModel = aircraftsResponse.Aircraft[i].ToViewModel();
                            airViewModel.ExtensionRequestId = result.RequestId;
                            if (regixCertificateSettings.SaveEntityWithSearchedIdentifier && searchModel.IdentifierTypeCode == "MSN")
                                airViewModel.MsnserialNumber = searchModel.Identifier;
                            aircrafts.Add(airViewModel);
                        }

                        entities = (aircrafts).Cast<object>().ToList();
                    }
                    break;
                case Shared.Enums.PropertyType.VEHICLE:
                    if (regixCertificateSettings.UseVehicleV3)
                    {
                        GetMotorVehicleRegistrationInfoV3Response vehiclesResponse = result.ResponseObject as GetMotorVehicleRegistrationInfoV3Response;
                        if (vehiclesResponse == null)
                            throw new Exception("Could not convert response to GetMotorVehicleRegistrationInfoV3Response");
                        if (vehiclesResponse.Response != null && 
                            vehiclesResponse.Response.Results.Length > 0 && 
                            vehiclesResponse.Response.Results[0].VehicleData != null)
                        {
                            List<VehicleViewModel> vehicles = new List<VehicleViewModel>();

                            VehicleViewModel vehicleViewModel = vehiclesResponse.Response.Results[0].ToViewModel();
                            vehicleViewModel.ExtensionRequestId = result.RequestId;
                            if (regixCertificateSettings.SaveEntityWithSearchedIdentifier)
                                vehicleViewModel.RegistrationNumber = searchModel.Identifier;
                            vehicles.Add(vehicleViewModel);

                            entities = (vehicles).Cast<object>().ToList();
                        }
                    }
                    else
                    {
                        MotorVehicleRegistrationResponse vehiclesResponse = result.ResponseObject as MotorVehicleRegistrationResponse;
                        if (vehiclesResponse == null)
                            throw new Exception("Could not convert response to MotorVehicleRegistrationResponse");
                        if (vehiclesResponse.Vehicles != null && vehiclesResponse.Vehicles.Length > 0)
                        {
                            //List<Data.Vehicle> vehicles = await SaveVehicles(vehiclesResponse, result.RequestId, searchModel, useSearchIdentifier);
                            List<VehicleViewModel> vehicles = new List<VehicleViewModel>();

                            foreach (RegiX.Client.ResponseModels.Vehicle vehicle in vehiclesResponse.Vehicles)
                            {
                                VehicleViewModel vehicleViewModel = vehicle.ToViewModel();
                                vehicleViewModel.ExtensionRequestId = result.RequestId;
                                if (regixCertificateSettings.SaveEntityWithSearchedIdentifier)
                                    vehicleViewModel.RegistrationNumber = searchModel.Identifier;
                                vehicles.Add(vehicleViewModel);
                            }

                            entities = (vehicles).Cast<object>().ToList();
                        }
                    }
                    break;
                case Shared.Enums.PropertyType.VESSEL:
                    RegistrationInfoByOwnerResponse vesselResponse = result.ResponseObject as RegistrationInfoByOwnerResponse;
                    if (vesselResponse == null)
                        throw new Exception("Could not convert response to RegistrationInfoByOwnerResponse");
                    if (vesselResponse.VesselInfo != null)
                    {
                        VesselViewModel vesselViewModel = vesselResponse.VesselInfo.ToViewModel();
                        vesselViewModel.ExtensionRequestId = result.RequestId;
                        vesselViewModel = SetVesselStatusInModel(vesselViewModel);

                        entities = new List<object>();
                        entities.Add(vesselViewModel);
                    }
                    break;
                case Shared.Enums.PropertyType.AGRIFORMACHINERY:
                    AgriculturalMachineryCollectionModel machines = result.ResponseObject as AgriculturalMachineryCollectionModel;
                    entities = (machines.Machines).Cast<object>().ToList();
                    break;
                default:
                    break;
            }

            return entities;
        }

        private VesselViewModel SetVesselStatusInModel(VesselViewModel vessel)
        {
            VesselStatus status = _context.VesselStatus
                .Where(x => x.Code == vessel.RegistrationData.Status)
                .FirstOrDefault();

            if (status != null)
            {
                vessel.RegistrationData.StatusName = status.Name;
                vessel.RegistrationData.StatusNameEn = status.NameEn;
            }

            return vessel;
        }


        private async Task<List<BaseProperty>> SaveResponseEntities(Shared.Enums.PropertyType propertyType, PropertySearchResultModel result, PropertySearchRequestModel searchModel, bool useSearchIdentifier)
        {
            if (result == null || result.ResponseObject == null)
                return null;

            List<BaseProperty> entities = null;

            switch (propertyType)
            {
                case Shared.Enums.PropertyType.AIRCRAFT:
                    AircraftsResponse aircraftsResponse = result.ResponseObject as AircraftsResponse;
                    if (aircraftsResponse == null)
                        throw new Exception("Could not convert response to AircraftsResponse");
                    if (aircraftsResponse.Aircraft != null && aircraftsResponse.Aircraft.Length > 0)
                    {
                        List<Data.Aircraft> aircrafts = await SaveAircrafts(aircraftsResponse, result.RequestId, searchModel, useSearchIdentifier);
                        entities = (aircrafts).Cast<BaseProperty>().ToList();
                    }
                    break;
                case Shared.Enums.PropertyType.VEHICLE:
                    MotorVehicleRegistrationResponse vehiclesResponse = result.ResponseObject as MotorVehicleRegistrationResponse;
                    if (vehiclesResponse == null)
                        throw new Exception("Could not convert response to MotorVehicleRegistrationResponse");
                    if (vehiclesResponse.Vehicles != null && vehiclesResponse.Vehicles.Length > 0)
                    {
                        List<Data.Vehicle> vehicles = await SaveVehicles(vehiclesResponse, result.RequestId, searchModel, useSearchIdentifier);
                        entities = (vehicles).Cast<BaseProperty>().ToList();
                    }

                    //GetMotorVehicleRegistrationInfoV3ResponseTypeResponse vehiclesResponse = result.ResponseObject as GetMotorVehicleRegistrationInfoV3ResponseTypeResponse;
                    //if (vehiclesResponse == null)
                    //    throw new Exception("Could not convert response to GetMotorVehicleRegistrationInfoV3ResponseTypeResponse");
                    //if (vehiclesResponse.Results != null && vehiclesResponse.Results.Length > 0)
                    //{
                    //    List<Data.Vehicle> vehicles = await SaveVehiclesV3(vehiclesResponse, result.RequestId, searchModel, useSearchIdentifier);
                    //    entities = (vehicles).Cast<BaseProperty>().ToList();
                    //}

                    break;
                default:
                    break;
            }

            return entities;
        }

        private async Task<List<Data.Aircraft>> SaveAircrafts(AircraftsResponse aircraftsResponse, long? messageId, PropertySearchRequestModel searchModel, bool useSearchIdentifier)
        {
            List<Data.Aircraft> newAircrafts = new List<Data.Aircraft>();
            List<Data.Aircraft> existingAircrafts = new List<Data.Aircraft>();

            for (int i = 0; i < aircraftsResponse.Aircraft.Length; i++)
            {
                string msnToUse = aircraftsResponse.Aircraft[i].MSNSerialNumber;
                if (useSearchIdentifier)
                    msnToUse = searchModel.IdentifierTypeCode.ToUpper() == "MSN" ? searchModel.Identifier : searchModel.Identifier + "_" + i.ToString();

                var existing = await _context.Aircraft
                    .Include(x => x.AircraftExtension)
                    .Include(x => x.AircraftRegistration)
                    .Include(x => x.AircraftRegistration).ThenInclude(a => a.AircraftRegistrationOperatorEntity)
                    .Include(x => x.AircraftRegistration).ThenInclude(a => a.AircraftRegistrationOperatorPerson)
                    .Include(x => x.AircraftRegistration).ThenInclude(a => a.AircraftRegistrationOwnerEntity)
                    .Include(x => x.AircraftRegistration).ThenInclude(a => a.AircraftRegistrationOwnerPerson)
                    .Include(x => x.AircraftDebt)
                    .Where(x => x.MsnserialNumber == msnToUse &&
                        x.AircraftExtension != null &&
                        x.AircraftExtension.Deactivated == false)
                    .ToListAsync();

                if (!existing.Any())
                {
                    Data.Aircraft entity = aircraftsResponse.Aircraft[i].ToEntity();
                    entity.MsnserialNumber = msnToUse;
                    entity.AircraftExtension = new AircraftExtension
                    {
                        AircraftId = entity.Id,
                        UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        RequestId = messageId,
                        Deactivated = false
                    };

                    if (entity != null)
                        newAircrafts.Add(entity);
                }
                else
                {
                    foreach (Data.Aircraft item in existing)
                    {
                        item.AircraftExtension.Deactivated = true;
                        foreach (Data.AircraftRegistration reg in item.AircraftRegistration)
                        {
                            _context.AircraftRegistrationOperatorEntity.RemoveRange(reg.AircraftRegistrationOperatorEntity);
                            _context.AircraftRegistrationOperatorPerson.RemoveRange(reg.AircraftRegistrationOperatorPerson);

                            _context.AircraftRegistrationOwnerPerson.RemoveRange(reg.AircraftRegistrationOwnerPerson);
                            _context.AircraftRegistrationOwnerEntity.RemoveRange(reg.AircraftRegistrationOwnerEntity);
                        }
                        _context.AircraftRegistration.RemoveRange(item.AircraftRegistration);
                        _context.AircraftDebt.RemoveRange(item.AircraftDebt);
                    }

                    existing[0].UpdateEntity(aircraftsResponse.Aircraft[i]);
                    existing[0].MsnserialNumber = msnToUse;
                    existing[0].AircraftExtension.Deactivated = false;
                    existing[0].AircraftExtension.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    existing[0].AircraftExtension.RequestId = messageId;

                    existingAircrafts.AddRange(existing);
                }
            }

            if (newAircrafts.Count > 0)
            {
                _context.Aircraft.AddRange(newAircrafts);
            }

            if (existingAircrafts.Count > 0)
            {
                _context.Aircraft.UpdateRange(existingAircrafts);
            }

            await _context.SaveChangesAsync();

            return newAircrafts.Concat(existingAircrafts).ToList();
        }



        private async Task<List<Data.Vehicle>> SaveVehicles(MotorVehicleRegistrationResponse vehiclesResponse, long? messageId, PropertySearchRequestModel searchModel, bool useSearchIdentifier)
        {
            List<Data.Vehicle> newVehicles = new List<Data.Vehicle>();
            List<Data.Vehicle> existingVehicles = new List<Data.Vehicle>();

            foreach (RegiX.Client.ResponseModels.Vehicle vehicle in vehiclesResponse.Vehicles)
            {
                string regNumberToUse = vehicle.VehicleRegistrationNumber;
                if (useSearchIdentifier)
                    regNumberToUse = searchModel.Identifier;

                var existing = await _context.Vehicle
                    .Include(x => x.VehicleExtension)
                    .Include(x => x.VehicleOwner)
                    .Where(x => x.RegistrationNumber == regNumberToUse &&
                        x.VehicleExtension != null &&
                        x.VehicleExtension.Deactivated == false)
                    .ToListAsync();

                if (!existing.Any())
                {
                    Data.Vehicle entity = vehicle.ToEntity();
                    entity.RegistrationNumber = regNumberToUse;
                    entity.VehicleExtension = new VehicleExtension
                    {
                        VehicleId = entity.Id,
                        UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        RequestId = messageId,
                        Deactivated = false
                    };

                    if (entity != null)
                        newVehicles.Add(entity);
                }
                else
                {
                    foreach (Data.Vehicle item in existing)
                    {
                        item.VehicleExtension.Deactivated = true;
                        _context.VehicleOwner.RemoveRange(item.VehicleOwner);
                    }

                    existing[0].UpdateEntity(vehicle);
                    existing[0].RegistrationNumber = regNumberToUse;
                    existing[0].VehicleExtension.Deactivated = false;
                    existing[0].VehicleExtension.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    existing[0].VehicleExtension.RequestId = messageId;

                    existingVehicles.AddRange(existing);
                }
            }

            if (newVehicles.Count > 0)
            {
                _context.Vehicle.AddRange(newVehicles);
            }

            if (existingVehicles.Count > 0)
            {
                _context.Vehicle.UpdateRange(existingVehicles);
            }

            await _context.SaveChangesAsync();

            return newVehicles.Concat(existingVehicles).ToList();
        }

        public async Task<PropertyModel> AddAsync(PropertyModel model, string userId)
        {
            if (model == null) throw new ArgumentNullException(nameof(PropertyModel));

            Property entry = new Property
            {
                Type = model.Type,
                Floor = model.Floor,
                Area = model.Area ?? 0m,
                Address = model.Address != null
                    ? new Address { 
                        RegionId = model.Address.RegionId,
                        MunicipalityId = model.Address.MunicipalityId,
                        CityId = model.Address.CityId,
                        StreetAddress = model.Address.StreetAddress ?? ""
                    }
                    : null,
                PropertyConstructionTypeId = model.PropertyConstructionTypeId,
                Identifier = model.Identifier,
                IdentifierType = model.IdentifierType,
                Description = model.Description,
                IsManuallyAdded = model.IsManuallyAdded
            };

            SetCreateStamp(entry, userId);
            _context.Property.Add(entry);

            await _context.SaveChangesAsync();

            return entry.ToViewModel();
        }

        public Task<PropertyModel> UpdateAsunc(PropertyModel model, string userId)
        {
            throw new NotImplementedException();
        }

        //private async Task<List<Data.Vehicle>> SaveVehiclesV3(GetMotorVehicleRegistrationInfoV3ResponseTypeResponse vehiclesResponse, long? messageId, PropertySearchRequestModel searchModel, bool useSearchIdentifier)
        //{
        //    List<Data.Vehicle> newVehicles = new List<Data.Vehicle>();
        //    List<Data.Vehicle> existingVehicles = new List<Data.Vehicle>();

        //    foreach (GetMotorVehicleRegistrationInfoV3ResponseTypeResponseResult vehicle in vehiclesResponse.Results)
        //    {
        //        string regNumberToUse = vehicle.VehicleData.RegistrationNumber;
        //        if (useSearchIdentifier)
        //            regNumberToUse = searchModel.Identifier;

        //        var existing = await _context.Vehicle
        //            .Include(x => x.VehicleExtension)
        //            .Include(x => x.VehicleOwner)
        //            .Include(x => x.VehicleUser)
        //            .Where(x => x.RegistrationNumber == regNumberToUse &&
        //                x.VehicleExtension != null &&
        //                x.VehicleExtension.Deactivated == false)
        //            .ToListAsync();

        //        if (!existing.Any())
        //        {
        //            Data.Vehicle entity = vehicle.ToEntity();
        //            entity.RegistrationNumber = regNumberToUse;
        //            entity.VehicleExtension = new VehicleExtension
        //            {
        //                VehicleId = entity.Id,
        //                UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
        //                RequestId = messageId,
        //                Deactivated = false
        //            };

        //            if (entity != null)
        //                newVehicles.Add(entity);
        //        }
        //        else
        //        {
        //            foreach (Data.Vehicle item in existing)
        //            {
        //                item.VehicleExtension.Deactivated = true;
        //                _context.VehicleOwner.RemoveRange(item.VehicleOwner);
        //                _context.VehicleUser.RemoveRange(item.VehicleUser);
        //            }

        //            existing[0].UpdateEntity(vehicle);
        //            existing[0].RegistrationNumber = regNumberToUse;
        //            existing[0].VehicleExtension.Deactivated = false;
        //            existing[0].VehicleExtension.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        //            existing[0].VehicleExtension.RequestId = messageId;

        //            existingVehicles.AddRange(existing);
        //        }
        //    }

        //    if (newVehicles.Count > 0)
        //    {
        //        _context.Vehicle.AddRange(newVehicles);
        //    }

        //    if (existingVehicles.Count > 0)
        //    {
        //        _context.Vehicle.UpdateRange(existingVehicles);
        //    }

        //    await _context.SaveChangesAsync();

        //    return newVehicles.Concat(existingVehicles).ToList();
        //}




    }
}
