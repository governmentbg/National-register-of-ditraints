using Hangfire;
using NRZ.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.Notifications.Job
{
    public class PaymentRequestSendingJob : IPaymentRequestSendingJob
    {
        private readonly IEPaymentJobService _ePaymentJobService;
        public PaymentRequestSendingJob(IEPaymentJobService ePaymentJobService)
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
            await _ePaymentJobService.SendAllUnsentPaymentRequestsAsync();
        }
    }
}
