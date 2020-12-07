using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.RegiX
{
    public class RegiXReportModel
    {
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
    }
}
