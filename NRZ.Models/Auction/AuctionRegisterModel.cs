using NRZ.Models.Company;
using NRZ.Models.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Auction
{
    public class AuctionRegisterModel
    {
        public int Id { get; set; }
        public string UniqueNumber { get; set; }
        public string ParticipantId { get; set; }
        public PersonModel Participant { get; set; }
        public int AnnouncementId { get; set; }
        public string AnnouncementCreatedBy { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public string ProcessedBy { get; set; }
        public string RepresentationType { get; set; }
        public string RepresentationTypeName { get; set; }
        public bool IsOwner { get; set; }
        public bool IsOwnerSpouse { get; set; }
        public bool AppliedByCourtEnforcer { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string RepresentedUserId { get; set; }
        public int? RepresentedPersonId { get; set; }
        public PersonModel RepresentedPerson { get; set; }
        public int? RepresentedCompanyId { get; set; }
        public CompanyModel RepresentedCompany { get; set; }
        public string ResultDeliveryType { get; set; }
        public string ResultDeliveryTypeName { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public bool Deleted { get; set; }
        public string RejectReason { get; set; }
        public IEnumerable<AttachmentModel> Attachments { get; set; }
    }


    public class AnnouncemetRegisterGridModel
    {
        public int Id { get; set; }
        public string UniqueNumber { get; set; }
        public int AnnouncementId { get; set; }
        public string AnnouncementTitle { get; set; }
        public string AnnouncementCode { get; set; }
        public PersonModel Participant { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public string ProcessedBy { get; set; }
        public string RejectReason { get; set; }
    }
}