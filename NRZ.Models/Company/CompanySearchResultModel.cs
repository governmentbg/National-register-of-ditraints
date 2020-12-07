using NRZ.RegiX.Client.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Company
{
    public class CompanySearchResultModel
    {
        public string CompanyIdentifier { get; set; }
        public long? RequestId { get; set; }
        public BaseResponse ResponseObject { get; set; }
    }
}
