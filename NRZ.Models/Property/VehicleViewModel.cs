using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Property
{
    public class VehicleViewModel
    {
        public long Id { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime? FirstRegistrationDate { get; set; }
        public string Vin { get; set; }
        public string EngineNumber { get; set; }
        public string VehicleType { get; set; }
        public string Model { get; set; }
        public string TypeApprovalNumber { get; set; }
        public string ApprovalType { get; set; }
        public string TradeDescription { get; set; }
        public string Color { get; set; }
        public string Category { get; set; }
        public string OffRoadSymbols { get; set; }
        public string MassG { get; set; }
        public string MassF1 { get; set; }
        public string MassF2 { get; set; }
        public string MassF3 { get; set; }
        public string VehicleNumOfAxles { get; set; }
        public string VehicleMassO1 { get; set; }
        public string VehicleMassO2 { get; set; }
        public string Capacity { get; set; }
        public string MaxPower { get; set; }
        public string Fuel { get; set; }
        public string EnvironmentalCategory { get; set; }
        public string VehicleDocumentNumber { get; set; }
        public DateTime? VehicleDocumentDate { get; set; }

        public IEnumerable<VehicleOwnerViewModel> Owners { get; set; }
        public IEnumerable<VehicleUserViewModel> Users { get; set; }
        public long? ExtensionRequestId { get; set; }
    }
}
