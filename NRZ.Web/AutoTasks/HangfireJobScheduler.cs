using Hangfire;
using Microsoft.Extensions.Options;
using NRZ.Models.Settings;
using NRZ.Services.Notifications.Job;
using Microsoft.Extensions.DependencyInjection;
using System;
using NRZ.Web.Services;

namespace NRZ.Web.AutoTasks
{
    public class HangfireJobScheduler
    {
        public static void ScheduleRecurringJobs(IServiceProvider serviceProvider)
        {
            IOptions<HangFireJobSettings> config = serviceProvider.GetService<IOptions<HangFireJobSettings>>();
            HangFireJobSettings settings = config?.Value;

            var paymentRequestSendingJobMinutesInterval = settings?.PaymentRequestSendingJobMinutesInterval ?? 5;

            // Every 5 minutes
            RecurringJob.RemoveIfExists(nameof(PaymentRequestSendingJob));
            RecurringJob.AddOrUpdate<PaymentRequestSendingJob>(nameof(PaymentRequestSendingJob),
                job => job.Run(JobCancellationToken.Null),
                $"*/{paymentRequestSendingJobMinutesInterval} * * * *", TimeZoneInfo.Utc);

            var auctionEndCheckJobMinutesInterval = settings?.AuctionEndCheckJobMinutesInterval ?? 1;
            // Every 1 minute
            RecurringJob.RemoveIfExists(nameof(AuctionServiceJob));
            RecurringJob.AddOrUpdate<AuctionServiceJob>(nameof(AuctionServiceJob),
                job => job.Run(JobCancellationToken.Null),
                $"*/{auctionEndCheckJobMinutesInterval} * * * *", TimeZoneInfo.Utc);

            var paymentRequestStatusCheckJobMinutesInterval = settings?.PaymentRequestStatusCheckJobMinutesInterval ?? 2;
            // Every 2 minute
            RecurringJob.RemoveIfExists(nameof(PaymentRequestStatusCheckJob));
            RecurringJob.AddOrUpdate<PaymentRequestStatusCheckJob>(nameof(PaymentRequestStatusCheckJob),
                job => job.Run(JobCancellationToken.Null),
                $"*/{paymentRequestStatusCheckJobMinutesInterval} * * * *", TimeZoneInfo.Utc);
        }
    }
}
