using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Property
{
    public class VesselViewModel
    {
        public int Id { get; set; }
        public decimal? Bt { get; set; }
        public decimal? Nt { get; set; }
        public decimal? MaxLength { get; set; }
        public decimal? LengthBetweenPerpendiculars { get; set; }
        public decimal? MaxWidth { get; set; }
        public decimal? Waterplane { get; set; }
        public decimal? ShipboardHeight { get; set; }
        public decimal? Deadweight { get; set; }
        public int? NumberOfEngines { get; set; }
        public string EnginesFuel { get; set; }
        public decimal? SumEnginePower { get; set; }
        public string BodyNumber { get; set; }

        public IEnumerable<VesselEngineViewModel> Engines { get; set; }
        public IEnumerable<VesselOwnerViewModel> Owners { get; set; }
        public VesselRegistrationDataViewData RegistrationData { get; set; }
        public long? ExtensionRequestId { get; set; }
    }
}
