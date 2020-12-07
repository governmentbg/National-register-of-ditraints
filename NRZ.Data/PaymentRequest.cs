﻿using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class PaymentRequest
    {
        public PaymentRequest()
        {
            EservicePaymentRequest = new HashSet<EservicePaymentRequest>();
        }

        public int AisPaymentId { get; set; }
        public string ServiceProviderName { get; set; }
        public string ServiceProviderBank { get; set; }
        public string ServiceProviderBic { get; set; }
        public string ServiceProviderIban { get; set; }
        public string Currency { get; set; }
        public string PaymentTypeCode { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentReason { get; set; }
        public int ApplicantUinTypeId { get; set; }
        public string ApplicantUin { get; set; }
        public string ApplicantName { get; set; }
        public string PaymentReferenceType { get; set; }
        public string PaymentReferenceNumber { get; set; }
        public DateTime PaymentReferenceDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string AdditionalInformation { get; set; }
        public string AdministrativeServiceUri { get; set; }
        public string AdministrativeServiceSupplierUri { get; set; }
        public string AdministrativeServiceNotificationUrl { get; set; }

        public virtual ICollection<EservicePaymentRequest> EservicePaymentRequest { get; set; }
    }
}
