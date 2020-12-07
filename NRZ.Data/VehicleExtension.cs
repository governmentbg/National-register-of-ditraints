using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class VehicleExtension
    {
        public long VehicleId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? RequestId { get; set; }
        public bool Deactivated { get; set; }

        public virtual Vehicle Vehicle { get; set; }
    }
}
