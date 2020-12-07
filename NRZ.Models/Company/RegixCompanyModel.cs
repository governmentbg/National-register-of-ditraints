using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Company
{
    public class RegixCompanyModel
    {
        public int Id { get; set; }
        public string Uic { get; set; }
        public string Name { get; set; }
        public string LegalFormAbbr { get; set; }
        public string LegalFormName { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string StatusNameEn { get; set; }
        public long? RequestId { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
