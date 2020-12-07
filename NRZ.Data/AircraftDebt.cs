using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AircraftDebt
    {
        public int Id { get; set; }
        public int AircraftId { get; set; }
        public DateTime? InputDate { get; set; }
        public int? DebtTypeCode { get; set; }
        public string DebtType { get; set; }
        public bool? IsActive { get; set; }
        public string ApplicantIdentifier { get; set; }
        public string ApplicantName { get; set; }
        public string DocumentIncomingNumber { get; set; }
        public DateTime? DocumentIncomingDate { get; set; }
        public string DocumentExternalNumber { get; set; }
        public DateTime? DocumentExternalDate { get; set; }
        public DateTime? RepaymentDate { get; set; }
        public string RepaymentDocumentIncomingNumber { get; set; }
        public DateTime? RepaymentDocumentIncomingDate { get; set; }
        public string RepaymentDocumentExternalNumber { get; set; }
        public DateTime? RepaymentDocumentExternalDate { get; set; }
        public string RepaymentNotes { get; set; }
        public string Notes { get; set; }

        public virtual Aircraft Aircraft { get; set; }
    }
}
