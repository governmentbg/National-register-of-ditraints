using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AuctionResultDeliveryType
    {
        public AuctionResultDeliveryType()
        {
            AuctionRegistration = new HashSet<AuctionRegistration>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool Deactivated { get; set; }
        public int Sort { get; set; }

        public virtual ICollection<AuctionRegistration> AuctionRegistration { get; set; }
    }
}
