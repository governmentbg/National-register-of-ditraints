using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AircraftExtension
    {
        public int AircraftId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? RequestId { get; set; }
        public bool Deactivated { get; set; }

        public virtual Aircraft Aircraft { get; set; }
    }
}
