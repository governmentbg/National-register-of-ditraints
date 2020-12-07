using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Auction
{
    public class AuctionResult
    {
        public int AuctionId { get; set; }
        public decimal MaxPrice { get; set; }
        public string WinnerId { get; set; }
    }
}
