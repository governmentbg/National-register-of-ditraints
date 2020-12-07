using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Identity
{
    public class ApplicationUserRoles:ApplicationUser
    {
        public string UserId { get; set; }

        public  IEnumerable<ApplicationRole> Roles { get; set; }
    }
}
