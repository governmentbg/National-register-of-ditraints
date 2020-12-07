using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.EPayment
{
    public class EServicePaymentStatusHistoryModel
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string StatusCode { get; set; }
        public string StatusCodeName { get; set; }
        public string StatusCodeNameEn { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? EServiceTime { get; set; }
        public string Errors { get; set; }
    }
}
