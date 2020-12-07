using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Nomenclatures
{
    public class NomenclatureBaseModel
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool Deactivated { get; set; }
        public short Sort { get; set; }
    }
}
