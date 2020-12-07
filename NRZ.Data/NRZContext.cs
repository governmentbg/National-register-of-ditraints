using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NRZ.Data
{
    public partial class NRZContext : DbContext
    {
        public NRZContext()
        {
        }

        public NRZContext(DbContextOptions<NRZContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActiveJwt> ActiveJwt { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<AggregatedCounter> AggregatedCounter { get; set; }
        public virtual DbSet<AgriculturalMachinery> AgriculturalMachinery { get; set; }
        public virtual DbSet<Aircraft> Aircraft { get; set; }
        public virtual DbSet<AircraftDebt> AircraftDebt { get; set; }
        public virtual DbSet<AircraftExtension> AircraftExtension { get; set; }
        public virtual DbSet<AircraftRegistration> AircraftRegistration { get; set; }
        public virtual DbSet<AircraftRegistrationOperatorEntity> AircraftRegistrationOperatorEntity { get; set; }
        public virtual DbSet<AircraftRegistrationOperatorPerson> AircraftRegistrationOperatorPerson { get; set; }
        public virtual DbSet<AircraftRegistrationOwnerEntity> AircraftRegistrationOwnerEntity { get; set; }
        public virtual DbSet<AircraftRegistrationOwnerPerson> AircraftRegistrationOwnerPerson { get; set; }
        public virtual DbSet<AnnouncementAttachments> AnnouncementAttachments { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AssetType> AssetType { get; set; }
        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<Auction> Auction { get; set; }
        public virtual DbSet<AuctionAnnouncement> AuctionAnnouncement { get; set; }
        public virtual DbSet<AuctionBid> AuctionBid { get; set; }
        public virtual DbSet<AuctionItem> AuctionItem { get; set; }
        public virtual DbSet<AuctionLog> AuctionLog { get; set; }
        public virtual DbSet<AuctionOrder> AuctionOrder { get; set; }
        public virtual DbSet<AuctionOrderSettings> AuctionOrderSettings { get; set; }
        public virtual DbSet<AuctionRegistration> AuctionRegistration { get; set; }
        public virtual DbSet<AuctionRegistrationAttachment> AuctionRegistrationAttachment { get; set; }
        public virtual DbSet<AuctionRepresentationType> AuctionRepresentationType { get; set; }
        public virtual DbSet<AuctionResultDeliveryType> AuctionResultDeliveryType { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Counter> Counter { get; set; }
        public virtual DbSet<Distraint> Distraint { get; set; }
        public virtual DbSet<DistraintStatus> DistraintStatus { get; set; }
        public virtual DbSet<EservicePaymentRequest> EservicePaymentRequest { get; set; }
        public virtual DbSet<EservicePaymentRequestStatusHistory> EservicePaymentRequestStatusHistory { get; set; }
        public virtual DbSet<EserviceType> EserviceType { get; set; }
        public virtual DbSet<EservicesSettings> EservicesSettings { get; set; }
        public virtual DbSet<Hash> Hash { get; set; }
        public virtual DbSet<IdentificationType> IdentificationType { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<JobParameter> JobParameter { get; set; }
        public virtual DbSet<JobQueue> JobQueue { get; set; }
        public virtual DbSet<List> List { get; set; }
        public virtual DbSet<Municipalities> Municipalities { get; set; }
        public virtual DbSet<OtherProperty> OtherProperty { get; set; }
        public virtual DbSet<PaymentRequest> PaymentRequest { get; set; }
        public virtual DbSet<PaymentRequestStatus> PaymentRequestStatus { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Property> Property { get; set; }
        public virtual DbSet<PropertyConstructionType> PropertyConstructionType { get; set; }
        public virtual DbSet<PropertyExtra> PropertyExtra { get; set; }
        public virtual DbSet<PropertyType> PropertyType { get; set; }
        public virtual DbSet<RealEstateType> RealEstateType { get; set; }
        public virtual DbSet<RegiXreport> RegiXreport { get; set; }
        public virtual DbSet<RegiXreportToPropertyType> RegiXreportToPropertyType { get; set; }
        public virtual DbSet<RegiXrequest> RegiXrequest { get; set; }
        public virtual DbSet<RegiXresponse> RegiXresponse { get; set; }
        public virtual DbSet<RegiXsearchCriteriaType> RegiXsearchCriteriaType { get; set; }
        public virtual DbSet<Regions> Regions { get; set; }
        public virtual DbSet<RegixCompany> RegixCompany { get; set; }
        public virtual DbSet<RegixCompanyStatus> RegixCompanyStatus { get; set; }
        public virtual DbSet<RegixPerson> RegixPerson { get; set; }
        public virtual DbSet<RequestForCertificateOfDistraintOfProperty> RequestForCertificateOfDistraintOfProperty { get; set; }
        public virtual DbSet<RequesterType> RequesterType { get; set; }
        public virtual DbSet<Schema> Schema { get; set; }
        public virtual DbSet<SeizedPropertyAvailabilityRequest> SeizedPropertyAvailabilityRequest { get; set; }
        public virtual DbSet<Server> Server { get; set; }
        public virtual DbSet<Set> Set { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<UserLogs> UserLogs { get; set; }
        public virtual DbSet<UserRegisterType> UserRegisterType { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }
        public virtual DbSet<Vehicle> Vehicle { get; set; }
        public virtual DbSet<VehicleExtension> VehicleExtension { get; set; }
        public virtual DbSet<VehicleOwner> VehicleOwner { get; set; }
        public virtual DbSet<VehicleUser> VehicleUser { get; set; }
        public virtual DbSet<Vessel> Vessel { get; set; }
        public virtual DbSet<VesselEngine> VesselEngine { get; set; }
        public virtual DbSet<VesselExtension> VesselExtension { get; set; }
        public virtual DbSet<VesselOwner> VesselOwner { get; set; }
        public virtual DbSet<VesselRegistrationData> VesselRegistrationData { get; set; }
        public virtual DbSet<VesselStatus> VesselStatus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActiveJwt>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ActiveJWT");

                entity.Property(e => e.ExpDateUtc)
                    .HasColumnName("ExpDateUTC")
                    .HasColumnType("date");

                entity.Property(e => e.Jwt)
                    .IsRequired()
                    .HasColumnName("JWT");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.StreetAddress).IsRequired();

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Address_Cities");

                entity.HasOne(d => d.Municipality)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.MunicipalityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Address_Municipalities");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Address_Regions");
            });

            modelBuilder.Entity<AggregatedCounter>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("PK_HangFire_CounterAggregated");

                entity.ToTable("AggregatedCounter", "HangFire");

                entity.HasIndex(e => e.ExpireAt)
                    .HasName("IX_HangFire_AggregatedCounter_ExpireAt")
                    .HasFilter("([ExpireAt] IS NOT NULL)");

                entity.Property(e => e.Key).HasMaxLength(100);

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<AgriculturalMachinery>(entity =>
            {
                entity.HasIndex(e => e.CompanyId)
                    .HasName("fki_Owner_Company");

                entity.HasIndex(e => e.OwnerId)
                    .HasName("fki_Owner_Person");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedOn).HasColumnType("date");

                entity.Property(e => e.FrameNumber).IsRequired();

                entity.Property(e => e.RegistrationNumber).IsRequired();

                entity.Property(e => e.Type).IsRequired();

                entity.Property(e => e.UpdatedOn).HasColumnType("date");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.AgriculturalMachinery)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("owner_company");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.AgriculturalMachinery)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("owner_person");
            });

            modelBuilder.Entity<Aircraft>(entity =>
            {
                entity.Property(e => e.Icao).HasColumnName("ICAO");

                entity.Property(e => e.MsnserialNumber)
                    .IsRequired()
                    .HasColumnName("MSNSerialNumber");
            });

            modelBuilder.Entity<AircraftDebt>(entity =>
            {
                entity.HasOne(d => d.Aircraft)
                    .WithMany(p => p.AircraftDebt)
                    .HasForeignKey(d => d.AircraftId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("aircraft_fkey");
            });

            modelBuilder.Entity<AircraftExtension>(entity =>
            {
                entity.HasKey(e => e.AircraftId)
                    .HasName("AircraftExtension_pkey");

                entity.Property(e => e.AircraftId).ValueGeneratedNever();

                entity.HasOne(d => d.Aircraft)
                    .WithOne(p => p.AircraftExtension)
                    .HasForeignKey<AircraftExtension>(d => d.AircraftId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("aircraft_fkey1");
            });

            modelBuilder.Entity<AircraftRegistration>(entity =>
            {
                entity.HasOne(d => d.Aircraft)
                    .WithMany(p => p.AircraftRegistration)
                    .HasForeignKey(d => d.AircraftId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("aircraft_fkey2");
            });

            modelBuilder.Entity<AircraftRegistrationOperatorEntity>(entity =>
            {
                entity.HasOne(d => d.Registration)
                    .WithMany(p => p.AircraftRegistrationOperatorEntity)
                    .HasForeignKey(d => d.RegistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("registration_fkey");
            });

            modelBuilder.Entity<AircraftRegistrationOperatorPerson>(entity =>
            {
                entity.HasOne(d => d.Registration)
                    .WithMany(p => p.AircraftRegistrationOperatorPerson)
                    .HasForeignKey(d => d.RegistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("registration_fkey1");
            });

            modelBuilder.Entity<AircraftRegistrationOwnerEntity>(entity =>
            {
                entity.HasOne(d => d.Registration)
                    .WithMany(p => p.AircraftRegistrationOwnerEntity)
                    .HasForeignKey(d => d.RegistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("registration_fkey2");
            });

            modelBuilder.Entity<AircraftRegistrationOwnerPerson>(entity =>
            {
                entity.HasOne(d => d.Registration)
                    .WithMany(p => p.AircraftRegistrationOwnerPerson)
                    .HasForeignKey(d => d.RegistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("registration_fkey3");
            });

            modelBuilder.Entity<AnnouncementAttachments>(entity =>
            {
                entity.HasKey(e => new { e.AnnouncementId, e.AttachmentId })
                    .HasName("AnouncementAttachments_pkey");

                entity.HasOne(d => d.Announcement)
                    .WithMany(p => p.AnnouncementAttachments)
                    .HasForeignKey(d => d.AnnouncementId)
                    .HasConstraintName("announcement");

                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.AnnouncementAttachments)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("attachments");
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).HasMaxLength(200);

                entity.Property(e => e.ConcurrencyStamp).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogins_Id");

                entity.Property(e => e.LoginProvider).HasMaxLength(100);

                entity.Property(e => e.ProviderKey).HasMaxLength(200);

                entity.Property(e => e.UserId).HasMaxLength(200);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_AspNetUserRoles_Id");

                entity.Property(e => e.UserId).HasMaxLength(200);

                entity.Property(e => e.RoleId).HasMaxLength(200);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                    .HasName("PK_AspNetUserTokens_Id");

                entity.Property(e => e.UserId).HasMaxLength(200);

                entity.Property(e => e.LoginProvider).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.HasIndex(e => e.UserType)
                    .HasName("fki_UserType_UserType");

                entity.Property(e => e.Id).HasMaxLength(200);

                entity.Property(e => e.ApprovedBy).HasMaxLength(200);

                entity.Property(e => e.AuthType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CertificateName).HasColumnName("Certificate_Name");

                entity.Property(e => e.CertificateThumbprint).HasColumnName("Certificate_Thumbprint");

                entity.Property(e => e.CertificateUniqueIdentifier).HasColumnName("Certificate_UniqueIdentifier");

                entity.Property(e => e.CheckedInChsiregister).HasColumnName("CheckedInCHSIRegister");

                entity.Property(e => e.Chsinumber)
                    .HasColumnName("CHSINumber")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(200);

                entity.Property(e => e.DeletedBy).HasMaxLength(200);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UpdatedBy).HasMaxLength(200);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.Property(e => e.UserType).HasMaxLength(20);

                entity.HasOne(d => d.ApprovedByNavigation)
                    .WithMany(p => p.InverseApprovedByNavigation)
                    .HasForeignKey(d => d.ApprovedBy)
                    .HasConstraintName("ApprovedBy_AspNetUsers");

                entity.HasOne(d => d.AuthTypeNavigation)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.AuthType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("aspnetuser_auth");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InverseCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("CreatedBy_AspNetUsers");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InverseDeletedByNavigation)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_DeletedBy_AspNetUsers");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InverseUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_UpdatedBy_AspNetUsers");

                entity.HasOne(d => d.UserTypeNavigation)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.UserType)
                    .HasConstraintName("usertype_usertype");
            });

            modelBuilder.Entity<AssetType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__AssetTyp__A25C5AA60AAB206F");

                entity.ToTable("AssetType", "N");

                entity.Property(e => e.Code).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.NameEn).HasMaxLength(500);
            });

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.Property(e => e.ContentType).HasMaxLength(100);

                entity.Property(e => e.FileName).HasMaxLength(500);

                entity.Property(e => e.FileType).HasMaxLength(100);
            });

            modelBuilder.Entity<Auction>(entity =>
            {
                entity.Property(e => e.BidStep).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.DeletedBy).HasMaxLength(200);

                entity.Property(e => e.EndPrice).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.StartPrice).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Winner).HasMaxLength(200);

                entity.HasOne(d => d.Announcement)
                    .WithMany(p => p.Auction)
                    .HasForeignKey(d => d.AnnouncementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Auction_AuctionAnnouncement");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AuctionCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Auction_AspNetUsers2");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AuctionDeletedByNavigation)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Auction_AspNetUsers1");

                entity.HasOne(d => d.WinnerNavigation)
                    .WithMany(p => p.AuctionWinnerNavigation)
                    .HasForeignKey(d => d.Winner)
                    .HasConstraintName("FK_Auction_AspNetUsers");
            });

            modelBuilder.Entity<AuctionAnnouncement>(entity =>
            {
                entity.Property(e => e.BidStep).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.DeletedBy).HasMaxLength(200);

                entity.Property(e => e.Order).HasMaxLength(50);

                entity.Property(e => e.PropertyType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartPrice).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.AuctionAnnouncement)
                    .HasForeignKey(d => d.AttachmentId)
                    .HasConstraintName("announcement_attachment");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AuctionAnnouncementCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuctionAnnouncement_AspNetUsers2");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AuctionAnnouncementDeletedByNavigation)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_AuctionAnnouncement_AspNetUsers1");

                entity.HasOne(d => d.OrderNavigation)
                    .WithMany(p => p.AuctionAnnouncement)
                    .HasForeignKey(d => d.Order)
                    .HasConstraintName("AuctionAnnouncement_Order");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AuctionAnnouncementUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuctionAnnouncement_AspNetUsers");
            });

            modelBuilder.Entity<AuctionBid>(entity =>
            {
                entity.Property(e => e.Bid).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.BidderId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Auction)
                    .WithMany(p => p.AuctionBid)
                    .HasForeignKey(d => d.AuctionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuctionBid_Auction");

                entity.HasOne(d => d.Bidder)
                    .WithMany(p => p.AuctionBid)
                    .HasForeignKey(d => d.BidderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuctionBid_AspNetUsers");

                entity.HasOne(d => d.NextBid)
                    .WithMany(p => p.InverseNextBid)
                    .HasForeignKey(d => d.NextBidId)
                    .HasConstraintName("FK_AuctionBid_AuctionBid");

                entity.HasOne(d => d.PreviousBid)
                    .WithMany(p => p.InversePreviousBid)
                    .HasForeignKey(d => d.PreviousBidId)
                    .HasConstraintName("FK_AuctionBid_AuctionBid1");
            });

            modelBuilder.Entity<AuctionItem>(entity =>
            {
                entity.HasIndex(e => e.AuctionId)
                    .HasName("fki_AuctionItem_Auction");

                entity.Property(e => e.CreatedBy).HasMaxLength(200);

                entity.Property(e => e.DeletedBy).HasMaxLength(200);

                entity.Property(e => e.Nrzid).HasColumnName("NRZId");

                entity.Property(e => e.PropertyType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.AgriculturalMachinary)
                    .WithMany(p => p.AuctionItem)
                    .HasForeignKey(d => d.AgriculturalMachinaryId)
                    .HasConstraintName("FK_AuctionItem_AgriculturalMachinery");

                entity.HasOne(d => d.Aircraft)
                    .WithMany(p => p.AuctionItem)
                    .HasForeignKey(d => d.AircraftId)
                    .HasConstraintName("FK_AuctionItem_Aircraft");

                entity.HasOne(d => d.Auction)
                    .WithMany(p => p.AuctionItem)
                    .HasForeignKey(d => d.AuctionId)
                    .HasConstraintName("auctionitem_auction");

                entity.HasOne(d => d.OtherProperty)
                    .WithMany(p => p.AuctionItem)
                    .HasForeignKey(d => d.OtherPropertyId)
                    .HasConstraintName("FK_AuctionItem_OtherProperty");

                entity.HasOne(d => d.PropertyTypeNavigation)
                    .WithMany(p => p.AuctionItem)
                    .HasForeignKey(d => d.PropertyType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AuctionItem_Type");

                entity.HasOne(d => d.RealEstate)
                    .WithMany(p => p.AuctionItem)
                    .HasForeignKey(d => d.RealEstateId)
                    .HasConstraintName("FK_AuctionItem_RealEstate");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.AuctionItem)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("FK_AuctionItem_Vehicle");
            });

            modelBuilder.Entity<AuctionLog>(entity =>
            {
                entity.Property(e => e.Text).IsRequired();

                entity.Property(e => e.UserId).HasMaxLength(200);

                entity.HasOne(d => d.Auction)
                    .WithMany(p => p.AuctionLog)
                    .HasForeignKey(d => d.AuctionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuctionLog_Auction");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AuctionLog)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AuctionLog_AspNetUsers");
            });

            modelBuilder.Entity<AuctionOrder>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_N_AuctionOrder_Code");

                entity.ToTable("AuctionOrder", "N");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.DeletedBy).HasMaxLength(200);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<AuctionOrderSettings>(entity =>
            {
                entity.Property(e => e.AuctionOrderCodeId).HasMaxLength(50);

                entity.HasOne(d => d.AuctionOrderCode)
                    .WithMany(p => p.AuctionOrderSettings)
                    .HasForeignKey(d => d.AuctionOrderCodeId)
                    .HasConstraintName("FK_AuctionSettings_AuctionOrder");
            });

            modelBuilder.Entity<AuctionRegistration>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.DeletedBy).HasMaxLength(200);

                entity.Property(e => e.ParticipantId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ProcessedBy).HasMaxLength(200);

                entity.Property(e => e.RepresentationType).HasMaxLength(50);

                entity.Property(e => e.RepresentedUserId).HasMaxLength(200);

                entity.Property(e => e.ResultDeliveryType).HasMaxLength(50);

                entity.Property(e => e.UniqueNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Announcement)
                    .WithMany(p => p.AuctionRegistration)
                    .HasForeignKey(d => d.AnnouncementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuctionRegistration_AuctionAnnouncement");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AuctionRegistrationCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuctionRegistration_AspNetUsers2");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AuctionRegistrationDeletedByNavigation)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_AuctionRegistration_AspNetUsers4");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.AuctionRegistrationParticipant)
                    .HasForeignKey(d => d.ParticipantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuctionRegistration_AspNetUsers");

                entity.HasOne(d => d.ProcessedByNavigation)
                    .WithMany(p => p.AuctionRegistrationProcessedByNavigation)
                    .HasForeignKey(d => d.ProcessedBy)
                    .HasConstraintName("FK_AuctionRegistration_AspNetUsers1");

                entity.HasOne(d => d.RepresentationTypeNavigation)
                    .WithMany(p => p.AuctionRegistration)
                    .HasForeignKey(d => d.RepresentationType)
                    .HasConstraintName("FK_AuctionRegistration_AuctionRepresentationType");

                entity.HasOne(d => d.RepresentedCompany)
                    .WithMany(p => p.AuctionRegistration)
                    .HasForeignKey(d => d.RepresentedCompanyId)
                    .HasConstraintName("FK_AuctionRegistration_Company");

                entity.HasOne(d => d.RepresentedPerson)
                    .WithMany(p => p.AuctionRegistration)
                    .HasForeignKey(d => d.RepresentedPersonId)
                    .HasConstraintName("FK_AuctionRegistration_Person");

                entity.HasOne(d => d.RepresentedUser)
                    .WithMany(p => p.AuctionRegistrationRepresentedUser)
                    .HasForeignKey(d => d.RepresentedUserId)
                    .HasConstraintName("FK_AuctionRegistration_AspNetUsers3");

                entity.HasOne(d => d.ResultDeliveryTypeNavigation)
                    .WithMany(p => p.AuctionRegistration)
                    .HasForeignKey(d => d.ResultDeliveryType)
                    .HasConstraintName("FK_AuctionRegistration_AuctionResultDeliveryType");
            });

            modelBuilder.Entity<AuctionRegistrationAttachment>(entity =>
            {
                entity.HasKey(e => new { e.AuctionRegistrationId, e.AttachmentId });

                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.AuctionRegistrationAttachment)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuctionRegistrationAttachment_Attachment");

                entity.HasOne(d => d.AuctionRegistration)
                    .WithMany(p => p.AuctionRegistrationAttachment)
                    .HasForeignKey(d => d.AuctionRegistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuctionRegistrationAttachment_AuctionRegistration");
            });

            modelBuilder.Entity<AuctionRepresentationType>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("AuctionRepresentationType", "N");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.NameEn).HasMaxLength(500);
            });

            modelBuilder.Entity<AuctionResultDeliveryType>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("AuctionResultDeliveryType", "N");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.NameEn).HasMaxLength(500);
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.ToTable("Cities", "N");

                entity.HasIndex(e => e.MunicipalityId)
                    .HasName("fki_Cities_Municipalities");

                entity.Property(e => e.DisadvantagedCode).HasMaxLength(10);

                entity.Property(e => e.Ekatte)
                    .HasColumnName("EKATTE")
                    .HasMaxLength(10);

                entity.Property(e => e.Latitude).HasMaxLength(15);

                entity.Property(e => e.Longitude).HasMaxLength(15);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PointX)
                    .HasColumnName("Point_X")
                    .HasMaxLength(15);

                entity.Property(e => e.PointY)
                    .HasColumnName("Point_Y")
                    .HasMaxLength(15);

                entity.Property(e => e.PostCode).HasMaxLength(10);

                entity.Property(e => e.Prefix).HasMaxLength(10);

                entity.HasOne(d => d.Municipality)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.MunicipalityId)
                    .HasConstraintName("cities_municipalities");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanyCaseNumber).HasMaxLength(500);

                entity.Property(e => e.Eik)
                    .IsRequired()
                    .HasColumnName("EIK")
                    .HasMaxLength(13);

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Company)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("company_address");
            });

            modelBuilder.Entity<Counter>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Counter", "HangFire");

                entity.HasIndex(e => e.Key)
                    .HasName("CX_HangFire_Counter")
                    .IsClustered();

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Distraint>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(200);

                entity.Property(e => e.EnforcedBy).HasMaxLength(200);

                entity.Property(e => e.ExemptedBy).HasMaxLength(200);

                entity.Property(e => e.PropertyTypeCode).HasMaxLength(50);

                entity.Property(e => e.RevokedBy).HasMaxLength(200);

                entity.Property(e => e.StatusCode).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(200);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DistraintCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("CreatedBy_fkey");

                entity.HasOne(d => d.DebtorCompany)
                    .WithMany(p => p.DistraintDebtorCompany)
                    .HasForeignKey(d => d.DebtorCompanyId)
                    .HasConstraintName("FK_Distraint_RegixCompany_Debtor");

                entity.HasOne(d => d.DebtorPerson)
                    .WithMany(p => p.DistraintDebtorPerson)
                    .HasForeignKey(d => d.DebtorPersonId)
                    .HasConstraintName("FK_Distraint_RegixPerson_Debtor");

                entity.HasOne(d => d.EnforcedByNavigation)
                    .WithMany(p => p.DistraintEnforcedByNavigation)
                    .HasForeignKey(d => d.EnforcedBy)
                    .HasConstraintName("FK_Distraint_AspUser_EnforcedBy");

                entity.HasOne(d => d.ExemptedByNavigation)
                    .WithMany(p => p.DistraintExemptedByNavigation)
                    .HasForeignKey(d => d.ExemptedBy)
                    .HasConstraintName("FK_Distraint_AspUser_ExemptedBy");

                entity.HasOne(d => d.InFavourOfCompany)
                    .WithMany(p => p.DistraintInFavourOfCompany)
                    .HasForeignKey(d => d.InFavourOfCompanyId)
                    .HasConstraintName("FK_Distraint_RegixCompany_InFavourOf");

                entity.HasOne(d => d.InFavourOfPerson)
                    .WithMany(p => p.DistraintInFavourOfPerson)
                    .HasForeignKey(d => d.InFavourOfPersonId)
                    .HasConstraintName("infavourofperson_fkey");

                entity.HasOne(d => d.PropertyIdAgriForMachineryNavigation)
                    .WithMany(p => p.Distraint)
                    .HasForeignKey(d => d.PropertyIdAgriForMachinery)
                    .HasConstraintName("FK_Distraint_Agriculture_PropertyId");

                entity.HasOne(d => d.PropertyIdAircraftNavigation)
                    .WithMany(p => p.Distraint)
                    .HasForeignKey(d => d.PropertyIdAircraft)
                    .HasConstraintName("aircraft_fkey3");

                entity.HasOne(d => d.PropertyIdOtherPropertyNavigation)
                    .WithMany(p => p.Distraint)
                    .HasForeignKey(d => d.PropertyIdOtherProperty)
                    .HasConstraintName("otherproperty_fkey");

                entity.HasOne(d => d.PropertyIdVehicleNavigation)
                    .WithMany(p => p.Distraint)
                    .HasForeignKey(d => d.PropertyIdVehicle)
                    .HasConstraintName("vehicle_fkey");

                entity.HasOne(d => d.PropertyIdVesselNavigation)
                    .WithMany(p => p.Distraint)
                    .HasForeignKey(d => d.PropertyIdVessel)
                    .HasConstraintName("FK_Distraint_Vessel_Id");

                entity.HasOne(d => d.PropertyTypeCodeNavigation)
                    .WithMany(p => p.Distraint)
                    .HasForeignKey(d => d.PropertyTypeCode)
                    .HasConstraintName("PropertyType_fkey");

                entity.HasOne(d => d.RevokedByNavigation)
                    .WithMany(p => p.DistraintRevokedByNavigation)
                    .HasForeignKey(d => d.RevokedBy)
                    .HasConstraintName("FK_Distraint_AspUser_RevokedBy");

                entity.HasOne(d => d.StatusCodeNavigation)
                    .WithMany(p => p.Distraint)
                    .HasForeignKey(d => d.StatusCode)
                    .HasConstraintName("Status_fkey");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.DistraintUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("UpdatedBy_fkey");
            });

            modelBuilder.Entity<DistraintStatus>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_N_DistraintStatus_Code");

                entity.ToTable("DistraintStatus", "N");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Deactivated).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<EservicePaymentRequest>(entity =>
            {
                entity.ToTable("EServicePaymentRequest");

                entity.Property(e => e.EserviceTypeCode)
                    .IsRequired()
                    .HasColumnName("EServiceTypeCode")
                    .HasMaxLength(100);

                entity.Property(e => e.StatusCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.EserviceTypeCodeNavigation)
                    .WithMany(p => p.EservicePaymentRequest)
                    .HasForeignKey(d => d.EserviceTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EServicePaymentRequest_EServiceType_Code");

                entity.HasOne(d => d.PaymentRequest)
                    .WithMany(p => p.EservicePaymentRequest)
                    .HasForeignKey(d => d.PaymentRequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EServicePaymentRequest_PaymentRequest_RequestId");

                entity.HasOne(d => d.SeizedPropertyCertificateRequest)
                    .WithMany(p => p.EservicePaymentRequest)
                    .HasForeignKey(d => d.SeizedPropertyCertificateRequestId)
                    .HasConstraintName("FK_EServicePaymentRequest_CertificateRequestId_RequestId");

                entity.HasOne(d => d.SeizedPropertyReportRequest)
                    .WithMany(p => p.EservicePaymentRequest)
                    .HasForeignKey(d => d.SeizedPropertyReportRequestId)
                    .HasConstraintName("FK_EServicePaymentRequest_ReportRequestId_RequestId");

                entity.HasOne(d => d.StatusCodeNavigation)
                    .WithMany(p => p.EservicePaymentRequest)
                    .HasForeignKey(d => d.StatusCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EServicePaymentRequest_RequestStatus_Code");
            });

            modelBuilder.Entity<EservicePaymentRequestStatusHistory>(entity =>
            {
                entity.ToTable("EServicePaymentRequestStatusHistory");

                entity.Property(e => e.EserviceTime).HasColumnName("EServiceTime");

                entity.Property(e => e.StatusCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.EservicePaymentRequestStatusHistory)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServicePaymentRequest_StatusHistory_Id");

                entity.HasOne(d => d.StatusCodeNavigation)
                    .WithMany(p => p.EservicePaymentRequestStatusHistory)
                    .HasForeignKey(d => d.StatusCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServicePaymentStatusHistory_Status_StatusCode");
            });

            modelBuilder.Entity<EserviceType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_N_EServiceType");

                entity.ToTable("EServiceType", "N");

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.NameEn).IsRequired();
            });

            modelBuilder.Entity<EservicesSettings>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EServicesSettings");

                entity.Property(e => e.AdministrativeServiceNotificationUrl).HasColumnName("AdministrativeServiceNotificationURL");

                entity.Property(e => e.Currency).HasMaxLength(5);

                entity.Property(e => e.SeizedPropertyByOwnerReportFee).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SeizedPropertyCertificateFee).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SeizedPropertyCertificateReason).HasMaxLength(200);

                entity.Property(e => e.SeizedPropertyReportReason).HasMaxLength(200);

                entity.Property(e => e.ServiceProviderBic)
                    .HasColumnName("ServiceProviderBIC")
                    .HasMaxLength(20);

                entity.Property(e => e.ServiceProviderIban)
                    .HasColumnName("ServiceProviderIBAN")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Hash>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.Field })
                    .HasName("PK_HangFire_Hash");

                entity.ToTable("Hash", "HangFire");

                entity.HasIndex(e => e.ExpireAt)
                    .HasName("IX_HangFire_Hash_ExpireAt")
                    .HasFilter("([ExpireAt] IS NOT NULL)");

                entity.Property(e => e.Key).HasMaxLength(100);

                entity.Property(e => e.Field).HasMaxLength(100);
            });

            modelBuilder.Entity<IdentificationType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("IdentificationType_pkey");

                entity.ToTable("IdentificationType", "N");

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.Deactivated)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("Job", "HangFire");

                entity.HasIndex(e => e.StateName)
                    .HasName("IX_HangFire_Job_StateName")
                    .HasFilter("([StateName] IS NOT NULL)");

                entity.HasIndex(e => new { e.StateName, e.ExpireAt })
                    .HasName("IX_HangFire_Job_ExpireAt")
                    .HasFilter("([ExpireAt] IS NOT NULL)");

                entity.Property(e => e.Arguments).IsRequired();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");

                entity.Property(e => e.InvocationData).IsRequired();

                entity.Property(e => e.StateName).HasMaxLength(20);
            });

            modelBuilder.Entity<JobParameter>(entity =>
            {
                entity.HasKey(e => new { e.JobId, e.Name })
                    .HasName("PK_HangFire_JobParameter");

                entity.ToTable("JobParameter", "HangFire");

                entity.Property(e => e.Name).HasMaxLength(40);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobParameter)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_HangFire_JobParameter_Job");
            });

            modelBuilder.Entity<JobQueue>(entity =>
            {
                entity.HasKey(e => new { e.Queue, e.Id })
                    .HasName("PK_HangFire_JobQueue");

                entity.ToTable("JobQueue", "HangFire");

                entity.Property(e => e.Queue).HasMaxLength(50);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.FetchedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<List>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.Id })
                    .HasName("PK_HangFire_List");

                entity.ToTable("List", "HangFire");

                entity.HasIndex(e => e.ExpireAt)
                    .HasName("IX_HangFire_List_ExpireAt")
                    .HasFilter("([ExpireAt] IS NOT NULL)");

                entity.Property(e => e.Key).HasMaxLength(100);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Municipalities>(entity =>
            {
                entity.ToTable("Municipalities", "N");

                entity.HasIndex(e => e.RegionId)
                    .HasName("fki_Municipality_Region");

                entity.Property(e => e.Code).HasMaxLength(5);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Municipalities)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("municipality_region");
            });

            modelBuilder.Entity<OtherProperty>(entity =>
            {
                entity.Property(e => e.Identifier).IsRequired();

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.OtherProperty)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OtherProperty_AssetType");
            });

            modelBuilder.Entity<PaymentRequest>(entity =>
            {
                entity.HasKey(e => e.AisPaymentId);

                entity.Property(e => e.AdministrativeServiceNotificationUrl).HasColumnName("AdministrativeServiceNotificationURL");

                entity.Property(e => e.ApplicantName).IsRequired();

                entity.Property(e => e.ApplicantUin)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PaymentReason)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PaymentReferenceDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentReferenceNumber)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PaymentReferenceType).HasMaxLength(100);

                entity.Property(e => e.PaymentTypeCode).HasMaxLength(50);

                entity.Property(e => e.ServiceProviderBank).IsRequired();

                entity.Property(e => e.ServiceProviderBic)
                    .IsRequired()
                    .HasColumnName("ServiceProviderBIC")
                    .HasMaxLength(20);

                entity.Property(e => e.ServiceProviderIban)
                    .IsRequired()
                    .HasColumnName("ServiceProviderIBAN")
                    .HasMaxLength(50);

                entity.Property(e => e.ServiceProviderName).IsRequired();
            });

            modelBuilder.Entity<PaymentRequestStatus>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_N_PaymentRequestStatus");

                entity.ToTable("PaymentRequestStatus", "N");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.NameEn).IsRequired();
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(200);

                entity.Property(e => e.CreatedOn).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(200);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IdentificationNumber).HasMaxLength(15);

                entity.Property(e => e.IdentificationNumberType).HasMaxLength(10);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(200);

                entity.Property(e => e.UserId).HasMaxLength(200);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Person)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("fk_address");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.PersonCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("CreatedBy_Person");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.PersonDeletedByNavigation)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("DeletedBy_Person");

                entity.HasOne(d => d.IdentificationNumberTypeNavigation)
                    .WithMany(p => p.Person)
                    .HasForeignKey(d => d.IdentificationNumberType)
                    .HasConstraintName("FK_Person_IdentificationType");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.PersonUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("UpdatedBy_Person");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PersonUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Person_AspNetUsers");
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.Property(e => e.Area).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Floor).HasMaxLength(5);

                entity.Property(e => e.Identifier).HasMaxLength(255);

                entity.Property(e => e.IdentifierType).HasMaxLength(255);

                entity.Property(e => e.PropertyConstructionTypeId).HasMaxLength(50);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(200);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Property)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Property_Address");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.PropertyCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Property_AspNetUsers_Creator");

                entity.HasOne(d => d.PropertyConstructionType)
                    .WithMany(p => p.Property)
                    .HasForeignKey(d => d.PropertyConstructionTypeId)
                    .HasConstraintName("FK_Property_PropertyConstructionType");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Property)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Property_RealEstateType");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.PropertyUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Property_AspNetUsers_Updater");
            });

            modelBuilder.Entity<PropertyConstructionType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__Property__A25C5AA6DEB537F3");

                entity.ToTable("PropertyConstructionType", "N");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.NameEn).HasMaxLength(500);
            });

            modelBuilder.Entity<PropertyExtra>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__Property__A25C5AA6EE0C7F10");

                entity.ToTable("PropertyExtra", "N");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.NameEn).HasMaxLength(500);
            });

            modelBuilder.Entity<PropertyType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_N_PropertyType_Code");

                entity.ToTable("PropertyType", "N");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<RealEstateType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__RealEsta__A25C5AA6A5233392");

                entity.ToTable("RealEstateType", "N");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.NameEn).HasMaxLength(500);
            });

            modelBuilder.Entity<RegiXreport>(entity =>
            {
                entity.ToTable("RegiXReport");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AdapterSubdirectory).IsRequired();

                entity.Property(e => e.ReportName).IsRequired();
            });

            modelBuilder.Entity<RegiXreportToPropertyType>(entity =>
            {
                entity.ToTable("RegiXReportToPropertyType");

                entity.Property(e => e.PropertyTypeCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RegiXreportId).HasColumnName("RegiXReportId");

                entity.Property(e => e.RegiXsearchCriteriaTypeCode)
                    .IsRequired()
                    .HasColumnName("RegiXSearchCriteriaTypeCode")
                    .HasMaxLength(50);

                entity.HasOne(d => d.PropertyTypeCodeNavigation)
                    .WithMany(p => p.RegiXreportToPropertyType)
                    .HasForeignKey(d => d.PropertyTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Report_PropertyType_fkey");

                entity.HasOne(d => d.RegiXreport)
                    .WithMany(p => p.RegiXreportToPropertyType)
                    .HasForeignKey(d => d.RegiXreportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reportid_fkey");

                entity.HasOne(d => d.RegiXsearchCriteriaTypeCodeNavigation)
                    .WithMany(p => p.RegiXreportToPropertyType)
                    .HasForeignKey(d => d.RegiXsearchCriteriaTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SearchCriteria_fkey");
            });

            modelBuilder.Entity<RegiXrequest>(entity =>
            {
                entity.ToTable("RegiXRequest");

                entity.Property(e => e.Argument).IsRequired();

                entity.Property(e => e.RegiXreportId).HasColumnName("RegiXReportId");

                entity.Property(e => e.UserId).HasMaxLength(200);

                entity.HasOne(d => d.RegiXreport)
                    .WithMany(p => p.RegiXrequest)
                    .HasForeignKey(d => d.RegiXreportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("report_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RegiXrequest)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_RegiXRequest_User_Id");
            });

            modelBuilder.Entity<RegiXresponse>(entity =>
            {
                entity.HasKey(e => e.RequestId)
                    .HasName("RegiXResponse_pkey");

                entity.ToTable("RegiXResponse");

                entity.Property(e => e.RequestId).ValueGeneratedNever();

                entity.Property(e => e.RawContent).IsRequired();

                entity.HasOne(d => d.Request)
                    .WithOne(p => p.RegiXresponse)
                    .HasForeignKey<RegiXresponse>(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("regixresponse_fkey");
            });

            modelBuilder.Entity<RegiXsearchCriteriaType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_N_RegiXSearchCriteriaType_Code");

                entity.ToTable("RegiXSearchCriteriaType", "N");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.ElementName).IsRequired();
            });

            modelBuilder.Entity<Regions>(entity =>
            {
                entity.ToTable("Regions", "N");

                entity.Property(e => e.Code).HasMaxLength(2);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Nuts3Code).HasMaxLength(5);
            });

            modelBuilder.Entity<RegixCompany>(entity =>
            {
                entity.Property(e => e.LegalFormAbbr).HasMaxLength(50);

                entity.Property(e => e.LegalFormName).HasMaxLength(250);

                entity.Property(e => e.StatusCode).HasMaxLength(50);

                entity.Property(e => e.Uic)
                    .IsRequired()
                    .HasColumnName("UIC")
                    .HasMaxLength(200);

                entity.HasOne(d => d.StatusCodeNavigation)
                    .WithMany(p => p.RegixCompany)
                    .HasForeignKey(d => d.StatusCode)
                    .HasConstraintName("RegixCompany_Status_fkey");
            });

            modelBuilder.Entity<RegixCompanyStatus>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_N_RegixCompanyStatus_Code");

                entity.ToTable("RegixCompanyStatus", "N");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<RegixPerson>(entity =>
            {
                entity.Property(e => e.Identifier).IsRequired();
            });

            modelBuilder.Entity<RequestForCertificateOfDistraintOfProperty>(entity =>
            {
                entity.Property(e => e.CompanyCaseNumber).HasMaxLength(15);

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.IdentifierOfLegalEntity).HasMaxLength(15);

                entity.Property(e => e.IsPersonalIdentifierTypeLnch).HasColumnName("IsPersonalIdentifierTypeLNCh");

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.MiddleName).HasMaxLength(100);

                entity.Property(e => e.NameOfLegalEntity).HasMaxLength(100);

                entity.Property(e => e.PersonalIdentifier).HasMaxLength(15);

                entity.Property(e => e.PropertyTypeCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserTypeCode).HasMaxLength(15);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.RequestForCertificateOfDistraintOfPropertyCity)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("city_fkey");

                entity.HasOne(d => d.CityIdOfLegalEntityNavigation)
                    .WithMany(p => p.RequestForCertificateOfDistraintOfPropertyCityIdOfLegalEntityNavigation)
                    .HasForeignKey(d => d.CityIdOfLegalEntity)
                    .HasConstraintName("cityoflegalentity_fkey");

                entity.HasOne(d => d.Distraint)
                    .WithMany(p => p.RequestForCertificateOfDistraintOfProperty)
                    .HasForeignKey(d => d.DistraintId)
                    .HasConstraintName("distraint_fkey");

                entity.HasOne(d => d.Municipality)
                    .WithMany(p => p.RequestForCertificateOfDistraintOfPropertyMunicipality)
                    .HasForeignKey(d => d.MunicipalityId)
                    .HasConstraintName("municipality_fkey");

                entity.HasOne(d => d.MunicipalityIdOfLegalEntityNavigation)
                    .WithMany(p => p.RequestForCertificateOfDistraintOfPropertyMunicipalityIdOfLegalEntityNavigation)
                    .HasForeignKey(d => d.MunicipalityIdOfLegalEntity)
                    .HasConstraintName("municipalityoflegalentity_fkey");

                entity.HasOne(d => d.PropertyIdAircraftNavigation)
                    .WithMany(p => p.RequestForCertificateOfDistraintOfProperty)
                    .HasForeignKey(d => d.PropertyIdAircraft)
                    .HasConstraintName("aircraft_fkey4");

                entity.HasOne(d => d.PropertyIdOtherPropertyNavigation)
                    .WithMany(p => p.RequestForCertificateOfDistraintOfProperty)
                    .HasForeignKey(d => d.PropertyIdOtherProperty)
                    .HasConstraintName("otherproperty_fkey1");

                entity.HasOne(d => d.PropertyIdVehicleNavigation)
                    .WithMany(p => p.RequestForCertificateOfDistraintOfProperty)
                    .HasForeignKey(d => d.PropertyIdVehicle)
                    .HasConstraintName("vehicle_fkey1");

                entity.HasOne(d => d.PropertyTypeCodeNavigation)
                    .WithMany(p => p.RequestForCertificateOfDistraintOfProperty)
                    .HasForeignKey(d => d.PropertyTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestForCertificateOfDistraintOfProperty_PropertyType");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.RequestForCertificateOfDistraintOfPropertyRegion)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("regions_fkey");

                entity.HasOne(d => d.RegionIdOfLegalEntityNavigation)
                    .WithMany(p => p.RequestForCertificateOfDistraintOfPropertyRegionIdOfLegalEntityNavigation)
                    .HasForeignKey(d => d.RegionIdOfLegalEntity)
                    .HasConstraintName("regionoflegalentity_fkey");
            });

            modelBuilder.Entity<RequesterType>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("RequesterType", "N");

                entity.Property(e => e.Code).HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Sort).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<Schema>(entity =>
            {
                entity.HasKey(e => e.Version)
                    .HasName("PK_HangFire_Schema");

                entity.ToTable("Schema", "HangFire");

                entity.Property(e => e.Version).ValueGeneratedNever();
            });

            modelBuilder.Entity<SeizedPropertyAvailabilityRequest>(entity =>
            {
                entity.HasIndex(e => e.CheckedPersonId)
                    .HasName("fki_FK_SeizedPropertyAvailabilityRequest_CheckedPerson");

                entity.HasIndex(e => e.RequesterCompanyId)
                    .HasName("fki_FK_SeizedPropertyAvailabilityRequest_RequesterCompany");

                entity.HasIndex(e => e.RequestorPersonId)
                    .HasName("fki_FK_SeizedPropertyAvailabilityRequest_RequesterPerson");

                entity.Property(e => e.Date).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InTheQualityOfPersonTypeCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.RequesterCompanyCaseNumber)
                    .HasColumnName("RequesterCompanyCaseNUmber")
                    .HasMaxLength(150);

                entity.Property(e => e.RequesterCompanyEik).HasMaxLength(20);

                entity.Property(e => e.RequesterCompanyRepresentative).HasMaxLength(150);

                entity.Property(e => e.RequestorUserId).HasMaxLength(200);

                entity.HasOne(d => d.CheckedCompany)
                    .WithMany(p => p.SeizedPropertyAvailabilityRequestCheckedCompany)
                    .HasForeignKey(d => d.CheckedCompanyId)
                    .HasConstraintName("FK_SeizedPropertyAvailabilityRequest_Company");

                entity.HasOne(d => d.CheckedPerson)
                    .WithMany(p => p.SeizedPropertyAvailabilityRequestCheckedPerson)
                    .HasForeignKey(d => d.CheckedPersonId)
                    .HasConstraintName("fk_seizedpropertyavailabilityrequest_checkedperson");

                entity.HasOne(d => d.InTheQualityOfPersonTypeCodeNavigation)
                    .WithMany(p => p.SeizedPropertyAvailabilityRequest)
                    .HasForeignKey(d => d.InTheQualityOfPersonTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeizedPropertyAvailabilityRequest_RequesterType");

                entity.HasOne(d => d.RequesterCompany)
                    .WithMany(p => p.SeizedPropertyAvailabilityRequestRequesterCompany)
                    .HasForeignKey(d => d.RequesterCompanyId)
                    .HasConstraintName("fk_seizedpropertyavailabilityrequest_requestercompany");

                entity.HasOne(d => d.RequestorPerson)
                    .WithMany(p => p.SeizedPropertyAvailabilityRequestRequestorPerson)
                    .HasForeignKey(d => d.RequestorPersonId)
                    .HasConstraintName("fk_seizedpropertyavailabilityrequest_requesterperson");

                entity.HasOne(d => d.RequestorUser)
                    .WithMany(p => p.SeizedPropertyAvailabilityRequest)
                    .HasForeignKey(d => d.RequestorUserId)
                    .HasConstraintName("FK_SeizedPropertyAvailabilityRequest_AspNetUsers");
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.ToTable("Server", "HangFire");

                entity.HasIndex(e => e.LastHeartbeat)
                    .HasName("IX_HangFire_Server_LastHeartbeat");

                entity.Property(e => e.Id).HasMaxLength(200);

                entity.Property(e => e.LastHeartbeat).HasColumnType("datetime");
            });

            modelBuilder.Entity<Set>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.Value })
                    .HasName("PK_HangFire_Set");

                entity.ToTable("Set", "HangFire");

                entity.HasIndex(e => e.ExpireAt)
                    .HasName("IX_HangFire_Set_ExpireAt")
                    .HasFilter("([ExpireAt] IS NOT NULL)");

                entity.HasIndex(e => new { e.Key, e.Score })
                    .HasName("IX_HangFire_Set_Score");

                entity.Property(e => e.Key).HasMaxLength(100);

                entity.Property(e => e.Value).HasMaxLength(256);

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.HasKey(e => new { e.JobId, e.Id })
                    .HasName("PK_HangFire_State");

                entity.ToTable("State", "HangFire");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Reason).HasMaxLength(100);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.State)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_HangFire_State_Job");
            });

            modelBuilder.Entity<UserLogs>(entity =>
            {
                entity.Property(e => e.ActionName).HasMaxLength(100);

                entity.Property(e => e.Controller).HasMaxLength(100);

                entity.Property(e => e.Ip).HasMaxLength(128);

                entity.Property(e => e.RequestMethod).HasMaxLength(50);

                entity.Property(e => e.Result).HasMaxLength(500);

                entity.Property(e => e.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<UserRegisterType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("UserRegisterType_pkey");

                entity.ToTable("UserRegisterType", "N");

                entity.Property(e => e.Code).HasMaxLength(20);

                entity.Property(e => e.IsPublic)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("UserType_pkey");

                entity.ToTable("UserType", "N");

                entity.Property(e => e.Code).HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.Property(e => e.RegistrationNumber).IsRequired();

                entity.Property(e => e.VehicleNumOfAxles).HasColumnName("VehicleNumOfAxles ");

                entity.Property(e => e.Vin)
                    .IsRequired()
                    .HasColumnName("VIN");
            });

            modelBuilder.Entity<VehicleExtension>(entity =>
            {
                entity.HasKey(e => e.VehicleId)
                    .HasName("VehicleExtension_pkey");

                entity.Property(e => e.VehicleId).ValueGeneratedNever();

                entity.HasOne(d => d.Vehicle)
                    .WithOne(p => p.VehicleExtension)
                    .HasForeignKey<VehicleExtension>(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("vehicle_fkey2");
            });

            modelBuilder.Entity<VehicleOwner>(entity =>
            {
                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.VehicleOwner)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("vehicle+fkey");
            });

            modelBuilder.Entity<VehicleUser>(entity =>
            {
                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.VehicleUser)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("vehicle_fkey3");
            });

            modelBuilder.Entity<Vessel>(entity =>
            {
                entity.Property(e => e.Bt)
                    .HasColumnName("BT")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Deadweight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LengthBetweenPerpendiculars).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MaxLength).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MaxWidth).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Nt)
                    .HasColumnName("NT")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ShipboardHeight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SumEnginePower).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Waterplane).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<VesselEngine>(entity =>
            {
                entity.HasOne(d => d.Vessel)
                    .WithMany(p => p.VesselEngine)
                    .HasForeignKey(d => d.VesselId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VesselEngine_Vessel_Id");
            });

            modelBuilder.Entity<VesselExtension>(entity =>
            {
                entity.HasKey(e => e.VesselId);

                entity.Property(e => e.VesselId).ValueGeneratedNever();

                entity.HasOne(d => d.Vessel)
                    .WithOne(p => p.VesselExtension)
                    .HasForeignKey<VesselExtension>(d => d.VesselId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VesselExtension_Vessel_Id");
            });

            modelBuilder.Entity<VesselOwner>(entity =>
            {
                entity.Property(e => e.Egn)
                    .HasColumnName("EGN")
                    .HasMaxLength(10);

                entity.Property(e => e.Eik)
                    .HasColumnName("EIK")
                    .HasMaxLength(20);

                entity.Property(e => e.PersonFirstName).HasMaxLength(200);

                entity.Property(e => e.PersonLastName).HasMaxLength(200);

                entity.Property(e => e.PersonMiddleName).HasMaxLength(200);

                entity.HasOne(d => d.Vessel)
                    .WithMany(p => p.VesselOwner)
                    .HasForeignKey(d => d.VesselId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VesselOwner_Vessel_Id");
            });

            modelBuilder.Entity<VesselRegistrationData>(entity =>
            {
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.VesselRegistrationData)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK_VesselRegistration_Status_Status");

                entity.HasOne(d => d.Vessel)
                    .WithMany(p => p.VesselRegistrationData)
                    .HasForeignKey(d => d.VesselId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VesselRegistrationData_Vessel_Id");
            });

            modelBuilder.Entity<VesselStatus>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_N_VesselStatus_Code");

                entity.ToTable("VesselStatus", "N");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Name).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
