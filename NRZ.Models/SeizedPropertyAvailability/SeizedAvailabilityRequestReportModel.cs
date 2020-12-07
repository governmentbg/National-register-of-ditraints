using System;

namespace NRZ.Models.SeizedPropertyAvailability
{
    public class SeizedAvailabilityRequestReportModel
    {
        public string PropertyName { get; set; }

        public string PropertyType { get; set; }

        public string PropertyTypeCode { get; set; }

        public string EnforcementDate { get; set; }

        public string EnforcedBy { get; set; }

        public string EnforcedAt { get; set; }

        public string InFavorOf { get; set; }

        public string DebtorName { get; set; }
    }
}
