using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class RegixCompanyStatus
    {
        public RegixCompanyStatus()
        {
            RegixCompany = new HashSet<RegixCompany>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool Deactivated { get; set; }

        public virtual ICollection<RegixCompany> RegixCompany { get; set; }
    }
}
