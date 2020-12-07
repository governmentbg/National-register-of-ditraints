using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class RegiXsearchCriteriaType
    {
        public RegiXsearchCriteriaType()
        {
            RegiXreportToPropertyType = new HashSet<RegiXreportToPropertyType>();
        }

        public string Code { get; set; }
        public string ElementName { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
        public bool Deactivated { get; set; }

        public virtual ICollection<RegiXreportToPropertyType> RegiXreportToPropertyType { get; set; }
    }
}
