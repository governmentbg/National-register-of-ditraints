using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class Auction
    {
        public Auction()
        {
            AuctionBid = new HashSet<AuctionBid>();
            AuctionLog = new HashSet<AuctionLog>();
        }

        public int Id { get; set; }
        public int AnnouncementId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal BidStep { get; set; }
        public decimal StartPrice { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public decimal? EndPrice { get; set; }
        public string Winner { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public virtual AuctionAnnouncement Announcement { get; set; }
        public virtual AspNetUsers CreatedByNavigation { get; set; }
        public virtual AspNetUsers DeletedByNavigation { get; set; }
        public virtual AspNetUsers WinnerNavigation { get; set; }
        public virtual ICollection<AuctionBid> AuctionBid { get; set; }
        public virtual ICollection<AuctionLog> AuctionLog { get; set; }
    }
}
