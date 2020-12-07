using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class Cities
    {
        public Cities()
        {
            Address = new HashSet<Address>();
            RequestForCertificateOfDistraintOfPropertyCity = new HashSet<RequestForCertificateOfDistraintOfProperty>();
            RequestForCertificateOfDistraintOfPropertyCityIdOfLegalEntityNavigation = new HashSet<RequestForCertificateOfDistraintOfProperty>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string PostCode { get; set; }
        public string Prefix { get; set; }
        public string Ekatte { get; set; }
        public string DisadvantagedCode { get; set; }
        public string PointY { get; set; }
        public string PointX { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int? MunicipalityId { get; set; }
        public DateTime? ActiveTo { get; set; }

        public virtual Municipalities Municipality { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<RequestForCertificateOfDistraintOfProperty> RequestForCertificateOfDistraintOfPropertyCity { get; set; }
        public virtual ICollection<RequestForCertificateOfDistraintOfProperty> RequestForCertificateOfDistraintOfPropertyCityIdOfLegalEntityNavigation { get; set; }
    }
}
