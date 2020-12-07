using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class Vessel
    {
        public Vessel()
        {
            Distraint = new HashSet<Distraint>();
            VesselEngine = new HashSet<VesselEngine>();
            VesselOwner = new HashSet<VesselOwner>();
            VesselRegistrationData = new HashSet<VesselRegistrationData>();
        }

        public int Id { get; set; }
        public decimal? Bt { get; set; }
        public decimal? Nt { get; set; }
        public decimal? MaxLength { get; set; }
        public decimal? LengthBetweenPerpendiculars { get; set; }
        public decimal? MaxWidth { get; set; }
        public decimal? Waterplane { get; set; }
        public decimal? ShipboardHeight { get; set; }
        public decimal? Deadweight { get; set; }
        public int? NumberOfEngines { get; set; }
        public string EnginesFuel { get; set; }
        public decimal? SumEnginePower { get; set; }
        public string BodyNumber { get; set; }

        public virtual VesselExtension VesselExtension { get; set; }
        public virtual ICollection<Distraint> Distraint { get; set; }
        public virtual ICollection<VesselEngine> VesselEngine { get; set; }
        public virtual ICollection<VesselOwner> VesselOwner { get; set; }
        public virtual ICollection<VesselRegistrationData> VesselRegistrationData { get; set; }
    }
}
