using Disig.TimeStampClient;
using NRZ.Ts.Client.Enums;
using NRZ.Ts.Client.Models;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Tsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace NRZ.Ts.Client
{
    /// <summary>
    /// Timestamp генератор.
    /// </summary>
    public class TimestampGenerator
    {
        // Алгоритъм по подразбиране(sha1, sha256, sha512, md5)
        private const string HashAlg = "sha256";
        private const string DefaultFileName = "UnspecifiedFileName.json";

        private readonly Dictionary<Authority, string> AuthorityServiceList = new Dictionary<Authority, string>()
        {
            {Authority.Infonotary, "http://ts.infonotary.com/tsa"},
            {Authority.B_Trust, "http://tsa.b-trust.org"},
            {Authority.StampIt, "https://tsa.stampit.org"},
            {Authority.StampIt1, "https://tsa.stampit.org/api/v1/"},
            {Authority.Spektar, ""},
            {Authority.MobiSafe, "http://tsa.sep.bg/"},
            {Authority.BcExample, "http://www.cryptopro.ru/tsp/tsp.srf"},
            {Authority.StampItNew, "https://tsa-srv.stampit.org/ts/"},
            {Authority.Evotrust, "http://ts.evrotrust.com/tsa"},
        };

        private string _authorityServiceUrl;
        private X509Certificate2 _authorityCert;
        private Authority _authority;

        private void AppendLog(string text)
        {
            // Todo: add logger
            //_logger.Info(text);
        }

        /// <summary>
        /// </summary>
        /// <param name="authority">Доставчик на услугата. Имеплементирана лога само за Infonotary и Evotrust от <see cref="Authority"/></param>
        public TimestampGenerator(Authority authority)
        {
            LoadAuthority(authority);
        }

        public void ChangeAutorithyInfo(Authority authority)
        {
            LoadAuthority(authority);
        }

        public TimestampResult Generate(byte[] contentToTimeStamp, string fileName)
        {
            return GetTimestamp(contentToTimeStamp, fileName);
        }

        public string Validate(byte[] tsr)
        {
            AppendLog("TimeStamp validating...");

            Org.BouncyCastle.Tsp.TimeStampToken token;
            try
            {
                token = new Org.BouncyCastle.Tsp.TimeStampToken(new CmsSignedData(tsr));
            }
            catch (Exception e)
            {
                AppendLog(e.Message);

                return "Invalid tsr";
            }

            try
            {
                var certParser = new Org.BouncyCastle.X509.X509CertificateParser();
                var bouncyCert = certParser.ReadCertificate(_authorityCert.GetRawCertData());

                token.Validate(bouncyCert);
                return "Valid timestamp";
            }
            catch (Exception e)
            {
                AppendLog(e.Message);

                return e is TspValidationException || e is TspException
                    ? e.Message
                    : "Validation error";
            }
        }

        private void LoadAuthority(Authority authority)
        {
            _authorityServiceUrl = AuthorityServiceList[authority];
            _authority = authority;
            _authorityCert = GetAuthorityCert(authority);
            
        }

        private X509Certificate2 GetAuthorityCert(Authority authority)
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string certFilePath = null;
            switch (authority)
            {
                case Authority.Infonotary:
                    certFilePath = Path.Combine(dir, "in-csp-root.cer");
                    break;
                case Authority.Evotrust:
                    certFilePath = Path.Combine(dir, "TSA.crt");
                    break;
                case Authority.B_Trust:
                case Authority.StampIt:
                case Authority.StampIt1:
                case Authority.Spektar:
                case Authority.MobiSafe:
                case Authority.BcExample:
                case Authority.StampItNew:
                default:
                    break;
            }

            return string.IsNullOrWhiteSpace(certFilePath)
                ? null
                : new X509Certificate2(certFilePath);
        }

        private TimestampResult GetTimestamp(byte[] contentToTimeStamp, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) fileName = DefaultFileName;
            var credentials = new UserCredentials(this._authorityCert);

            TimestampResult result = Utils.RequestTimeStamp(_authorityServiceUrl, 
                contentToTimeStamp, fileName, HashAlg, true, credentials, 
                AppendLog, true);

            return result;
        }
    }
}
