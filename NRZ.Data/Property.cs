using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class Property
    {
        public Property()
        {
            AuctionItem = new HashSet<AuctionItem>();
        }

        public long Id { get; set; }
        public string Type { get; set; }
        public string Floor { get; set; }
        public decimal Area { get; set; }
        public int? AddressId { get; set; }
        public string PropertyConstructionTypeId { get; set; }
        public string Identifier { get; set; }
        public string IdentifierType { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsManuallyAdded { get; set; }

        public virtual Address Address { get; set; }
        public virtual AspNetUsers CreatedByNavigation { get; set; }
        public virtual PropertyConstructionType PropertyConstructionType { get; set; }
        public virtual RealEstateType TypeNavigation { get; set; }
        public virtual AspNetUsers UpdatedByNavigation { get; set; }
        public virtual ICollection<AuctionItem> AuctionItem { get; set; }
    }
}
