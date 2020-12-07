using NRZ.Models.Distraint;
using NRZ.Models.Nomenclatures;
using NRZ.Models.EAuthentication;
using System;

namespace NRZ.Models.RequestForCertificateOfDistraintOfProperty
{
    public class RequestViewModel
    {
        public RequestViewModel(
            long id,
            string firstName,
            string middleName,
            string lastName,
            bool? isPersonalIdentifierTypeLnch,
            string personalIdentifier,
            string region,
            string municipality,
            string city,
            string streetAddress,
            string phoneNumber,
            string email,
            string userType,
            string nameOfLegalEntity,
            string identifierOfLegalEntity,
            string regionOfLegalEntity,
            string municipalityOfLegalEntity,
            string cityOfLegalEntity,
            string streetAddressOfLegalEntity,
            string companyCaseNumber,
            string propertyTypeName,
            string propertyTypeNameEn,
            long? propertyIdOtherProperty,
            long? propertyIdVehicle,
            int? propertyIdAircraft,
            string otherPropertyIdentifier,
            string vehicleRegistrationNumber,
            string aircraftMsnserialNumber,
            DateTime createdOn,
            string statusCode,
            string statusName,
            string statusNameEn)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.MiddleName = middleName;
            this.LastName = lastName;
            if (isPersonalIdentifierTypeLnch == null)
            {
                this.personalIdentifierType = String.Empty;
            }
            else if (isPersonalIdentifierTypeLnch.Value) 
            {
                this.personalIdentifierType = "ЛНЧ";
            }
            else
            {
                this.personalIdentifierType = "ЕГН";
            }
            this.PersonalIdentifier = personalIdentifier;
            this.Region = region;
            this.Municipality = municipality;
            this.City = city;
            this.StreetAddress = streetAddress;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.UserType = userType;
            this.NameOfLegalEntity = nameOfLegalEntity;
            this.IdentifierOfLegalEntity = identifierOfLegalEntity;
            this.RegionOfLegalEntity = regionOfLegalEntity;
            this.MunicipalityOfLegalEntity = municipalityOfLegalEntity;
            this.CityOfLegalEntity = cityOfLegalEntity;
            this.StreetAddressOfLegalEntity = streetAddressOfLegalEntity;
            this.CompanyCaseNumber = companyCaseNumber;
            this.PropertyTypeName = propertyTypeName;
            this.PropertyTypeNameEn = propertyTypeNameEn;
            this.PropertyIdOtherProperty = propertyIdOtherProperty;
            this.PropertyIdVehicle = propertyIdVehicle;
            this.PropertyIdAircraft = propertyIdAircraft;
            this.OtherPropertyIdentifier = otherPropertyIdentifier;
            this.VehicleRegistrationNumber = vehicleRegistrationNumber;
            this.AircraftMsnserialNumber = aircraftMsnserialNumber;
            this.CreatedOn = createdOn;
            this.StatusCode = statusCode;
            this.StatusName = statusName;
            this.StatusNameEn = statusNameEn;
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string personalIdentifierType { get; set; }
        public string PersonalIdentifier { get; set; }
        public string Region { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public string NameOfLegalEntity { get; set; }
        public string IdentifierOfLegalEntity { get; set; }
        public string RegionOfLegalEntity { get; set; }
        public string MunicipalityOfLegalEntity { get; set; }
        public string CityOfLegalEntity { get; set; }
        public string StreetAddressOfLegalEntity { get; set; }
        public string CompanyCaseNumber { get; set; }
        public string PropertyTypeName { get; set; }
        public string PropertyTypeNameEn { get; set; }
        public long? PropertyIdOtherProperty { get; set; }
        public long? PropertyIdVehicle { get; set; }
        public int? PropertyIdAircraft { get; set; }
        public string OtherPropertyIdentifier { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public string AircraftMsnserialNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string StatusNameEn { get; set; }
    }
}
