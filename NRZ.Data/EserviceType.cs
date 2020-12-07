using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class EserviceType
    {
        public EserviceType()
        {
            EservicePaymentRequest = new HashSet<EservicePaymentRequest>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool Deactivated { get; set; }

        public virtual ICollection<EservicePaymentRequest> EservicePaymentRequest { get; set; }
    }
}
