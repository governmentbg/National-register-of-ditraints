using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Settings
{
    public class HangFireJobSettings
    {
        public int? PaymentRequestSendingJobMinutesInterval { get; set; }
        public int? AuctionEndCheckJobMinutesInterval { get; set; }
        public int? PaymentRequestStatusCheckJobMinutesInterval { get; set; }
    }
}
