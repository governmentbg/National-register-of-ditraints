using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class Company
    {
        public Company()
        {
            AgriculturalMachinery = new HashSet<AgriculturalMachinery>();
            AuctionRegistration = new HashSet<AuctionRegistration>();
            SeizedPropertyAvailabilityRequestCheckedCompany = new HashSet<SeizedPropertyAvailabilityRequest>();
            SeizedPropertyAvailabilityRequestRequesterCompany = new HashSet<SeizedPropertyAvailabilityRequest>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Eik { get; set; }
        public int? AddressId { get; set; }
        public string CompanyCaseNumber { get; set; }

        public virtual Address Address { get; set; }
        public virtual ICollection<AgriculturalMachinery> AgriculturalMachinery { get; set; }
        public virtual ICollection<AuctionRegistration> AuctionRegistration { get; set; }
        public virtual ICollection<SeizedPropertyAvailabilityRequest> SeizedPropertyAvailabilityRequestCheckedCompany { get; set; }
        public virtual ICollection<SeizedPropertyAvailabilityRequest> SeizedPropertyAvailabilityRequestRequesterCompany { get; set; }
    }
}
