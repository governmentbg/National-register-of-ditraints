using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class OtherProperty
    {
        public OtherProperty()
        {
            AuctionItem = new HashSet<AuctionItem>();
            Distraint = new HashSet<Distraint>();
            RequestForCertificateOfDistraintOfProperty = new HashSet<RequestForCertificateOfDistraintOfProperty>();
        }

        public long Id { get; set; }
        public string Identifier { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? CreatedBy { get; set; }
        public bool IsManuallyAdded { get; set; }

        public virtual AssetType TypeNavigation { get; set; }
        public virtual ICollection<AuctionItem> AuctionItem { get; set; }
        public virtual ICollection<Distraint> Distraint { get; set; }
        public virtual ICollection<RequestForCertificateOfDistraintOfProperty> RequestForCertificateOfDistraintOfProperty { get; set; }
    }
}
