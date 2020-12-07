using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class RegiXresponse
    {
        public long RequestId { get; set; }
        public string RawContent { get; set; }

        public virtual RegiXrequest Request { get; set; }
    }
}
