using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AnouncementAttachments
    {
        public int AnnouncementId { get; set; }
        public int AttachmentId { get; set; }

        public virtual AuctionAnnouncement Announcement { get; set; }
        public virtual Attachment Attachment { get; set; }
    }
}
