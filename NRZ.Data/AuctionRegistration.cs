using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AuctionRegistration
    {
        public AuctionRegistration()
        {
            AuctionRegistrationAttachment = new HashSet<AuctionRegistrationAttachment>();
        }

        public int Id { get; set; }
        public string UniqueNumber { get; set; }
        public string ParticipantId { get; set; }
        public int AnnouncementId { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public string ProcessedBy { get; set; }
        public string RepresentationType { get; set; }
        public bool IsOwner { get; set; }
        public bool IsOwnerSpouse { get; set; }
        public bool AppliedByCourtEnforcer { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string RepresentedUserId { get; set; }
        public int? RepresentedPersonId { get; set; }
        public int? RepresentedCompanyId { get; set; }
        public string ResultDeliveryType { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public string RejectReason { get; set; }

        public virtual AuctionAnnouncement Announcement { get; set; }
        public virtual AspNetUsers CreatedByNavigation { get; set; }
        public virtual AspNetUsers DeletedByNavigation { get; set; }
        public virtual AspNetUsers Participant { get; set; }
        public virtual AspNetUsers ProcessedByNavigation { get; set; }
        public virtual AuctionRepresentationType RepresentationTypeNavigation { get; set; }
        public virtual Company RepresentedCompany { get; set; }
        public virtual Person RepresentedPerson { get; set; }
        public virtual AspNetUsers RepresentedUser { get; set; }
        public virtual AuctionResultDeliveryType ResultDeliveryTypeNavigation { get; set; }
        public virtual ICollection<AuctionRegistrationAttachment> AuctionRegistrationAttachment { get; set; }
    }
}
