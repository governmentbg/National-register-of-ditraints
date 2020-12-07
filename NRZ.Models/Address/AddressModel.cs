using System.ComponentModel.DataAnnotations;

namespace NRZ.Models.Address
{
    public class AddressModel
    {
        public int? Id { get; set; }

        public int RegionId { get; set; }

        public int MunicipalityId { get; set; }

        public int CityId { get; set; }

        [Required]
        public string StreetAddress { get; set; }

    }
}
