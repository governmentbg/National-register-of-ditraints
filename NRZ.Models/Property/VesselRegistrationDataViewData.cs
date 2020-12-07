using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Property
{
    public class VesselRegistrationDataViewData
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
        public string StatusName { get; set; }
        public string StatusNameEn { get; set; }
    }
}
