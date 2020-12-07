using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NRZ.Models.Identity;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using NRZ.Web.Attributes;
using Serilog;
using System;
using System.Threading.Tasks;
using NRZ.Shared;
using NRZ.Models.EPayment;
using System.Collections.Generic;
using NRZ.Web.Extensions;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EPaymentController : BaseApiController
    {
        private readonly IEPaymentService _ePaymentService;

        public EPaymentController(UserManager<ApplicationUser> userManager,
            IStringLocalizer<SharedResources> localizer,
            IEPaymentService ePaymentService

            )
            : base(userManager, localizer)
        {
            _ePaymentService = ePaymentService;
        }

        [HttpGet("list")]
        [Audit]
        public IActionResult List()
        {
            try
            {
                var list = _ePaymentService.GetAll();
                return Ok(list);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR getting list with e-payments by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error getting list with e-payments");
            }
        }

        [HttpGet("getPaymentRequestHistory/{requestId}")]
        [Audit]
        public IActionResult GetPaymentRequestHistory(int requestId)
        {
            try
            {
                var list = _ePaymentService.GetPaymentRequestHistory(requestId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR getting payment request history by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error getting payment request history");
            }
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<IActionResult> TestPaymentRequest()
        {
            try
            {
                PaymentRequestSendResultModel result = await _ePaymentService.TestPaymentRequestAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                string msg = e.GetInnerMostException()?.Message ?? "";
                Log.Error(e, msg);
                return BadRequest(msg);
            }
        }

        [AllowAnonymous]
        [HttpGet("statusCheck")]
        public async Task<IActionResult> TestPaymentRequestStatusCheck([FromQuery] int? id)
        {
            try
            {
                PaymentRequestStatusCheckResultModel result = await _ePaymentService.PaymentRequestsStatusCheckAsync(new List<int>() { id ?? throw new ArgumentNullException(nameof(id)) });
                return Ok(result);
            }
            catch (Exception e)
            {
                string msg = e.GetInnerMostException()?.Message ?? "";
                Log.Error(e, msg);
                return BadRequest(msg);
            }
        }

        [AllowAnonymous]
        [HttpGet("requestRefusal")]
        public async Task<IActionResult> TestPaymentRequestRefusal([FromQuery] int? id)
        {
            try
            {
                PaymentRequestRefusalResultModel result = await _ePaymentService.PaymentRequestRefusalAsync(id ?? throw new ArgumentNullException(nameof(id)));
                return Ok(result);
            }
            catch (Exception e)
            {
                string msg = e.GetInnerMostException()?.Message ?? "";
                Log.Error(e, msg);
                return BadRequest(msg);
            }
        }



        [HttpGet("loadSettings")]
        [Audit]
        public async Task<IActionResult> LoadSettings()
        {
            try
            {
                var settings = await _ePaymentService.LoadSettingsAsync();
                return Ok(settings);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR getting eServices settings by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error getting eServices settings");
            }
        }


        [HttpPost("changeEServiceSettings")]
        [Audit]
        public async Task<IActionResult> ChangeEServiceSettings([FromBody] EServicesSettingsModel model)
        {
            if (!ModelState.IsValid)
            {
                string msg = $"{_localizer.GetString("Invalid EServicesSettingsModel").Value}!<br />{ModelState.GetErrors()}";
                Log.Information($"EPaymentController/ChangeEServiceSettings/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return BadRequest(msg);
            }

            try
            {
                await _ePaymentService.ChangeSettingsAsync(model);

                string msg = $"{_localizer.GetString("Edit success").Value}!";
                Log.Information($"EPaymentController/ChangeEServiceSettings/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR changing eService settings by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error changing eService settings");
            }
        }

    }
}
