using Microsoft.AspNetCore.Identity;
using System;

namespace NRZ.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public byte[] Certificate { get; set; }
        public string Certificate_Thumbprint { get; set; }
        public string Certificate_Name { get; set; }
        public string Certificate_UniqueIdentifier { get; set; }
        public bool Deleted { get; set; }
        public bool ConfirmedByAdmin { get; set; }
        public string AuthType { get; set; }
        public string UserType { get; set; }
        public string CHSINumber { get; set; }
        public bool? CheckedInCHSIRegister { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string CreatedBy { get; set; }
        public string DeletedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
