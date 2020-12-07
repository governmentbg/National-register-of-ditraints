using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class EservicePaymentRequest
    {
        public EservicePaymentRequest()
        {
            EservicePaymentRequestStatusHistory = new HashSet<EservicePaymentRequestStatusHistory>();
        }

        public int Id { get; set; }
        public string EserviceTypeCode { get; set; }
        public int? SeizedPropertyCertificateRequestId { get; set; }
        public int? SeizedPropertyReportRequestId { get; set; }
        public int PaymentRequestId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string StatusCode { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? AisPaymentId { get; set; }

        public virtual EserviceType EserviceTypeCodeNavigation { get; set; }
        public virtual PaymentRequest PaymentRequest { get; set; }
        public virtual RequestForCertificateOfDistraintOfProperty SeizedPropertyCertificateRequest { get; set; }
        public virtual SeizedPropertyAvailabilityRequest SeizedPropertyReportRequest { get; set; }
        public virtual PaymentRequestStatus StatusCodeNavigation { get; set; }
        public virtual ICollection<EservicePaymentRequestStatusHistory> EservicePaymentRequestStatusHistory { get; set; }
    }
}
