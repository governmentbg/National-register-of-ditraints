using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class Aircraft
    {
        public Aircraft()
        {
            AircraftDebt = new HashSet<AircraftDebt>();
            AircraftRegistration = new HashSet<AircraftRegistration>();
            AuctionItem = new HashSet<AuctionItem>();
            Distraint = new HashSet<Distraint>();
            RequestForCertificateOfDistraintOfProperty = new HashSet<RequestForCertificateOfDistraintOfProperty>();
        }

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

        public virtual AircraftExtension AircraftExtension { get; set; }
        public virtual ICollection<AircraftDebt> AircraftDebt { get; set; }
        public virtual ICollection<AircraftRegistration> AircraftRegistration { get; set; }
        public virtual ICollection<AuctionItem> AuctionItem { get; set; }
        public virtual ICollection<Distraint> Distraint { get; set; }
        public virtual ICollection<RequestForCertificateOfDistraintOfProperty> RequestForCertificateOfDistraintOfProperty { get; set; }
    }
}
