using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class IdentificationType
    {
        public IdentificationType()
        {
            Person = new HashSet<Person>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public short Sort { get; set; }
        public bool? Deactivated { get; set; }

        public virtual ICollection<Person> Person { get; set; }
    }
}
