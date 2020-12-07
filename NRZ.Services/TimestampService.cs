using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using NRZ.Data;
using NRZ.Models.Nomenclatures;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using NRZ.Ts.Client;
using NRZ.Ts.Client.Enums;
using NRZ.Ts.Client.Models;
using Polly;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Services
{
    public class TimestampService : BaseService, ITimestampService
    {
        private const Authority DefaultAuthority = Authority.Evotrust;
        private readonly bool RetryTimestampingOnError = true;
        public TimestampService(NRZContext context,
            IStringLocalizer<SharedResources> localizer)
            : base(context, localizer)
        {
        }

        /// <summary>
        /// Връща timestamp от зададен доставичк на услигата.
        /// </summary>
        /// <param name="inpit">Стрингова стойност за подпечатване. Празен стринг,ако е null.</param>
        /// <param name="filePath">Име на файл, който подпечатване. Не е задължително.</param>
        /// <param name="authority">Доставчик на услугата. Стринг от енумерацията <see cref="Authority"/>,
        /// За момента е имплементирана логика само за Authority.Infonotary и Authority.Evotrust.
        /// Ако се подаде друг ще върне null. 
        /// !!! Не е задължително да се подаде. По подразвиране ще използва Evotrust.
        /// Използва retry логика т.е. ако гръмне с единия доставчик ще пробва с другия. !!!</param>
        /// <returns></returns>
        public TimestampResult GetTimeStamp(string inpit, string filePath, string authority)
        {
            byte[] toTimestamp = Encoding.UTF8.GetBytes(inpit ?? "");

            return DoGetTimeStamp(toTimestamp, filePath, authority);
        }

        /// <summary>
        /// Връща timestamp от зададен доставичк на услигата.
        /// </summary>
        /// <param name="objectToStamp">Обект за подпечатване. Ако липсва ще създаде нов object.</param>
        /// <param name="filePath">Име на файл, който подпечатване. Не е задължително.</param>
        /// <param name="authority">Доставчик на услугата. Стринг от енумерацията <see cref="Authority"/>,
        /// За момента е имплементирана логика само за Authority.Infonotary и Authority.Evotrust.
        /// Ако се подаде друг ще върне null. 
        /// !!! Не е задължително да се подаде. По подразвиране ще използва Evotrust.
        /// Използва retry логика т.е. ако гръмне с единия доставчик ще пробва с другия. !!!</param>
        /// <returns></returns>
        public TimestampResult GetTimeStamp(object objectToStamp, string filePath, string authority)
        {
            string output = JsonConvert.SerializeObject(objectToStamp ?? new object());
            byte[] toTimestamp = new byte[output.Length * sizeof(char)];
            Buffer.BlockCopy(output.ToCharArray(), 0, toTimestamp, 0, toTimestamp.Length);

            return DoGetTimeStamp(toTimestamp, filePath, authority);
        }

        /// <summary>
        /// Валидация на timestamp. Още не е импленетирано както трябва.
        /// </summary>
        /// <param name="tsr"></param>
        /// <returns></returns>
        public string Validate(byte[] tsr)
        {
            var generator = new TimestampGenerator(DefaultAuthority);

            return generator.Validate(tsr);
        }

        private TimestampResult DoGetTimeStamp(byte[] contentToStamp, string filePath, string authority)
        {
            var usedPrividers = new HashSet<Authority>();
            Authority? authorityEnum = null;
            if (!string.IsNullOrWhiteSpace(authority))
            {
                try
                {
                    authorityEnum = (Authority)Enum.Parse(typeof(Authority), authority);
                }
                catch
                {

                }
            }

            usedPrividers.Add(authorityEnum ?? DefaultAuthority);

            var tsGenerator = new TimestampGenerator(authorityEnum ?? DefaultAuthority);
            if (RetryTimestampingOnError)
            {
                // retry механизъм
                var policy = Policy.Handle<Exception>()
                 .Retry(2, onRetry: (exception, retryCount, context) =>
                 {
                     if (false == usedPrividers.Contains(Authority.Evotrust))
                     {
                         tsGenerator.ChangeAutorithyInfo(Authority.Evotrust);
                         usedPrividers.Add(Authority.Evotrust);
                     }
                     else if (false == usedPrividers.Contains(Authority.Infonotary))
                     {
                         tsGenerator.ChangeAutorithyInfo(Authority.Infonotary);
                         usedPrividers.Add(Authority.Infonotary);
                     }
                 });

                var timestampResult = policy.Execute(() => tsGenerator.Generate(contentToStamp, filePath));
                return timestampResult;
            }
            else
            {
                var timestampResult = tsGenerator.Generate(contentToStamp, filePath);
                return timestampResult;
            }
        }
    }
}
