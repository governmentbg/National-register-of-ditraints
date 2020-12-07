using Hangfire;
using NRZ.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace NRZ.Services.Notifications.Job
{
    public class PaymentRequestStatusCheckJob : IPaymentRequestStatusCheckJob
    {
        private readonly IEPaymentJobService _ePaymentJobService;

        public PaymentRequestStatusCheckJob(IEPaymentJobService ePaymentJobService)
        {
            _ePaymentJobService = ePaymentJobService;
        }
        public async Task Run(IJobCancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await RunAtTimeOf(DateTime.UtcNow);
        }

        public async Task RunAtTimeOf(DateTime now)
        {
            //Do some work
            await _ePaymentJobService.UpdateStatusesAsync();
        }
    }
}
