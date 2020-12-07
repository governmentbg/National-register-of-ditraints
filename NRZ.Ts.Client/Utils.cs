using Disig.TimeStampClient;
using NRZ.Ts.Client.Models;
using System;
using System.Security.Cryptography;
using Oid = Disig.TimeStampClient.Oid;

namespace NRZ.Ts.Client
{
    internal class Utils
    {
        public delegate void LogDelegate(string msg);
        private const string DateTimeFormat = "dd MMM yyyy HH':'mm':'ss 'GMT'";

        internal static TimestampResult RequestTimeStamp(string tsaService, 
            string fileName, string hashAlg, bool certReq, UserCredentials credentials, 
            LogDelegate logger, bool logExceptions)
        {
            string policy = "";
            byte[] nonceBytes = GenerateNonceBytes();
            string nonce = BytesToHexString(nonceBytes);

            TimestampResult result = InitializeResult(nonceBytes);
            result.FileName = fileName;

            TimeStampToken token = RequestTimeStamp(tsaService, fileName, GetHashAlgorithm(hashAlg), 
                policy, nonce, certReq, credentials, logger, logExceptions, result);
            result.TimeStampResponse = token;

            return result;
        }

        internal static TimestampResult RequestTimeStamp(string tsaService, 
            byte[] contentToTimestamp, string fileName, string hashAlg, bool certReq, UserCredentials credentials,
            LogDelegate logger, bool logExceptions)
        {
            string policy = "";
            byte[] nonceBytes = GenerateNonceBytes();
            string nonce = BytesToHexString(nonceBytes);

            TimestampResult result = InitializeResult(nonceBytes);
            result.FileBytes = contentToTimestamp;
            result.FileName = fileName;

            TimeStampToken token = RequestTimeStamp(tsaService, contentToTimestamp, GetHashAlgorithm(hashAlg), 
                policy, nonce, certReq, credentials, logger, logExceptions, result);
             result.TimeStampResponse = token;

            return result;
        }

        #region Private memebers
        
        private static TimeStampToken RequestTST(string fileName, string tsaService, Oid hashAlg, string policy, string nonce, bool certReq, UserCredentials credentials, TimestampResult result)
        {
            byte[] nonceBytes = null;
            byte[] hashedMessage = DigestUtils.ComputeDigest(fileName, hashAlg);
            if (!string.IsNullOrEmpty(nonce))
            {
                nonceBytes = HexStringToBytes(nonce);
            }

            Request request = new Request(hashedMessage, hashAlg, nonceBytes, policy, certReq);
            result.TimeStampRequest = request;
            result.Tsq = request.ToByteArray();

            return TimeStampClient.RequestTimeStampToken(tsaService, request, credentials);
        }

        private static TimeStampToken RequestTST(byte[] contentToTimestamp, string tsaService, Oid hashAlg, string policy, string nonce, bool certReq, UserCredentials credentials, TimestampResult result)
        {
            byte[] nonceBytes = null;
            byte[] hashedMessage = DigestUtils.ComputeDigest(contentToTimestamp, hashAlg);
            if (!string.IsNullOrEmpty(nonce))
            {
                nonceBytes = HexStringToBytes(nonce);
            }

            var request = new Request(hashedMessage, hashAlg, nonceBytes, policy, certReq);
            result.TimeStampRequest = request;
            result.Tsq = request.ToByteArray();

            return TimeStampClient.RequestTimeStampToken(tsaService, request, credentials);
        }

        private static TimestampResult InitializeResult(byte[] nonceBytes)
        {
            return new TimestampResult
            {
                Nonce = BytesToLong(nonceBytes)
            };
        }

        private static byte[] HexStringToBytes(string value)
        {
            if (value == null)
            {
                return null;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"\A\b[0-9a-fA-F]+\b\Z"))
            {
                throw new ArgumentException("Nonce field can contain only hex characters");
            }

            if (0 != value.Length % 2)
            {
                value = $"0{value}";
            }

            var bytes = new byte[value.Length / 2];

            for (var i = 0; i < value.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(value.Substring(i, 2), 16);
            }

            return bytes;
        }

        private static long BytesToLong(byte[] value)
        {
            return BitConverter.ToInt64(value, 0);
        }

        private static string BytesToHexString(byte[] value)
        {
            return BitConverter.ToString(value).Replace("-", string.Empty);
        }

        private static byte[] GenerateNonceBytes()
        {
            var nonce = new byte[10];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(nonce);
                return nonce;
            }
        }

        private static Oid GetHashAlgorithm(string hashAlg)
        {
            switch (hashAlg ?? throw new ArgumentNullException(nameof(hashAlg)))
            {
                case "sha1":
                    return Oid.SHA1;
                case "md5":
                    return Oid.MD5;
                case "sha512":
                    return Oid.SHA512;
                case "sha256":
                default:
                    return Oid.SHA256;
            }
        }

        private static TimeStampToken RequestTimeStamp(string tsaAddress, string fileToTimestamp,
            Oid hashAlg, string requestedPolicy, string nonce, bool certReq,
            UserCredentials credentials, LogDelegate logger, bool logExceptions,
            TimestampResult result)
        {
            try
            {
                if (logger == null)
                {
                    return null;
                }

                logger(string.Format("=== {0:" + DateTimeFormat + "} =============================================", DateTime.UtcNow));
                logger($"Requesting time stamp from {tsaAddress}");

                TimeStampToken token = RequestTST(fileToTimestamp, tsaAddress, hashAlg,
                    requestedPolicy, nonce, certReq, credentials, result);
                if (null == token)
                {
                    var msg = "Empty response token";
                    logger(msg);
                    throw new Exception(msg);
                }

                result.Tsr = token.ToByteArray();

                logger("Time stamp successfully received:");

                var timeStampSerNum = BytesToHexString(token.SerialNumber);
                logger($"    Serial number: {timeStampSerNum}");
                result.SerialNumber = timeStampSerNum;

                var localTime = (token.Time).ToLocalTime();
                result.TimeLocal = localTime;
                result.TimeUTC = token.Time;
                logger($"    UTCTime: {token.Time:dd MMM yyyy HH':'mm':'ss}");
                logger($"    LocalTime: {localTime:dd MMM yyyy HH':'mm':'ss}");

                logger("TSA certificate:");
                var issuerName = token.TsaInformation?.TsaCertIssuerName?.Name ?? "";
                result.TSAName = issuerName;
                logger($"    Issuer: {issuerName}");
                logger($"    Serial: {BytesToHexString(token.TsaInformation?.TsaCertSerialNumber ?? new byte[0])}");

                if (null != token.TsaInformation?.TsaCert)
                {
                    logger($"    Subject: {token.TsaInformation.TsaCert.Subject}");
                    logger($"    Valid from: {token.TsaInformation.TsaCert.NotBefore}");
                    logger($"    Valid to: {token.TsaInformation.TsaCert.NotAfter}");
                }

                if (null != token.PolicyOid)
                {
                    result.Policy = token.PolicyOid;
                }

                return token;
            }
            catch (Exception e)
            {
                logger("Error occurred:");
                logger(logExceptions ? e.ToString() : e.Message);

                throw;
            }
        }

        private static TimeStampToken RequestTimeStamp(string tsaAddress, byte[] contentToTimestamp,
            Oid hashAlg, string requestedPolicy, string nonce, bool certReq,
            UserCredentials credentials, LogDelegate logger, bool logExceptions,
            TimestampResult result)
        {
            try
            {
                if (logger == null)
                {
                    return null;
                }

                logger(string.Format("=== {0:" + DateTimeFormat + "} =============================================", DateTime.UtcNow));
                logger($"Requesting time stamp from {tsaAddress}");

                TimeStampToken token = RequestTST(contentToTimestamp, tsaAddress, hashAlg,
                    requestedPolicy, nonce, certReq, credentials, result);
                if (null == token)
                {
                    var msg = "Empty response token";
                    logger(msg);
                    throw new Exception(msg);
                }

                result.Tsr = token.ToByteArray();

                logger("Time stamp successfully received:");

                var timeStampSerNum = BytesToHexString(token.SerialNumber);
                logger($"    Serial number: {timeStampSerNum}");
                result.SerialNumber = timeStampSerNum;

                var localTime = (token.Time).ToLocalTime();
                result.TimeUTC = token.Time;
                result.TimeLocal = localTime;
                logger($"    UTCTime: {token.Time:dd MMM yyyy HH':'mm':'ss}");
                logger($"    Time: {localTime:dd MMM yyyy HH':'mm':'ss}");

                logger("TSA certificate:");
                var issuerName = token.TsaInformation?.TsaCertIssuerName?.Name ?? "";
                result.TSAName = issuerName;
                logger($"    Issuer: {issuerName}");
                logger($"    Serial: {BytesToHexString(token.TsaInformation?.TsaCertSerialNumber ?? new byte[0])}");

                if (null != token.TsaInformation?.TsaCert)
                {
                    logger($"    Subject: {token.TsaInformation.TsaCert.Subject}");
                    logger($"    Valid from: {token.TsaInformation.TsaCert.NotBefore}");
                    logger($"    Valid to: {token.TsaInformation.TsaCert.NotAfter}");
                }

                if (null != token.PolicyOid)
                {
                    result.Policy = token.PolicyOid;
                }

                return token;
            }
            catch (Exception e)
            {
                logger("Error occurred:");
                logger(logExceptions ? e.ToString() : e.Message);

                throw;
            }
        }

        #endregion
    }
}
