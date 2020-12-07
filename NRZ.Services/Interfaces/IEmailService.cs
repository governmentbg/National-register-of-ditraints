using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string to, string cc, string bcc, string subject, string body, IList<byte[]> files = null);
    }
}
