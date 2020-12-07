using NRZ.Models.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Auction
{
    public class BidModel
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public decimal Bid { get; set; }
        public string BidderId { get; set; }
        public PersonModel Bidder { get; set; }
        public DateTime ClientTime { get; set; }
        public DateTime ServerTime { get; set; }
        public DateTime TimestampTime { get; set; }
        public string TimeStamp { get; set; }
        public int?  PreviousBidId { get; set; }
        public int NextBidId { get; set; }
        public bool Valid { get; set; }
    }
}
