using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Property
{
    public class AircraftViewModel
    {
        public int Id { get; set; }
        public string ProducerName { get; set; }
        public string ProducerNameEn { get; set; }
        public string ProducerCountryCode { get; set; }
        public string ProducerCountryName { get; set; }
        public string AirCategoryCode { get; set; }
        public string AirCategoryName { get; set; }
        public string Icao { get; set; }
        public string MsnserialNumber { get; set; }
        public string ModelName { get; set; }
        public string ModelNameEn { get; set; }

        public IEnumerable<AircraftDebtViewModel> Debts { get; set; }
        public IEnumerable<AircraftRegistrationViewModel> Registrations { get; set; }
        public long? ExtensionRequestId { get; set; }
        
    }
}
