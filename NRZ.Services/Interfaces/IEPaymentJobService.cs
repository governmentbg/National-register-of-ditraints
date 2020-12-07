
using NRZ.Models.EPayment;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IEPaymentJobService
    {
        Task<string> SendPaymentRequest(PaymentRequestModel paymentRequest);
        Task SendAllUnsentPaymentRequestsAsync();
        Task UpdateStatusesAsync();
    }
}
