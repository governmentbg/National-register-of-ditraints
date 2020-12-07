using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AuctionLog
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserId { get; set; }

        public virtual Auction Auction { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
