using NRZ.Models.Role;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.User
{
    public class AspNetUserModel
    {
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
        public int? AccessFailedCount { get; set; }
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
        public int? Chsinumber { get; set; }
        public bool? CheckedInChsiregister { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public ICollection<AspNetRoleModel> Roles { get; set; }
    }
}
