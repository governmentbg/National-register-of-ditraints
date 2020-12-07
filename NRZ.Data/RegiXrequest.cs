using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class RegiXrequest
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public short RegiXreportId { get; set; }
        public string Argument { get; set; }
        public DateTime? CreatedAtUtc { get; set; }
        public DateTime? AnsweredAtUtc { get; set; }
        public string Errors { get; set; }

        public virtual RegiXreport RegiXreport { get; set; }
        public virtual AspNetUsers User { get; set; }
        public virtual RegiXresponse RegiXresponse { get; set; }
    }
}
