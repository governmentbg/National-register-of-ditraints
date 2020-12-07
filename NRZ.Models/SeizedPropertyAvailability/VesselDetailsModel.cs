namespace NRZ.Models.SeizedPropertyAvailability
{
    public class VesselDetailsModel
    {
        public decimal? Bt { get; set; }
        public decimal? Nt { get; set; }
        public decimal? MaxLength { get; set; }
        public decimal? LengthBetweenPerpendiculars { get; set; }
        public decimal? MaxWidth { get; set; }
        public decimal? Waterplane { get; set; }
        public decimal? ShipboardHeight { get; set; }
        public decimal? DeadWight { get; set; }
        public int? NumberOfEngines { get; set; }
        public string EnginesFuel { get; set; }
        public decimal? SumEnginePower { get; set; }
        public string BodyNumber { get; set; }
    }
}
