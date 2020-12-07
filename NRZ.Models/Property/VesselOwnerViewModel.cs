using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Property
{
    public class VesselOwnerViewModel
    {
        public int Id { get; set; }
        public int VesselId { get; set; }
        public bool? IsCompany { get; set; }
        public string CompanyName { get; set; }
        public string Eik { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonMiddleName { get; set; }
        public string PersonLastName { get; set; }
        public string Egn { get; set; }
        public string ImoNumber { get; set; }
        public bool? IsUser { get; set; }
        public string Address { get; set; }
    }
}
