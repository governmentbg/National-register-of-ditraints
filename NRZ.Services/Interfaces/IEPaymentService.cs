using NRZ.Data;
using NRZ.Models.EPayment;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IEPaymentService
    {
        Task<EServicesSettingsModel> LoadSettingsAsync();
        Task ChangeSettingsAsync(EServicesSettingsModel model);
        Task<EServicePaymentRequestCreateModel> GeneratePaymentRequestAsync(Shared.Enums.EServiceType serviceType, int requestId);
        Task<PaymentRequest> GetPaymentRequestAsync(int id);
        Task<EservicePaymentRequest> GetServicePaymentRequestAsync(int paymentRequestId);
        Task ProcessPaymentRequestSendResult(EservicePaymentRequest request, string jsonResult);
        Task ChangePaymentRequestStatusAsync(PaymentRequestStatusChangeModel request);
        Task SetPaymentRequestErrorAsync(EservicePaymentRequest request, string error);
        IQueryable<EServicePaymentRequestModel> GetAll();
        IQueryable<EServicePaymentStatusHistoryModel> GetPaymentRequestHistory(int requestId);

        /// <summary>
        /// Изпращане на заявка за плащане към ПОРТАЛ ЗА ЕЛЕКТРОННИ ПЛАЩАНИЯ.
        /// </summary>
        /// <param name="model">Модел с данни за изпращане. Dummy данни има в EPaymentService.GetTestData. Там може да се види и какво се очаква.</param>
        /// <param name="isTestEnv">Определя дали ще се ползват настройките за тестовата среда в appsetting.json, секция EPaymentSettings.</param>
        /// <returns></returns>
        Task<PaymentRequestSendResultModel> SendEpaymentRequestAsync(EPaymentRequestModel model);
        Task<PaymentRequestSendResultModel> TestPaymentRequestAsync();

        /// <summary>
        /// Запитване за статус на заявки за плащане по идентификатор.
        /// Ако в средата за елемент от списъка не съществува регистрирана заявка за плащане 
        /// с дадено id услугата ще върне празен стринг в полетата status и changeTime.
        /// Errors: няма.
        /// </summary>
        /// <param name="ids">Идентификатори на заявките за плащане.</param>
        /// <param name="isTestEnv">Определя дали ще се ползват настройките за тестовата среда в appsetting.json, секция EPaymentSettings.</param>
        /// <returns>Услугата по списък с идентификатори връща списък със статуси на заявки за плащане.</returns>
        Task<PaymentRequestStatusCheckResultModel> PaymentRequestsStatusCheckAsync(IList<int> ids);

        /// <summary>
        /// Отказване на заявка за плащане.
        /// Response Content: Няма.
        /// Response Content Description: Няма.
        /// Errors: При невалиден идентификатор на заявка за плащане (id) се връща грешка HTTP 400 BAD REQUEST.
        /// </summary>
        /// <param name="id">Идентификатор на заявка за плащане</param>
        /// <param name="isTestEnv">Определя дали ще се ползват настройките за тестовата среда в appsetting.json, секция EPaymentSettings.</param>
        /// <returns></returns>
        Task<PaymentRequestRefusalResultModel> PaymentRequestRefusalAsync(int id);
    }
}
