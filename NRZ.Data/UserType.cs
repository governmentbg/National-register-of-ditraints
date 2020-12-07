using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class UserType
    {
        public UserType()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool Deactivated { get; set; }
        public short Sort { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
