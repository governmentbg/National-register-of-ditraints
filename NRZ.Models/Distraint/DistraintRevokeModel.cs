using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Distraint
{
    public class DistraintRevokeModel
    {
        public long Id { get; set; }
        public DateTime? RevocationDate { get; set; }
        public string RevokedBy { get; set; }
        public DateTime? RevokedAt { get; set; }
    }
}
