using NRZ.RegiX.Client.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Property
{
    public class AgriculturalMachineryCollectionModel : BaseResponse
    {
        public IEnumerable<AgriculturalMachineryViewModel> Machines { get; set; }
    }
}
