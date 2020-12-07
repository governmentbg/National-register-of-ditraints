using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class RegiXreportToPropertyType
    {
        public short Id { get; set; }
        public short RegiXreportId { get; set; }
        public string RegiXsearchCriteriaTypeCode { get; set; }
        public string PropertyTypeCode { get; set; }

        public virtual PropertyType PropertyTypeCodeNavigation { get; set; }
        public virtual RegiXreport RegiXreport { get; set; }
        public virtual RegiXsearchCriteriaType RegiXsearchCriteriaTypeCodeNavigation { get; set; }
    }
}
