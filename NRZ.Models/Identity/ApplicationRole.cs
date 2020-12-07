using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Identity
{
    public class ApplicationRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
