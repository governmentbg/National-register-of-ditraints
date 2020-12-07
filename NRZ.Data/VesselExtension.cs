using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class VesselExtension
    {
        public int VesselId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? RequestId { get; set; }
        public bool Deactivated { get; set; }

        public virtual Vessel Vessel { get; set; }
    }
}
