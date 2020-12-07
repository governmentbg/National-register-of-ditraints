using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AuctionSettings
    {
        public int Id { get; set; }
        public string AuctionOrderCodeId { get; set; }
        public int? RegistrationDuration { get; set; }
        public int? AuctionStartAfterRegistration { get; set; }
        public int? AuctionDuration { get; set; }

        public virtual AuctionOrder AuctionOrderCode { get; set; }
    }
}
