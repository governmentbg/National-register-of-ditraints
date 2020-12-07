using NRZ.Models.Company;
using NRZ.Models.Person;
using NRZ.Models.Property;
using NRZ.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Distraint
{
    public class DistraintCreateModel
    {
        public long Id { get; set; }
        public string PropertyTypeCode { get; set; }
        public bool IsNewProperty { get; set; }
        public long? PropertyIdVehicle { get; set; }
        public int? PropertyIdAircraft { get; set; }
        public int? PropertyIdVessel { get; set; }
        public int? PropertyIdAgriForMachinery { get; set; }
        public long? PropertyIdRealEstate { get; set; }
        public long? PropertyIdOtherProperty { get; set; }

        public OtherPropertyModel NewOtherProperty { get; set; }
        public VehicleViewModel VehicleProperty { get; set; }
        public AircraftViewModel AircraftProperty { get; set; }
        public VesselViewModel VesselProperty { get; set; }
        public AgriculturalMachineryViewModel AgriculturalMachineProperty { get; set; }
        public string SearchedIdentifier { get; set; }


        public string StatusCode { get; set; }        
        public string InFavourOf { get; set; }
        public string SuitNumber { get; set; }
        public string Debtor { get; set; }
        public string CreatedBy { get; set; }
        public string Location { get; set; }
        public DateTime? EnforcementDate { get; set; }
        public string EnforcedBy { get; set; }
        public DateTime? EnforcedAt { get; set; }

        public RegixPersonModel InFavourOfPerson { get; set; }
        public RegixPersonModel DebtorPerson { get; set; }

        public RegixCompanyModel InFavourOfCompany { get; set; }
        public RegixCompanyModel DebtorCompany { get; set; }

        public bool IsInFavourOfPerson { get; set; }
        public bool IsDebtorPerson { get; set; }
    }
}
