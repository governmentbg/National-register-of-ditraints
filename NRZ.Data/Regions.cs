using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class Regions
    {
        public Regions()
        {
            Address = new HashSet<Address>();
            Municipalities = new HashSet<Municipalities>();
            RequestForCertificateOfDistraintOfPropertyRegion = new HashSet<RequestForCertificateOfDistraintOfProperty>();
            RequestForCertificateOfDistraintOfPropertyRegionIdOfLegalEntityNavigation = new HashSet<RequestForCertificateOfDistraintOfProperty>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Nuts3Code { get; set; }
        public DateTime? ActiveTo { get; set; }

        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<Municipalities> Municipalities { get; set; }
        public virtual ICollection<RequestForCertificateOfDistraintOfProperty> RequestForCertificateOfDistraintOfPropertyRegion { get; set; }
        public virtual ICollection<RequestForCertificateOfDistraintOfProperty> RequestForCertificateOfDistraintOfPropertyRegionIdOfLegalEntityNavigation { get; set; }
    }
}
