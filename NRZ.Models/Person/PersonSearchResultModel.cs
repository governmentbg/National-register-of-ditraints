using NRZ.RegiX.Client.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Person
{
    public class PersonSearchResultModel
    {
        public string PersonIdentifier { get; set; }
        public long? RequestId { get; set; }
        public BaseResponse ResponseObject { get; set; }
    }
}
