using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class VesselStatus
    {
        public VesselStatus()
        {
            VesselRegistrationData = new HashSet<VesselRegistrationData>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool Deactivated { get; set; }

        public virtual ICollection<VesselRegistrationData> VesselRegistrationData { get; set; }
    }
}
