using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NRZ.Data;
using NRZ.Models.Settings;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace NRZ.Services.Notifications
{
    public class EmailService : BaseService, IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(NRZContext context,
            IOptions<EmailSettings> emailConfig,
            IStringLocalizer<SharedResources> localizer = null)
            : base(context, localizer)
        {
            emailSettings = emailConfig.Value;
        }

        private MailAddress GetMailAddress()
        {
            return new MailAddress(emailSettings.SenderEmail, emailSettings.SenderName);
        }

        /// <summary>
        /// Create the SmtpClient object
        /// </summary>
        /// <returns></returns>
        private SmtpClient GetSmtpClient()
        {
            return new SmtpClient
            {
                Host = emailSettings.MailServer,
                Port = emailSettings.MailPort,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password)
            };
        }

        public void SendEmail(string to, string cc, string bcc, string subject, string body, IList<byte[]> files = null)
        {
            if (string.IsNullOrWhiteSpace(to))
            {
                return;
            }

            //Create the MailMessage instance 
            var myMailMessage = new MailMessage()
            {
                From = GetMailAddress(),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };
            SmtpClient smtp = null;

            try
            {
                myMailMessage.To.Add(to);
                if (!string.IsNullOrWhiteSpace(cc))
                {
                    myMailMessage.CC.Add(cc);
                }

                if (!string.IsNullOrWhiteSpace(bcc))
                {
                    myMailMessage.CC.Add(bcc);
                }

                if (files != null)
                {
                    foreach (byte[] file in files.Where(x => x.Length > 0))
                    {
                        myMailMessage.Attachments.Add(new System.Net.Mail.Attachment(new MemoryStream(file), "no file name"));
                    }
                }

                smtp = GetSmtpClient();
                smtp.SendCompleted += (s, e) =>
                {
                    // Get the unique identifier for this asynchronous operation.
                    var token = (string)e.UserState;

                    if (e.Cancelled)
                    {
                        //TODO Log
                        Debug.WriteLine("[{0}] Send canceled.", token);
                    }
                    if (e.Error != null)
                    {
                        Debug.WriteLine("[{0}] {1}", token, e.Error.ToString());
                    }
                    else
                    {
                        Debug.WriteLine("Message sent.");
                    }

                    smtp.Dispose();
                    myMailMessage.Dispose();
                };
                //Send the MailMessage (will use the Web.config settings) 
                smtp.Send(myMailMessage);
            }
            catch (Exception)
            {
                if (smtp != null)
                    smtp.Dispose();
                if (myMailMessage != null)
                    myMailMessage.Dispose();
                return;
            }
        }
    }
}
