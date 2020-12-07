using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Distraint
{
    public class DistraintExemptModel
    {
        public long Id { get; set; }
        public DateTime? ExemptionDate { get; set; }
        public string ExemptedBy { get; set; }
        public DateTime? ExemptedAt { get; set; }
    }
}
