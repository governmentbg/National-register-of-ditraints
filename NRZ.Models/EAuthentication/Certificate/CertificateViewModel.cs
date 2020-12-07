using System.Security.Cryptography.X509Certificates;

namespace NRZ.Models.EAuthentication.Certificate
{
    public class CertificateViewModel
    {
        public X509Certificate2 X509 { get; set; }

        public string Error { get; set; }

        public bool IsEmpty
        {
            get { return X509 == null && string.IsNullOrEmpty(Error); }
        }
    }
}
