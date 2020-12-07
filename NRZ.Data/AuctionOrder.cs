using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AuctionOrder
    {
        public AuctionOrder()
        {
            AuctionAnnouncement = new HashSet<AuctionAnnouncement>();
            AuctionOrderSettings = new HashSet<AuctionOrderSettings>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }

        public virtual ICollection<AuctionAnnouncement> AuctionAnnouncement { get; set; }
        public virtual ICollection<AuctionOrderSettings> AuctionOrderSettings { get; set; }
    }
}
