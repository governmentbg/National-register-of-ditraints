using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AuctionAnnouncement
    {
        public AuctionAnnouncement()
        {
            AnnouncementAttachments = new HashSet<AnnouncementAttachments>();
            Auction = new HashSet<Auction>();
            AuctionItem = new HashSet<AuctionItem>();
            AuctionRegistration = new HashSet<AuctionRegistration>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string PropertyType { get; set; }
        public decimal StartPrice { get; set; }
        public decimal BidStep { get; set; }
        public DateTime RegisterStartDate { get; set; }
        public DateTime RegisterEndDate { get; set; }
        public DateTime AuctionStartDate { get; set; }
        public DateTime AuctionEndDate { get; set; }
        public int? AttachmentId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public bool Deleted { get; set; }
        public string Description { get; set; }
        public string Order { get; set; }

        public virtual Attachment Attachment { get; set; }
        public virtual AspNetUsers CreatedByNavigation { get; set; }
        public virtual AspNetUsers DeletedByNavigation { get; set; }
        public virtual AuctionOrder OrderNavigation { get; set; }
        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<AnnouncementAttachments> AnnouncementAttachments { get; set; }
        public virtual ICollection<Auction> Auction { get; set; }
        public virtual ICollection<AuctionItem> AuctionItem { get; set; }
        public virtual ICollection<AuctionRegistration> AuctionRegistration { get; set; }
    }
}
