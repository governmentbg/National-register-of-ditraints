using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AuctionBid
    {
        public AuctionBid()
        {
            InverseNextBid = new HashSet<AuctionBid>();
            InversePreviousBid = new HashSet<AuctionBid>();
        }

        public int Id { get; set; }
        public int AuctionId { get; set; }
        public decimal Bid { get; set; }
        public string BidderId { get; set; }
        public DateTime ClientTime { get; set; }
        public DateTime ServerTime { get; set; }
        public DateTime? TimeStampTime { get; set; }
        public string TimeStamp { get; set; }
        public int? PreviousBidId { get; set; }
        public int? NextBidId { get; set; }
        public bool Valid { get; set; }

        public virtual Auction Auction { get; set; }
        public virtual AspNetUsers Bidder { get; set; }
        public virtual AuctionBid NextBid { get; set; }
        public virtual AuctionBid PreviousBid { get; set; }
        public virtual ICollection<AuctionBid> InverseNextBid { get; set; }
        public virtual ICollection<AuctionBid> InversePreviousBid { get; set; }
    }
}
