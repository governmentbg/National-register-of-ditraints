using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.EPayment
{
    /// <summary>
    /// Съобщение, че заявката за плащане се приема
    /// </summary>
    public class PaymentRequestRejectedModel
    {
        /// <summary>
        /// Времето на обработване на заявката за плащане в ISO 8601 формат.
        /// </summary>
        public DateTime ValidationTime { get; set; }

        /// <summary>
        /// Списък с грешки допуснати в заявката за плащане.
        /// </summary>
        public List<string> Errors { get; set; }
    }
}
