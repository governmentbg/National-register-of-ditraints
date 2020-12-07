using NRZ.RegiX.Client.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Property
{
    public class PropertySearchResultModel
    {
        public string PropertyIdentifier { get; set; }
        public long? RequestId { get; set; }
        public BaseResponse ResponseObject { get; set; }
    }
}
