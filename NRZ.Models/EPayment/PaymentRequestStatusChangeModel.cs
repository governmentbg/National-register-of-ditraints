using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.EPayment
{
    public class PaymentRequestStatusChangeModel
    {
        /// <summary>
        /// Идентификатор на заявка за плащане.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Статус на заявката за плащане, възможните стойности са: 
        /// “PENDING” (Очаква плащане), 
        /// “AUTHORIZED” (Получена е авторизация от виртуалния ПОС терминал), 
        /// “ORDERED” (Плащането е наредено), 
        /// “PAID” (Плащането е получено по сметката на доставчика), 
        /// “EXPIRED” (Заявката за плащане е изтекла), 
        /// “CANCELED” (Заявката за плащане е отказана от потребителя), 
        /// “SUSPENDED” (Заявката за плащане е отказана от АИС).
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Време на промяна на статуса на заявката за плащане в ISO 8601формат.
        /// </summary>
        public DateTime? ChangeTime { get; set; }
    }
}
