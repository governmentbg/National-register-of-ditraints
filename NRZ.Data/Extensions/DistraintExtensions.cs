using NRZ.Models.Distraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRZ.Data.Extensions
{
    public static class DistraintExtensions
    {
        public static Distraint ToEntity(this DistraintCreateModel model)
        {
            if (model == null)
            {
                return null;
            }

            DateTime now = DateTime.SpecifyKind(DateTime.Now.ToUniversalTime(), DateTimeKind.Utc);
            Distraint entity = new Distraint
            {
                PropertyIdVehicle = model.PropertyIdVehicle,
                PropertyIdAircraft = model.PropertyIdAircraft,
                PropertyIdVessel = model.PropertyIdVessel,
                PropertyIdAgriForMachinery = model.PropertyIdAgriForMachinery,
                PropertyIdRealEstate = model.PropertyIdRealEstate,
                PropertyIdOtherProperty = model.PropertyIdOtherProperty,
                PropertyTypeCode = model.PropertyTypeCode,
                StatusCode = model.StatusCode,
                InFavourOf = model.InFavourOf,
                SuitNumber = model.SuitNumber,
                Debtor = model.Debtor,
                CreatedOn = now,
                CreatedBy = model.CreatedBy,
                Location = model.Location,
                EnforcementDate = model.EnforcementDate.HasValue == true ? DateTime.SpecifyKind(model.EnforcementDate.Value, DateTimeKind.Utc) : default(DateTime?),
                EnforcedBy = model.EnforcedBy,
                EnforcedAt = now,
                InFavourOfPersonId = model.InFavourOfPerson?.Id,
                DebtorPersonId = model.DebtorPerson?.Id,
                InFavourOfCompanyId = model.InFavourOfCompany?.Id,
                DebtorCompanyId = model.DebtorCompany?.Id,
                IsInFavourOfPerson = model.IsInFavourOfPerson,
                IsDebtorPerson = model.IsDebtorPerson
            };

            return entity;
        }

        public static DistraintViewModel ToViewModel(this Distraint entity)
        {
            if (entity == null)
            {
                return null;
            }

            Tuple<Nullable<long>, string> data = GetDistraintPropertyData(entity);

            DistraintViewModel model = new DistraintViewModel
            {
                Id = entity.Id,
                PropertyId = data.Item1,
                PropertyRegNumber = data.Item2,
                PropertyTypeCode = entity.PropertyTypeCode,
                PropertyTypeName = entity.PropertyTypeCodeNavigation?.Name,
                PropertyTypeNameEn = entity.PropertyTypeCodeNavigation?.NameEn,
                StatusCode = entity.StatusCodeNavigation?.Code,
                StatusName = entity.StatusCodeNavigation?.Name,
                StatusNameEn = entity.StatusCodeNavigation?.NameEn,
                //InFavourOf = entity.InFavourOf,
                SuitNumber = entity.SuitNumber,
                //Debtor = entity.Debtor,
                Deactivated = entity.Deactivated,
                CreatedOn = DateTime.SpecifyKind(entity.CreatedOn, DateTimeKind.Utc),
                CreatedByUserType = entity.CreatedByNavigation != null ? String.Join(", ", entity.CreatedByNavigation?.AspNetUserRoles.Select(x => x.Role.Name).ToList()) : "",
                CreatedByUserName = entity.CreatedByNavigation?.UserName,
                Location = entity.Location,
                EnforcementDate = entity.EnforcementDate.HasValue == true ? DateTime.SpecifyKind(entity.EnforcementDate.Value, DateTimeKind.Utc) : default(DateTime?),
                EnforcedAt = entity.EnforcedAt.HasValue == true ? DateTime.SpecifyKind(entity.EnforcedAt.Value, DateTimeKind.Utc) : default(DateTime?),
                RevocationDate = entity.RevocationDate.HasValue == true ? DateTime.SpecifyKind(entity.RevocationDate.Value, DateTimeKind.Utc) : default(DateTime?),
                RevokedAt = entity.RevokedAt.HasValue == true ? DateTime.SpecifyKind(entity.RevokedAt.Value, DateTimeKind.Utc) : default(DateTime?),
                ExemptionDate = entity.ExemptionDate.HasValue == true ? DateTime.SpecifyKind(entity.ExemptionDate.Value, DateTimeKind.Utc) : default(DateTime?),
                ExemptedAt = entity.ExemptedAt.HasValue == true ? DateTime.SpecifyKind(entity.ExemptedAt.Value, DateTimeKind.Utc) : default(DateTime?),

                OtherProperty = entity.PropertyIdOtherPropertyNavigation?.ToViewModel(),
                VehicleProperty = entity.PropertyIdVehicleNavigation?.ToViewModel(),
                AircraftProperty = entity.PropertyIdAircraftNavigation?.ToViewModel(),
                VesselProperty = entity.PropertyIdVesselNavigation?.ToViewModel(),
                AgriculturalMachineProperty = entity.PropertyIdAgriForMachineryNavigation?.ToViewModel(),
                InFavourOfPerson = entity.InFavourOfPerson?.ToModel(),
                DebtorPerson = entity.DebtorPerson?.ToModel(),
                InFavourOfCompany = entity.InFavourOfCompany?.ToModel(),
                DebtorCompany = entity.DebtorCompany?.ToModel(),
                IsInFavourOfPerson = entity.IsInFavourOfPerson,
                IsDebtorPerson = entity.IsDebtorPerson
            };

            return model;
        }

        private static Tuple<Nullable<long>, string> GetDistraintPropertyData(Distraint entity)
        {
            Tuple<Nullable<long>, string> result = new Tuple<Nullable<long>, string>(null, null);
            long? id = 0;
            string regNumber = "";

            Shared.Enums.PropertyType convertedType;
            if (!Enum.TryParse(entity.PropertyTypeCode, out convertedType))
                return result;

            switch (convertedType)
            {
                case Shared.Enums.PropertyType.VEHICLE:
                    id = entity.PropertyIdVehicle;
                    regNumber = entity.PropertyIdVehicleNavigation?.RegistrationNumber;
                    break;
                case Shared.Enums.PropertyType.AIRCRAFT:
                    id = entity.PropertyIdAircraft;
                    regNumber = entity.PropertyIdAircraftNavigation?.MsnserialNumber;
                    break;
                case Shared.Enums.PropertyType.VESSEL:
                    id = entity.PropertyIdVessel;
                    regNumber = entity.PropertyIdVesselNavigation?.VesselRegistrationData.FirstOrDefault().RegistrationNumber;
                    break;
                case Shared.Enums.PropertyType.AGRIFORMACHINERY:
                    id = entity.PropertyIdAgriForMachinery;
                    regNumber = entity.PropertyIdAgriForMachineryNavigation?.RegistrationNumber;
                    break;
                case Shared.Enums.PropertyType.REALESTATE:
                    id = entity.PropertyIdRealEstate;
                    // TODO: regNumber
                    break;
                case Shared.Enums.PropertyType.OTHER:
                    id = entity.PropertyIdOtherProperty;
                    regNumber = entity.PropertyIdOtherPropertyNavigation?.Identifier;
                    break;
                default:
                    break;
            }

            result = new Tuple<long?, string>(id, regNumber);
            return result;
        }


    }
}
