using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Property
{
    public class AircraftRegistrationViewModel
    {
        public int Id { get; set; }
        public int AircraftId { get; set; }
        public int? ActNumber { get; set; }
        public int? RegistrationNumber { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string RegistrationMark { get; set; }
        public bool? IsLastRegistration { get; set; }
        public bool? RegistrationStatusIsActive { get; set; }
        public int? RegistrationStatusCode { get; set; }
        public string RegistrationStatusName { get; set; }
        public string RegistrationDocumentNumber { get; set; }
        public DateTime? RegistrationDocumentDate { get; set; }
        public string RegistrationDocumentDescription { get; set; }
        public DateTime? DeregistrationDate { get; set; }
        public string DeregistrationReason { get; set; }
        public string DeregistrationDescription { get; set; }
        public string DeregistrationCountryCode { get; set; }
        public string DeregistrationCountryName { get; set; }
        public string LeasingDocumentNumber { get; set; }
        public DateTime? LeasingDocumentDate { get; set; }
        public DateTime? LeasingEndDate { get; set; }
        public string LeasingAgreement { get; set; }
        public string LeasingLessorPersonIdentifier { get; set; }
        public string LeasingLessorPersonNames { get; set; }
        public string LeasingLessorEntityIdentifier { get; set; }
        public string LeasingLessorEntityName { get; set; }


        public IEnumerable<AircraftRegistrationOperatorEntityViewModel> OperatorEntities { get; set; }
        public IEnumerable<AircraftRegistrationOperatorPersonViewModel> OperatorPeople { get; set; }
        public IEnumerable<AircraftRegistrationOwnerEntityViewModel> OwnerEntities { get; set; }
        public IEnumerable<AircraftRegistrationOwnerPersonViewModel> OwnerPeople { get; set; }
    }
}
