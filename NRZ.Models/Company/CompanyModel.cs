using NRZ.Models.Address;
using System.ComponentModel.DataAnnotations;

namespace NRZ.Models.Company
{
    public class CompanyModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string EIK { get; set; }

        public string CompanyCaseNumber { get; set; }

        public AddressModel Address { get; set; }
    }
}
