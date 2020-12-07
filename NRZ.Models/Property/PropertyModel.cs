using NRZ.Models.Address;

namespace NRZ.Models.Property
{
    public class PropertyModel
    {
        public long? Id { get; set; }
        public string Type { get; set; }
        public string Floor { get; set; }
        public decimal? Area { get; set; }
        public AddressModel Address { get; set; }
        public string PropertyConstructionTypeId { get; set; }
        public string Identifier { get; set; }
        public string IdentifierType { get; set; }
        public string Description { get; set; }
        public bool IsManuallyAdded { get; set; }
    }
}
