using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AuctionItem
    {
        public int Id { get; set; }
        public string PropertyType { get; set; }
        public int? Nrzid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public int AuctionId { get; set; }
        public int? AircraftId { get; set; }
        public long? VehicleId { get; set; }
        public int? AgriculturalMachinaryId { get; set; }
        public long? VesselId { get; set; }
        public long? RealEstateId { get; set; }
        public long? OtherPropertyId { get; set; }

        public virtual AgriculturalMachinery AgriculturalMachinary { get; set; }
        public virtual Aircraft Aircraft { get; set; }
        public virtual AuctionAnnouncement Auction { get; set; }
        public virtual OtherProperty OtherProperty { get; set; }
        public virtual PropertyType PropertyTypeNavigation { get; set; }
        public virtual Property RealEstate { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
