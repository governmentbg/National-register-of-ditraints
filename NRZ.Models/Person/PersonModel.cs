using NRZ.Models.Address;
using System.ComponentModel.DataAnnotations;

namespace NRZ.Models.Person
{
    public class PersonModel
    {
        public int? Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string IdentificationType { get; set; }

        public string IdentificationNumber { get; set; }

        public AddressModel Address { get; set; }
    }
}
