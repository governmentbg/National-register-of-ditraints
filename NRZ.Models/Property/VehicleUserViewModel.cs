﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Property
{
    public class VehicleUserViewModel
    {
        public long Id { get; set; }
        public long VehicleId { get; set; }
        public string BulgarianCitizenPin { get; set; }
        public string BulgarianCitizenFirstName { get; set; }
        public string BulgarianCitizenMiddleName { get; set; }
        public string BulgarianCitizenLastName { get; set; }
        public string ForeignCitizenPin { get; set; }
        public string ForeignCitizenPn { get; set; }
        public string ForeignCitizenNamesCyrillic { get; set; }
        public string ForeignCitizenNamesLatin { get; set; }
        public string ForeignCitizenNationality { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameLatin { get; set; }
    }
}
