using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AuctionLogs
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Auction Auction { get; set; }
    }
}
