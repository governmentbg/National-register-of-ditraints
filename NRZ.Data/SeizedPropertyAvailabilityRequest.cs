using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class SeizedPropertyAvailabilityRequest
    {
        public SeizedPropertyAvailabilityRequest()
        {
            EservicePaymentRequest = new HashSet<EservicePaymentRequest>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string RequestorUserId { get; set; }
        public int? RequestorPersonId { get; set; }
        public int? RequesterCompanyId { get; set; }
        public string RequesterCompanyRepresentative { get; set; }
        public string RequesterCompanyEik { get; set; }
        public string RequesterCompanyCaseNumber { get; set; }
        public bool IsCheckedPerson { get; set; }
        public int? CheckedPersonId { get; set; }
        public int? CheckedCompanyId { get; set; }
        public string InTheQualityOfPersonTypeCode { get; set; }

        public virtual Company CheckedCompany { get; set; }
        public virtual Person CheckedPerson { get; set; }
        public virtual RequesterType InTheQualityOfPersonTypeCodeNavigation { get; set; }
        public virtual Company RequesterCompany { get; set; }
        public virtual Person RequestorPerson { get; set; }
        public virtual AspNetUsers RequestorUser { get; set; }
        public virtual ICollection<EservicePaymentRequest> EservicePaymentRequest { get; set; }
    }
}
