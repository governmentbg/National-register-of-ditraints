namespace NRZ.Models.EPayment
{
    public class PaymentRequestSendResultModel
    {
        /// <summary>
        /// acceptedReceiptJson – JSON документ от тип "СЪОБЩЕНИЕ, ЧЕ ЗАЯВКАТА ЗА ПЛАЩАНЕ СЕ ПРИЕМА" (има стойност само ако заявката е успешно приета).
        /// </summary>
        public PaymentRequestAcceptedModel AcceptedReceipt { get; set; }

        /// <summary>
        /// unacceptedReceiptJson - JSON документ от тип "СЪОБЩЕНИЕ, ЧЕ ЗАЯВКАТА ЗА ПЛАЩАНЕ НЕ СЕ ПРИЕМА" (има стойност само ако заявката не е приета).
        /// </summary>
        public PaymentRequestRejectedModel UnacceptedReceipt { get; set; }

        /// <summary>
        /// Gets a value that indicates if the HTTP response was successful.
        /// A value that indicates if the HTTP response was successful. 
        /// true if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise false.
        /// </summary>
        public bool IsSuccessStatusCode { get; set; }

        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; }

        public string EpaymentUrl { get; set; }
    }
}
