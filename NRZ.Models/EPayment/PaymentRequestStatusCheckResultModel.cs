
using System.Collections.Generic;

namespace NRZ.Models.EPayment
{
    public class PaymentRequestStatusCheckResultModel
    {
        public IList<PaymentRequestStatusChangeModel> Statuses { get; set; }

        /// <summary>
        /// Gets a value that indicates if the HTTP response was successful.
        /// A value that indicates if the HTTP response was successful. 
        /// true if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise false.
        /// </summary>
        public bool IsSuccessStatusCode { get; set; }

        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
