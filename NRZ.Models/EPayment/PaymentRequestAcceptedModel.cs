using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.EPayment
{
    /// <summary>
    /// Съобщение, че заявката за плащане не се приема
    /// </summary>
    public class PaymentRequestAcceptedModel
    {
        /// <summary>
        /// Идентификатор на заявка за плащане
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Време на регистрация на заявката за плащане ISO 8601 формат
        /// </summary>
        public DateTime RegistrationTime { get; set; }
    }
}
