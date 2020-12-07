using System;

namespace NRZ.Models.SeizedPropertyAvailability
{
    public class VehicleDetailsModel
    {
        public string VehicleType { get; set; }
        public string Model { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime? FirstRegistrationDate { get; set; }
        public string Vin { get; set; }
        public string Capacity { get; set; }
        public string MaxPower { get; set; }
        public string Fuel { get; set; }
        public string EnvironmentalCategory { get; set; }
        public string VehicleDocumentNumber { get; set; }
        public DateTime? VehicleDocumentDate { get; set; }
    }
}
