using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class DistraintStatus
    {
        public DistraintStatus()
        {
            Distraint = new HashSet<Distraint>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool? Deactivated { get; set; }

        public virtual ICollection<Distraint> Distraint { get; set; }
    }
}
