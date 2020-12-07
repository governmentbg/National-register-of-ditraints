using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class ActiveJwt
    {
        public string Jwt { get; set; }
        public DateTime ExpDateUtc { get; set; }
    }
}
