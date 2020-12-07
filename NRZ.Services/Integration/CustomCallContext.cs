using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NRZ.Services.Integration
{
    public class CustomCallContext
    {
        public X509Certificate2 Certificate { get; }
        public string EAuthToken { get; }

        public CustomCallContext()
        { }

        public CustomCallContext(
            X509Certificate2 certificate,
            string eAuthToken)
        {
            Certificate = certificate;
            EAuthToken = eAuthToken;
        }
    }
}
