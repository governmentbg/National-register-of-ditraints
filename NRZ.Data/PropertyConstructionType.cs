﻿using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class PropertyConstructionType
    {
        public PropertyConstructionType()
        {
            Property = new HashSet<Property>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool Deactivated { get; set; }
        public int Sort { get; set; }

        public virtual ICollection<Property> Property { get; set; }
    }
}
