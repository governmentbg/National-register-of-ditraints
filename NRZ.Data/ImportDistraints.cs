using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class ImportDistraints
    {
        public string ВПолзаНаИмена { get; set; }
        public string ВПолзаНаЛице { get; set; }
        public double? Дело { get; set; }
        public string ДлъжникИмена { get; set; }
        public string ДлъжникЛице { get; set; }
        public double? ЕгнЕик { get; set; }
        public string ЧсиИме { get; set; }
        public double? Рег { get; set; }
        public string ТипЗапорираноИмущество { get; set; }
        public string ИдентификаторНаВещРегНаМпсMsnНомерИДр { get; set; }
        public string ОписаниеНаВещКогатоЕТипДруга { get; set; }
        public string МестоположениеНаИмуществото { get; set; }
        public DateTime? ДатаНаВръчванеНаЗапорноСъобщение { get; set; }
    }
}
