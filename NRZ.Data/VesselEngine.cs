using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class VesselEngine
    {
        public int Id { get; set; }
        public int VesselId { get; set; }
        public string SystemModification { get; set; }
        public string EngineNumber { get; set; }
        public int? Power { get; set; }
        public string Type { get; set; }

        public virtual Vessel Vessel { get; set; }
    }
}
