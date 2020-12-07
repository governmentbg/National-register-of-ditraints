using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class Municipalities
    {
        public Municipalities()
        {
            Address = new HashSet<Address>();
            Cities = new HashSet<Cities>();
            RequestForCertificateOfDistraintOfPropertyMunicipality = new HashSet<RequestForCertificateOfDistraintOfProperty>();
            RequestForCertificateOfDistraintOfPropertyMunicipalityIdOfLegalEntityNavigation = new HashSet<RequestForCertificateOfDistraintOfProperty>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? RegionId { get; set; }
        public DateTime? ActiveTo { get; set; }

        public virtual Regions Region { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<Cities> Cities { get; set; }
        public virtual ICollection<RequestForCertificateOfDistraintOfProperty> RequestForCertificateOfDistraintOfPropertyMunicipality { get; set; }
        public virtual ICollection<RequestForCertificateOfDistraintOfProperty> RequestForCertificateOfDistraintOfPropertyMunicipalityIdOfLegalEntityNavigation { get; set; }
    }
}
