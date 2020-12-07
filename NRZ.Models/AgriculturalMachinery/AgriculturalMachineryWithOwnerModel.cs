using NRZ.Models.Company;
using NRZ.Models.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.AgriculturalMachinery
{
   public class AgriculturalMachineryWithOwnerModel
    {
        public int Id { get; set; }

        public string RegistrationNumber { get; set; }

        public string FrameNumber { get; set; }

        public string Type { get; set; }

        public PersonModel Owner { get; set; }

        public CompanyModel Company { get; set; }

    }
}
