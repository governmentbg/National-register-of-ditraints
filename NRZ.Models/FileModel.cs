using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models
{
    public class AttachmentModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public string Type { get; set; }
    }
}
