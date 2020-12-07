using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class PropertyType
    {
        public PropertyType()
        {
            AuctionItem = new HashSet<AuctionItem>();
            Distraint = new HashSet<Distraint>();
            RegiXreportToPropertyType = new HashSet<RegiXreportToPropertyType>();
            RequestForCertificateOfDistraintOfProperty = new HashSet<RequestForCertificateOfDistraintOfProperty>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool Deactivated { get; set; }
        public bool? UseForDistraint { get; set; }

        public virtual ICollection<AuctionItem> AuctionItem { get; set; }
        public virtual ICollection<Distraint> Distraint { get; set; }
        public virtual ICollection<RegiXreportToPropertyType> RegiXreportToPropertyType { get; set; }
        public virtual ICollection<RequestForCertificateOfDistraintOfProperty> RequestForCertificateOfDistraintOfProperty { get; set; }
    }
}
