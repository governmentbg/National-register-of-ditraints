using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class PaymentRequestStatus
    {
        public PaymentRequestStatus()
        {
            EservicePaymentRequest = new HashSet<EservicePaymentRequest>();
            EservicePaymentRequestStatusHistory = new HashSet<EservicePaymentRequestStatusHistory>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool Deactivated { get; set; }

        public virtual ICollection<EservicePaymentRequest> EservicePaymentRequest { get; set; }
        public virtual ICollection<EservicePaymentRequestStatusHistory> EservicePaymentRequestStatusHistory { get; set; }
    }
}
