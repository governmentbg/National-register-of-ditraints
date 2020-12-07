using System.Collections.Generic;

namespace NRZ.Models.AgriculturalMachinery
{
    public class AgriculturalMachineryResponseModel
    {
        public int Total { get; set; }

        public IEnumerable<object> Items { get; set; }
    }
}
