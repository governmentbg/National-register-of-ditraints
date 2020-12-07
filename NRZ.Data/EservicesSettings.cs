using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class EservicesSettings
    {
        public string ServiceProviderName { get; set; }
        public string ServiceProviderBank { get; set; }
        public string ServiceProviderBic { get; set; }
        public string ServiceProviderIban { get; set; }
        public string Currency { get; set; }
        public int? SeizedPropertyCertificateRequestExpirationDays { get; set; }
        public decimal? SeizedPropertyCertificateFee { get; set; }
        public string SeizedPropertyCertificateReason { get; set; }
        public int? SeizedPropertyReportRequestExpirationDays { get; set; }
        public decimal? SeizedPropertyByOwnerReportFee { get; set; }
        public string SeizedPropertyReportReason { get; set; }
        public string AdministrativeServiceNotificationUrl { get; set; }
    }
}
