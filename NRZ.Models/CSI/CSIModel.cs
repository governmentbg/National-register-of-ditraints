using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NRZ.Models.CSI
{
    public class CSIModel
    {
        public int Number { get; set; }
        public string Name_bg { get; set; }
        public string Name_en { get; set; }
        public string Region_bg { get; set; }
        public string Region_en { get; set; }
        public string Address_bg { get; set; }
        public string Address_en { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Mail { get; set; }
        public string Website { get; set; }
        public DateTime? Date_from { get; set; }
        public DateTime? Date_to { get; set; }
        public string Company { get; set; }
        public List<CSIModel> Helpers { get; set; }
    }
}
