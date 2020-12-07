using NRZ.Models.Company;
using NRZ.Models.Person;
using System.ComponentModel.DataAnnotations;

namespace NRZ.Models.AgriculturalMachinery
{
   public class AgriculturalMachineryModel 
    {
        [Required]
        public string RegistrationNumber { get; set; }

        [Required]
        public string FrameNumber { get; set; }

        [Required]
        public string Type { get; set; }

        public PersonModel Person { get; set; }

        public CompanyModel Company { get; set; }
    }
}
