using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Settings
{
    public class RegiXCertificateSettings
    {
        public string BindingName { get; set; }
        public uint MaxReceivedMessageSize { get; set; }
        public string EndpointAddress { get; set; }
        public string CertificateThumbprint { get; set; }
        public bool SaveEntityWithSearchedIdentifier { get; set; }
        public bool UseVehicleV3 { get; set; }
    }
}
