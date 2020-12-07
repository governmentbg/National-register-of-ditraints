using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AssetType
    {
        public AssetType()
        {
            OtherProperty = new HashSet<OtherProperty>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool Deactivated { get; set; }
        public int Sort { get; set; }

        public virtual ICollection<OtherProperty> OtherProperty { get; set; }
    }
}
