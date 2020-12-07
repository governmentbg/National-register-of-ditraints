using System.ComponentModel.DataAnnotations;

namespace NRZ.Models.RequestForCertificateOfDistraintOfProperty
{
    public class RequestModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool? IsPersonalIdentifierTypeLNCh { get; set; }
        public string PersonalIdentifier { get; set; }
        public int? RegionId { get; set; }
        public int? MunicipalityId { get; set; }
        public int? CityId { get; set; }
        public string StreetAddress { get; set; }
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserTypeCode { get; set; }
        public string NameOfLegalEntity { get; set; }
        public string IdentifierOfLegalEntity { get; set; }
        public int? RegionIdOfLegalEntity { get; set; }
        public int? MunicipalityIdOfLegalEntity { get; set; }
        public int? CityIdOfLegalEntity { get; set; }
        public string StreetAddressOfLegalEntity { get; set; }
        public string CompanyCaseNumber { get; set; }
        [Required]
        public string PropertyTypeCode { get; set; }
        [Required]
        public int? PropertyId { get; set; }
    }
}
