using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.EPayment
{
    public class EServicePaymentRequestModel
    {
        public int Id { get; set; }
        public string EserviceTypeCode { get; set; }
        public string EserviceTypeName { get; set; }
        public string EserviceTypeNameEn { get; set; }
        public int? SeizedPropertyCertificateRequestId { get; set; }
        public int? SeizedPropertyReportRequestId { get; set; }
        public int ServiceRequestId { get; set; }
        public int PaymentRequestId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string StatusNameEn { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
