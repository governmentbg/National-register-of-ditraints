using NRZ.Models.Company;
using NRZ.Models.Nomenclatures;
using NRZ.Models.Person;
using NRZ.Models.Property;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Distraint
{
    public class DistraintViewModel
    {
        public long Id { get; set; }
        public long? PropertyId { get; set; }
        public string PropertyRegNumber { get; set; }
        public string PropertyTypeCode { get; set; }
        public string PropertyTypeName { get; set; }
        public string PropertyTypeNameEn { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string StatusNameEn { get; set; }
        public string InFavourOf => ( IsInFavourOfPerson == true ? InFavourOfPerson?.FullName : InFavourOfCompany?.Name);
        public string InFavourOfIdentifier => (IsInFavourOfPerson == true ? InFavourOfPerson?.Identifier : InFavourOfCompany?.Uic);
        public string SuitNumber { get; set; }
        public string Debtor => (IsDebtorPerson == true ? DebtorPerson?.FullName : DebtorCompany?.Name);
        public string DebtorIdentifier => (IsDebtorPerson == true ? DebtorPerson?.Identifier : DebtorCompany?.Uic);
        public bool Deactivated { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedByUserType { get; set; }
        public string CreatedByUserName { get; set; }
        public string Location { get; set; }
        public DateTime? EnforcementDate { get; set; }
        public string EnforcedBy { get; set; }
        public DateTime? EnforcedAt { get; set; }
        public DateTime? RevocationDate { get; set; }
        public string RevokedBy { get; set; }
        public DateTime? RevokedAt { get; set; }
        public DateTime? ExemptionDate { get; set; }
        public string ExemptedBy { get; set; }
        public DateTime? ExemptedAt { get; set; }

        public OtherPropertyModel OtherProperty { get; set; }
        public VehicleViewModel VehicleProperty { get; set; }
        public AircraftViewModel AircraftProperty { get; set; }
        public VesselViewModel VesselProperty { get; set; }
        public AgriculturalMachineryViewModel AgriculturalMachineProperty { get; set; }


        public RegixPersonModel InFavourOfPerson { get; set; }
        public RegixPersonModel DebtorPerson { get; set; }

        public RegixCompanyModel InFavourOfCompany { get; set; }
        public RegixCompanyModel DebtorCompany { get; set; }

        public bool IsInFavourOfPerson { get; set; }
        public bool IsDebtorPerson { get; set; }

    }
}
