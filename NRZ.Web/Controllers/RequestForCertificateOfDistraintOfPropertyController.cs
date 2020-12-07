using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NRZ.Data;
using NRZ.Models.EPayment;
using NRZ.Models.Identity;
using NRZ.Models.RequestForCertificateOfDistraintOfProperty;
using NRZ.Models.Settings;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using NRZ.Web.Attributes;
using NRZ.Web.Extensions;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestForCertificateOfDistraintOfPropertyController : BaseApiController
    {
        private readonly IRequestForCertificateOfDistraintOfPropertyService _requestForCertificateOfDistraintOfPropertyService;
        private readonly IEmailService _emailService;
        private readonly EPaymentSettings _settings;

        public RequestForCertificateOfDistraintOfPropertyController(
            IRequestForCertificateOfDistraintOfPropertyService requestForCertificateOfDistraintOfPropertyService,
            IEmailService emailService,
            IOptions<EPaymentSettings> config,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<SharedResources> localizer
            )
            : base(userManager, localizer)
        {
            _requestForCertificateOfDistraintOfPropertyService = requestForCertificateOfDistraintOfPropertyService;
            _emailService = emailService;
            _settings = config.Value;
        }

        [HttpPost("create")]
        [Audit]
        public async Task<IActionResult> Create([FromBody] RequestModel model)
        {
            if (!ModelState.IsValid)
            {
                string msg = $"{_localizer.GetString("Invalid RequestModel").Value}!<br />{ModelState.GetErrors()}";
                return BadRequest(msg);
            }

            try
            {
                (bool hasToPay, PaymentRequestSendResultModel paymentRequestModel, RequestForCertificateOfDistraintOfProperty request) = 
                    await _requestForCertificateOfDistraintOfPropertyService.CreateAsync(model, CurrentUserId);
                string message = "";
                if (paymentRequestModel != null)
                {
                    string email = model?.Email;
                    if (string.IsNullOrWhiteSpace(email) == false)
                    {
                        string subject = "Регистрация на заявка в системата за електронно плащане";
                        string body = message = paymentRequestModel.IsSuccessStatusCode == false
                            ? $"Неуспешна регистрация на заявка в системата за електронно плащане. За детайли посетете платформата за електронно плащане на адрес: {(_settings.UseTestEnv ? "https://pay-test.egov.bg/" : "https://pay.egov.bg/")}"
                            : (paymentRequestModel.UnacceptedReceipt != null
                                ? $"Регистрация на заявка в системата за електронно плащане е отхвърлена. За детайли посетете платформата за електронно плащане на адрес: {(_settings.UseTestEnv ? "https://pay-test.egov.bg/" : "https://pay.egov.bg/")}"
                                : $"Успешна регистрация на заявка с ИД №: {paymentRequestModel.AcceptedReceipt?.Id} в системата за електронно плащане. За детайли и плащане посетете платформата за електронно плащане на адрес: {(_settings.UseTestEnv ? "https://pay-test.egov.bg/" : "https://pay.egov.bg/")}"
                                );
                        _emailService.SendEmail(email, null, null, subject, body);
                    }
                }

                return Ok(request.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + " / " + e.InnerException?.Message);
            }
        }

        [HttpGet("list")]
        [Audit]
        public IActionResult List()
        {
            try
            {
                var requests = _requestForCertificateOfDistraintOfPropertyService.GetAll();
                return Ok(requests);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
