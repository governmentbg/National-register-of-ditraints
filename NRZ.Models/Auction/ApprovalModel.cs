using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NRZ.Models.Auction
{
    public class ApprovalModel
    {
        [Required]
        public int EntityId { get; set; }
        [Required]
        public bool Approved { get; set; }
        public string RejectReason { get; set; }
    }
}
