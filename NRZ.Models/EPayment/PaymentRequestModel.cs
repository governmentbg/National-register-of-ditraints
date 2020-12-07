using System;

namespace NRZ.Models.EPayment
{
    public class PaymentRequestModel
    {
        /// <summary>
        /// Идентификатор на задължението за плащане в АИС на доставчика на ЕАУ.
        /// Когато aisPaymentId в JSON документа от тип "ЗАЯВКА ЗА ПЛАЩАНЕ" съдържа стойност, 
        /// която съвпада с вече регистрирана заявка за плащане със статус PENDING, 
        /// не се създава нова заявка за плащане, а се актуализират данните с подадените. 
        /// Ако статусът е различен от PENDING заявката не се приема.
        /// </summary>
        public int AisPaymentId { get; set; }

        /// <summary>
        /// Доставчик на ЕАУ.
        /// Задължително поле.
        /// </summary>
        public string ServiceProviderName { get; set; }

        /// <summary>
        /// Име на банката, в която е сметката на доставчика на ЕАУ.
        /// Задължително поле.
        /// </summary>
        public string ServiceProviderBank { get; set; }

        /// <summary>
        /// BIC код на сметката на доставчика на ЕАУ.
        /// Задължително поле.
        /// </summary>
        public string ServiceProviderBic { get; set; }

        /// <summary>
        /// IBAN код на сметката на доставчика на ЕАУ.
        /// Задължително поле.
        /// </summary>
        public string ServiceProviderIban { get; set; }

        /// <summary>
        /// Валута в която се плаща задължението (три символа, пр. "BGN").
        /// Задължително поле.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Код на плащане. Да се разбере какъв може да бъде. Примерно "1"
        /// </summary>
        public string PaymentTypeCode { get; set; }

        /// <summary>
        /// Сума на задължението (десетичен разделител ".", до 2 символа след десетичния разделител, пр. "2.33").
        /// Задължително поле.
        /// </summary>
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// Основание за плащане.
        /// Задължително поле.
        /// </summary>
        public string PaymentReason { get; set; }

        /// <summary>
        /// Тип на идентификатора на задължено лице ("1", "2" или "3" -> ЕГН = 1, ЛНЧ = 2, БУЛСТАТ = 3).
        /// Задължително поле.
        /// </summary>
        public int ApplicantUinTypeId { get; set; }

        /// <summary>
        /// Идентификатор на задължено лице.
        /// Задължително поле.
        /// </summary>
        public string ApplicantUin { get; set; }

        /// <summary>
        /// Име на задължено лице.
        /// Задължително поле.
        /// </summary>
        public string ApplicantName { get; set; }

        /// <summary>
        /// Тип на документ (референтен документ за плащане).
        /// Да се разбере какъв може да бъде.
        /// </summary>
        public string PaymentReferenceType { get; set; }

        /// <summary>
        /// Номер на документ (референтен документ за плащане).
        /// Задължително поле.
        /// </summary>
        public string PaymentReferenceNumber { get; set; }

        /// <summary>
        /// Дата на документ (референтен документ за плащане).
        /// Задължително поле.
        /// Всички дати са в ISO 8601 формат.
        /// </summary>
        public DateTime PaymentReferenceDate { get; set; }

        /// <summary>
        /// Дата на изтичане на заявката за плащане.
        /// Задължително поле.
        /// Всички дати са в ISO 8601 формат.
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Допълнителна информация
        /// </summary>
        public string AdditionalInformation { get; set; }
        /// <summary>
        /// УРИ на ЕАУ.
        /// </summary>
        public string AdministrativeServiceUri { get; set; }
        /// <summary>
        /// УРИ на доставчик на ЕАУ.
        /// </summary>
        public string AdministrativeServiceSupplierUri { get; set; }
        /// <summary>
        /// URL за нотификации при смяна на статус на задължение.
        /// </summary>
        public string AdministrativeServiceNotificationUrl { get; set; }
    }
}
