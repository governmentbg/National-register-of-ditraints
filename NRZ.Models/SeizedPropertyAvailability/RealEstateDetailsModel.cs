namespace NRZ.Models.SeizedPropertyAvailability
{
    public class RealEstateDetailsModel
    {
        public string Type { get; set; }
        public int? Floor { get; set; }
        public string Area { get; set; }
        public string PropertyConstructionType { get; set; }
        public string Identifier { get; set; }
        public string IdentifierType { get; set; }
        public string Description { get; set; }
    }
}
