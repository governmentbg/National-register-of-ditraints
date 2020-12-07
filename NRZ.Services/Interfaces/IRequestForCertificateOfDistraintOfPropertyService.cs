using NRZ.Data;
using NRZ.Models.EPayment;
using NRZ.Models.RequestForCertificateOfDistraintOfProperty;
using System.Linq;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IRequestForCertificateOfDistraintOfPropertyService
    {
        Task<(bool hasToPay, PaymentRequestSendResultModel paymentRequestModel, RequestForCertificateOfDistraintOfProperty request)> CreateAsync(RequestModel model, string currentUserId);

        IQueryable<RequestViewModel> GetAll();
    }
}
