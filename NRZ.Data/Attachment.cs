using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class Attachment
    {
        public Attachment()
        {
            AnnouncementAttachments = new HashSet<AnnouncementAttachments>();
            AuctionAnnouncement = new HashSet<AuctionAnnouncement>();
            AuctionRegistrationAttachment = new HashSet<AuctionRegistrationAttachment>();
        }

        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }

        public virtual ICollection<AnnouncementAttachments> AnnouncementAttachments { get; set; }
        public virtual ICollection<AuctionAnnouncement> AuctionAnnouncement { get; set; }
        public virtual ICollection<AuctionRegistrationAttachment> AuctionRegistrationAttachment { get; set; }
    }
}
