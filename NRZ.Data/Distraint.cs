using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class Distraint
    {
        public Distraint()
        {
            RequestForCertificateOfDistraintOfProperty = new HashSet<RequestForCertificateOfDistraintOfProperty>();
        }

        public long Id { get; set; }
        public long? PropertyIdVehicle { get; set; }
        public int? PropertyIdAircraft { get; set; }
        public int? PropertyIdVessel { get; set; }
        public int? PropertyIdAgriForMachinery { get; set; }
        public long? PropertyIdRealEstate { get; set; }
        public long? PropertyIdOtherProperty { get; set; }
        public string StatusCode { get; set; }
        public string InFavourOf { get; set; }
        public string SuitNumber { get; set; }
        public string Debtor { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deactivated { get; set; }
        public string PropertyTypeCode { get; set; }
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
        public int? InFavourOfPersonId { get; set; }
        public int? DebtorPersonId { get; set; }
        public int? InFavourOfCompanyId { get; set; }
        public int? DebtorCompanyId { get; set; }
        public bool IsInFavourOfPerson { get; set; }
        public bool IsDebtorPerson { get; set; }
        public bool IsImported { get; set; }

        public virtual AspNetUsers CreatedByNavigation { get; set; }
        public virtual RegixCompany DebtorCompany { get; set; }
        public virtual RegixPerson DebtorPerson { get; set; }
        public virtual AspNetUsers EnforcedByNavigation { get; set; }
        public virtual AspNetUsers ExemptedByNavigation { get; set; }
        public virtual RegixCompany InFavourOfCompany { get; set; }
        public virtual RegixPerson InFavourOfPerson { get; set; }
        public virtual AgriculturalMachinery PropertyIdAgriForMachineryNavigation { get; set; }
        public virtual Aircraft PropertyIdAircraftNavigation { get; set; }
        public virtual OtherProperty PropertyIdOtherPropertyNavigation { get; set; }
        public virtual Vehicle PropertyIdVehicleNavigation { get; set; }
        public virtual Vessel PropertyIdVesselNavigation { get; set; }
        public virtual PropertyType PropertyTypeCodeNavigation { get; set; }
        public virtual AspNetUsers RevokedByNavigation { get; set; }
        public virtual DistraintStatus StatusCodeNavigation { get; set; }
        public virtual AspNetUsers UpdatedByNavigation { get; set; }
        public virtual ICollection<RequestForCertificateOfDistraintOfProperty> RequestForCertificateOfDistraintOfProperty { get; set; }
    }
}
