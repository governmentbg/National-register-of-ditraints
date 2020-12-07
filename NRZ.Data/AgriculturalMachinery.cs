using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AgriculturalMachinery
    {
        public AgriculturalMachinery()
        {
            AuctionItem = new HashSet<AuctionItem>();
            Distraint = new HashSet<Distraint>();
        }

        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string FrameNumber { get; set; }
        public string Type { get; set; }
        public int? OwnerId { get; set; }
        public int? CompanyId { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Company Company { get; set; }
        public virtual Person Owner { get; set; }
        public virtual ICollection<AuctionItem> AuctionItem { get; set; }
        public virtual ICollection<Distraint> Distraint { get; set; }
    }
}
