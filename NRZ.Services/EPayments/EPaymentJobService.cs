using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NRZ.Data;
using NRZ.Data.Extensions;
using NRZ.Models.EPayment;
using NRZ.Models.Settings;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using System.Collections.Generic;

namespace NRZ.Services.EPayments
{
    public class EPaymentJobService : BaseService, IEPaymentJobService
    {
        private readonly IEPaymentService _ePaymentService;
        private readonly EPaymentSettings _settings;

        public EPaymentJobService(NRZContext context,
            IEPaymentService ePaymentService,
            IOptions<EPaymentSettings> config,
            IStringLocalizer<SharedResources> localizer = null)
            : base(context, localizer)
        {
            _ePaymentService = ePaymentService;
            _settings = config.Value;
        }

        public IQueryable<EservicePaymentRequest> All()
        {
            return _context.EservicePaymentRequest
                .Include(x => x.PaymentRequest);
        }

        public IQueryable<EservicePaymentRequest> UnsentPaymentRequests()
        {
            return All()
                .Where(x => x.StatusCode == Shared.Enums.PaymentRequestStatus.NEW.ToString());
        }

        public async Task SendAllUnsentPaymentRequestsAsync()
        {
            if (string.IsNullOrWhiteSpace(_settings.SendJsonPaymentRequestUri))
            {
                return;
            }

            var unsentRequests = await UnsentPaymentRequests()
                .ToListAsync();

            foreach (EservicePaymentRequest request in unsentRequests)
            {
                using (var tran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        string result = await SendPaymentRequest(request.PaymentRequest.ToModel());
                        await _ePaymentService.ProcessPaymentRequestSendResult(request, result);

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Log.Error(ex, $"ERROR sending payment requests");
                        try
                        {
                            await _ePaymentService.SetPaymentRequestErrorAsync(request, ex.Message);
                        }
                        catch
                        { }
                        throw ex;
                    }
                }
            }

        }


        public async Task<string> SendPaymentRequest(PaymentRequestModel paymentRequest)
        {
            string modelStr = JsonConvert.SerializeObject(paymentRequest, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            var client = new HttpClient();
            var uri = _settings.SendJsonPaymentRequestUri;
            Log.Information($"EPaymentJobService/SendPaymentRequest - uri: {uri}");
            HttpContent content = new StringContent(modelStr, Encoding.UTF8, "application/json");
            var request = CreatePaymentHttpRequest(HttpMethod.Post, uri, content);
            var response = await client.SendAsync(request);
            var jsonString = await response.Content.ReadAsStringAsync();

            Log.Information($"EPaymentJobService/SendPaymentRequest - result: {jsonString}");
            Log.Information($"EPaymentJobService/SendPaymentRequest - result code: {response.StatusCode}");
            if (response.IsSuccessStatusCode)
                return jsonString;
            else
                return null;
        }

        private HttpRequestMessage CreatePaymentHttpRequest(HttpMethod method, string uri, HttpContent content)
        {
            var request = new HttpRequestMessage(method, uri);
            request.Content = content;
            request.Content.Headers.Add("clientId", _settings.ClientId);
            request.Content.Headers.Add("secretKey", _settings.SecretKey);

            return request;
        }

        public async Task UpdateStatusesAsync()
        {
            // Всички в EServicePaymentRequest, който имат Id от регистрация в сустемата на ел.плащане
            // и имат статус различен от ERROR, PAID, CANCELED, EXPIRED, SUSPENDED

            var exceptHash = new HashSet<string>(new List<string> { "ERROR", "PAID", "CANCELED", "EXPIRED", "SUSPENDED" });
            var entities = await _context.EservicePaymentRequest
                .AsNoTracking()
                .Where(x => x.AisPaymentId != null && x.StatusCode != null && !exceptHash.Contains(x.StatusCode))
                .Select(x => new { x.AisPaymentId, x.StatusCode } )
                .ToListAsync();

            var ids = new HashSet<int>(entities.Where(x => x.AisPaymentId.HasValue).Select(x => x.AisPaymentId.Value));
            PaymentRequestStatusCheckResultModel checkResults = await _ePaymentService.PaymentRequestsStatusCheckAsync(ids.ToList());
            if (checkResults?.Statuses != null)
            {
                var toChage = checkResults.Statuses.Where(x => !entities.Any(e => e.AisPaymentId == x.Id && e.StatusCode.Equals(x.Status, StringComparison.OrdinalIgnoreCase)));
                foreach (PaymentRequestStatusChangeModel item in toChage)
                {
                    await _ePaymentService.ChangePaymentRequestStatusAsync(item);
                }
            }
        }
    }
}
