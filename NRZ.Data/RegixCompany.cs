using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class RegixCompany
    {
        public RegixCompany()
        {
            DistraintDebtorCompany = new HashSet<Distraint>();
            DistraintInFavourOfCompany = new HashSet<Distraint>();
        }

        public int Id { get; set; }
        public string Uic { get; set; }
        public string Name { get; set; }
        public string LegalFormAbbr { get; set; }
        public string LegalFormName { get; set; }
        public string StatusCode { get; set; }
        public long? RequestId { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual RegixCompanyStatus StatusCodeNavigation { get; set; }
        public virtual ICollection<Distraint> DistraintDebtorCompany { get; set; }
        public virtual ICollection<Distraint> DistraintInFavourOfCompany { get; set; }
    }
}
