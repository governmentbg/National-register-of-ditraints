using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Distraint
{
    public class DistraintEnforceModel
    {
        public long Id { get; set; }
        public DateTime? EnforcementDate { get; set; }
        public string EnforcedBy { get; set; }
        public DateTime? EnforcedAt { get; set; }
    }
}
