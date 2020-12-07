using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class Person
    {
        public Person()
        {
            AgriculturalMachinery = new HashSet<AgriculturalMachinery>();
            AuctionRegistration = new HashSet<AuctionRegistration>();
            SeizedPropertyAvailabilityRequestCheckedPerson = new HashSet<SeizedPropertyAvailabilityRequest>();
            SeizedPropertyAvailabilityRequestRequestorPerson = new HashSet<SeizedPropertyAvailabilityRequest>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool Deleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string IdentificationNumber { get; set; }
        public string IdentificationNumberType { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual AspNetUsers CreatedByNavigation { get; set; }
        public virtual AspNetUsers DeletedByNavigation { get; set; }
        public virtual IdentificationType IdentificationNumberTypeNavigation { get; set; }
        public virtual AspNetUsers UpdatedByNavigation { get; set; }
        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<AgriculturalMachinery> AgriculturalMachinery { get; set; }
        public virtual ICollection<AuctionRegistration> AuctionRegistration { get; set; }
        public virtual ICollection<SeizedPropertyAvailabilityRequest> SeizedPropertyAvailabilityRequestCheckedPerson { get; set; }
        public virtual ICollection<SeizedPropertyAvailabilityRequest> SeizedPropertyAvailabilityRequestRequestorPerson { get; set; }
    }
}
