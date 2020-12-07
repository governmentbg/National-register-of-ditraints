using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Property
{
    public class PropertySearchRequestModel
    {
        public string IdentifierTypeCode { get; set; }
        public string Identifier { get; set; }
        public string SuitNumber { get; set; }

    }
}
