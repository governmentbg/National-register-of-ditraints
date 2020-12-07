using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Property
{
    public class VesselEngineViewModel
    {
        public int Id { get; set; }
        public int VesselId { get; set; }
        public string SystemModification { get; set; }
        public string EngineNumber { get; set; }
        public int? Power { get; set; }
        public string Type { get; set; }
    }
}
