using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class RequesterType
    {
        public RequesterType()
        {
            SeizedPropertyAvailabilityRequest = new HashSet<SeizedPropertyAvailabilityRequest>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool Deactivated { get; set; }
        public DateTime Sort { get; set; }

        public virtual ICollection<SeizedPropertyAvailabilityRequest> SeizedPropertyAvailabilityRequest { get; set; }
    }
}
