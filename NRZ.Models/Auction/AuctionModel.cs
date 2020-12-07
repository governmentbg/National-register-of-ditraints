using NRZ.Models.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Auction
{
    public class AuctionModel
    {
        public int Id { get; set; }
        public int AnnouncementId { get; set; }
        public AuctionAnnouncementModel Announcement { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TimeSpan { 
            get {
                var timeSpan = EndDate - DateTime.UtcNow;
                return timeSpan.TotalMilliseconds;
            }
        }
        public decimal BidStep { get; set; }
        public decimal StartPrice { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal? EndPrice { get; set; }
        public string WinnerId { get; set; }
        public PersonModel Winner { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public bool Completed { get; set; }
        public bool Started { get; set; }
        public int ItemsCount { get; set; }
        public bool CanParticipate { get; set; }
        public bool CanVueResult { get; set; }
        public IEnumerable<BidModel> Bids { get; set; }
    }
}
