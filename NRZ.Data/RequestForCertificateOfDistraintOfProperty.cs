using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class RequestForCertificateOfDistraintOfProperty
    {
        public RequestForCertificateOfDistraintOfProperty()
        {
            EservicePaymentRequest = new HashSet<EservicePaymentRequest>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PersonalIdentifier { get; set; }
        public int? RegionId { get; set; }
        public int? MunicipalityId { get; set; }
        public int? CityId { get; set; }
        public string StreetAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserTypeCode { get; set; }
        public string NameOfLegalEntity { get; set; }
        public string IdentifierOfLegalEntity { get; set; }
        public int? RegionIdOfLegalEntity { get; set; }
        public int? MunicipalityIdOfLegalEntity { get; set; }
        public int? CityIdOfLegalEntity { get; set; }
        public string StreetAddressOfLegalEntity { get; set; }
        public string CompanyCaseNumber { get; set; }
        public long? DistraintId { get; set; }
        public string PropertyTypeCode { get; set; }
        public long? PropertyIdVehicle { get; set; }
        public int? PropertyIdAircraft { get; set; }
        public long? PropertyIdVessel { get; set; }
        public long? PropertyIdAgriForMachinery { get; set; }
        public long? PropertyIdRealEstate { get; set; }
        public long? PropertyIdOtherProperty { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool? IsPersonalIdentifierTypeLnch { get; set; }

        public virtual Cities City { get; set; }
        public virtual Cities CityIdOfLegalEntityNavigation { get; set; }
        public virtual Distraint Distraint { get; set; }
        public virtual Municipalities Municipality { get; set; }
        public virtual Municipalities MunicipalityIdOfLegalEntityNavigation { get; set; }
        public virtual Aircraft PropertyIdAircraftNavigation { get; set; }
        public virtual OtherProperty PropertyIdOtherPropertyNavigation { get; set; }
        public virtual Vehicle PropertyIdVehicleNavigation { get; set; }
        public virtual PropertyType PropertyTypeCodeNavigation { get; set; }
        public virtual Regions Region { get; set; }
        public virtual Regions RegionIdOfLegalEntityNavigation { get; set; }
        public virtual ICollection<EservicePaymentRequest> EservicePaymentRequest { get; set; }
    }
}
