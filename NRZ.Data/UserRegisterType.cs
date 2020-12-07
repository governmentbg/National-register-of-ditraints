using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class UserRegisterType
    {
        public UserRegisterType()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool Disabled { get; set; }
        public short Sort { get; set; }
        public bool? IsPublic { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
