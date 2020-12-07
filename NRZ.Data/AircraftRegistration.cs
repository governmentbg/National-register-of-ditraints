using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AircraftRegistration
    {
        public AircraftRegistration()
        {
            AircraftRegistrationOperatorEntity = new HashSet<AircraftRegistrationOperatorEntity>();
            AircraftRegistrationOperatorPerson = new HashSet<AircraftRegistrationOperatorPerson>();
            AircraftRegistrationOwnerEntity = new HashSet<AircraftRegistrationOwnerEntity>();
            AircraftRegistrationOwnerPerson = new HashSet<AircraftRegistrationOwnerPerson>();
        }

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

        public virtual Aircraft Aircraft { get; set; }
        public virtual ICollection<AircraftRegistrationOperatorEntity> AircraftRegistrationOperatorEntity { get; set; }
        public virtual ICollection<AircraftRegistrationOperatorPerson> AircraftRegistrationOperatorPerson { get; set; }
        public virtual ICollection<AircraftRegistrationOwnerEntity> AircraftRegistrationOwnerEntity { get; set; }
        public virtual ICollection<AircraftRegistrationOwnerPerson> AircraftRegistrationOwnerPerson { get; set; }
    }
}
