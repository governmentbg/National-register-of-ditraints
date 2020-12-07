using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class VesselRegistrationData
    {
        public int Id { get; set; }
        public int VesselId { get; set; }
        public string ShipName { get; set; }
        public string ShipNameLatin { get; set; }
        public string RegistrationPort { get; set; }
        public string RegistrationNumber { get; set; }
        public string Tom { get; set; }
        public string Page { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        public virtual VesselStatus StatusNavigation { get; set; }
        public virtual Vessel Vessel { get; set; }
    }
}
