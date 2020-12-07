using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRZ.Models.Auction
{
    public class AuctionsListResult
    {
        public IQueryable<AuctionModel> Ongoing { get; set; }
        public IQueryable<AuctionModel> Future { get; set; }
        public IQueryable<AuctionModel> Finished { get; set; }
    }
}
