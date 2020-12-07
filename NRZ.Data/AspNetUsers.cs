using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
            AuctionAnnouncementCreatedByNavigation = new HashSet<AuctionAnnouncement>();
            AuctionAnnouncementDeletedByNavigation = new HashSet<AuctionAnnouncement>();
            AuctionAnnouncementUser = new HashSet<AuctionAnnouncement>();
            AuctionBid = new HashSet<AuctionBid>();
            AuctionCreatedByNavigation = new HashSet<Auction>();
            AuctionDeletedByNavigation = new HashSet<Auction>();
            AuctionLog = new HashSet<AuctionLog>();
            AuctionRegistrationCreatedByNavigation = new HashSet<AuctionRegistration>();
            AuctionRegistrationDeletedByNavigation = new HashSet<AuctionRegistration>();
            AuctionRegistrationParticipant = new HashSet<AuctionRegistration>();
            AuctionRegistrationProcessedByNavigation = new HashSet<AuctionRegistration>();
            AuctionRegistrationRepresentedUser = new HashSet<AuctionRegistration>();
            AuctionWinnerNavigation = new HashSet<Auction>();
            DistraintCreatedByNavigation = new HashSet<Distraint>();
            DistraintEnforcedByNavigation = new HashSet<Distraint>();
            DistraintExemptedByNavigation = new HashSet<Distraint>();
            DistraintRevokedByNavigation = new HashSet<Distraint>();
            DistraintUpdatedByNavigation = new HashSet<Distraint>();
            InverseApprovedByNavigation = new HashSet<AspNetUsers>();
            InverseCreatedByNavigation = new HashSet<AspNetUsers>();
            InverseDeletedByNavigation = new HashSet<AspNetUsers>();
            InverseUpdatedByNavigation = new HashSet<AspNetUsers>();
            PersonCreatedByNavigation = new HashSet<Person>();
            PersonDeletedByNavigation = new HashSet<Person>();
            PersonUpdatedByNavigation = new HashSet<Person>();
            PersonUser = new HashSet<Person>();
            PropertyCreatedByNavigation = new HashSet<Property>();
            PropertyUpdatedByNavigation = new HashSet<Property>();
            RegiXrequest = new HashSet<RegiXrequest>();
            SeizedPropertyAvailabilityRequest = new HashSet<SeizedPropertyAvailabilityRequest>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public byte[] Certificate { get; set; }
        public string CertificateThumbprint { get; set; }
        public string CertificateName { get; set; }
        public string CertificateUniqueIdentifier { get; set; }
        public bool Deleted { get; set; }
        public bool ConfirmedByAdmin { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string AuthType { get; set; }
        public string UserType { get; set; }
        public string Chsinumber { get; set; }
        public bool? CheckedInChsiregister { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public virtual AspNetUsers ApprovedByNavigation { get; set; }
        public virtual UserRegisterType AuthTypeNavigation { get; set; }
        public virtual AspNetUsers CreatedByNavigation { get; set; }
        public virtual AspNetUsers DeletedByNavigation { get; set; }
        public virtual AspNetUsers UpdatedByNavigation { get; set; }
        public virtual UserType UserTypeNavigation { get; set; }
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual ICollection<AuctionAnnouncement> AuctionAnnouncementCreatedByNavigation { get; set; }
        public virtual ICollection<AuctionAnnouncement> AuctionAnnouncementDeletedByNavigation { get; set; }
        public virtual ICollection<AuctionAnnouncement> AuctionAnnouncementUser { get; set; }
        public virtual ICollection<AuctionBid> AuctionBid { get; set; }
        public virtual ICollection<Auction> AuctionCreatedByNavigation { get; set; }
        public virtual ICollection<Auction> AuctionDeletedByNavigation { get; set; }
        public virtual ICollection<AuctionLog> AuctionLog { get; set; }
        public virtual ICollection<AuctionRegistration> AuctionRegistrationCreatedByNavigation { get; set; }
        public virtual ICollection<AuctionRegistration> AuctionRegistrationDeletedByNavigation { get; set; }
        public virtual ICollection<AuctionRegistration> AuctionRegistrationParticipant { get; set; }
        public virtual ICollection<AuctionRegistration> AuctionRegistrationProcessedByNavigation { get; set; }
        public virtual ICollection<AuctionRegistration> AuctionRegistrationRepresentedUser { get; set; }
        public virtual ICollection<Auction> AuctionWinnerNavigation { get; set; }
        public virtual ICollection<Distraint> DistraintCreatedByNavigation { get; set; }
        public virtual ICollection<Distraint> DistraintEnforcedByNavigation { get; set; }
        public virtual ICollection<Distraint> DistraintExemptedByNavigation { get; set; }
        public virtual ICollection<Distraint> DistraintRevokedByNavigation { get; set; }
        public virtual ICollection<Distraint> DistraintUpdatedByNavigation { get; set; }
        public virtual ICollection<AspNetUsers> InverseApprovedByNavigation { get; set; }
        public virtual ICollection<AspNetUsers> InverseCreatedByNavigation { get; set; }
        public virtual ICollection<AspNetUsers> InverseDeletedByNavigation { get; set; }
        public virtual ICollection<AspNetUsers> InverseUpdatedByNavigation { get; set; }
        public virtual ICollection<Person> PersonCreatedByNavigation { get; set; }
        public virtual ICollection<Person> PersonDeletedByNavigation { get; set; }
        public virtual ICollection<Person> PersonUpdatedByNavigation { get; set; }
        public virtual ICollection<Person> PersonUser { get; set; }
        public virtual ICollection<Property> PropertyCreatedByNavigation { get; set; }
        public virtual ICollection<Property> PropertyUpdatedByNavigation { get; set; }
        public virtual ICollection<RegiXrequest> RegiXrequest { get; set; }
        public virtual ICollection<SeizedPropertyAvailabilityRequest> SeizedPropertyAvailabilityRequest { get; set; }
    }
}
