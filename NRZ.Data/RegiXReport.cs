using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class RegiXreport
    {
        public RegiXreport()
        {
            RegiXreportToPropertyType = new HashSet<RegiXreportToPropertyType>();
            RegiXrequest = new HashSet<RegiXrequest>();
        }

        public short Id { get; set; }
        public string ProviderName { get; set; }
        public string RegisterName { get; set; }
        public string ReportName { get; set; }
        public string AdapterSubdirectory { get; set; }
        public string OperationName { get; set; }
        public string RequestXsd { get; set; }
        public string ResponseXsd { get; set; }
        public string Operation { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<RegiXreportToPropertyType> RegiXreportToPropertyType { get; set; }
        public virtual ICollection<RegiXrequest> RegiXrequest { get; set; }
    }
}
