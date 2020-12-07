using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class EservicePaymentRequestStatusHistory
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string StatusCode { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? EserviceTime { get; set; }
        public string Errors { get; set; }

        public virtual EservicePaymentRequest Request { get; set; }
        public virtual PaymentRequestStatus StatusCodeNavigation { get; set; }
    }
}
