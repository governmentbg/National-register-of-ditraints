using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AuctionRegistrationAttachment
    {
        public int AuctionRegistrationId { get; set; }
        public int AttachmentId { get; set; }

        public virtual Attachment Attachment { get; set; }
        public virtual AuctionRegistration AuctionRegistration { get; set; }
    }
}
