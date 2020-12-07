using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.EPayment
{
    public class EServicePaymentRequestCreateModel
    {
        public int Id { get; set; }
        public string EserviceTypeCode { get; set; }
        public int? SeizedPropertyCertificateRequestId { get; set; }
        public int? SeizedPropertyReportRequestId { get; set; }
        public int PaymentRequestId { get; set; }
        public string StatusCode { get; set; }
    }
}
