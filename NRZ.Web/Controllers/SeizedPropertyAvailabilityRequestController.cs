using Microsoft.AspNetCore.Mvc;
using NRZ.Models.SeizedPropertyAvailability;
using NRZ.Services.Interfaces;
using System.Threading.Tasks;
using Serilog;
using System.Collections;
using System.Collections.Generic;
using NRZ.Models.EPayment;
using NRZ.Models.Settings;
using Microsoft.Extensions.Options;

namespace NRZ.Web.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SeizedPropertyAvailabilityRequestController : BaseApiController
    {
        private readonly ISeizedPropertyAvailabilityRequestService _service;
        private readonly IEmailService _emailService;
        private readonly EPaymentSettings _settings;

        public SeizedPropertyAvailabilityRequestController(
            ISeizedPropertyAvailabilityRequestService service,
            IEmailService emailService,
            IOptions<EPaymentSettings> config)
        {
            _service = service;
            _emailService = emailService;
            _settings = config.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var requests = await _service.GetAll();
                return Ok(requests);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("Report/{distraintId}")]
        public async Task<IActionResult> GetReport(int distraintId)
        {
            try
            {
                var report = await _service.GetReport(distraintId);
                return Ok(report);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAircraftInfo/{distraintId}")]
        public async Task<IActionResult> GetAircraftInfo(int distraintId)
        {
            // TODO: If user is authorized or has the guid for paid request
            try
            {
                var aircraftInfo = await _service.GetAircrafInfo(distraintId);
                return Ok(aircraftInfo);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetVehicleInfo/{distraintId}")]
        public async Task<IActionResult> GetVehicleInfo(int distraintId)
        {
            // TODO: If user is authorized or has the guid for paid request
            try
            {
                var vehicleInfo = await _service.GetVehicleInfo(distraintId);
                return Ok(vehicleInfo);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetVehicleInfo/{distraintId}")]
        public async Task<IActionResult> GetVesselInfo(int distraintId)
        {
            // TODO: If user is authorized or has the guid for paid request
            try
            {
                var vesselInfo = await _service.GetVesselInfo(distraintId);
                return Ok(vesselInfo);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetRealEstateInfo/{distraintId}")]
        public async Task<IActionResult> GetRealEstateInfo(int distraintId)
        {
            // TODO: If user is authorized or has the guid for paid request
            try
            {
                var realEstateInfo = await _service.GetRealEstateInfo(distraintId);
                return Ok(realEstateInfo);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAgriformachineryInfo/{distraintId}")]
        public async Task<IActionResult> GetAgriformachineryInfo(int distraintId)
        {
            // TODO: If user is authorized or has the guid for paid request
            try
            {
                var model = await _service.GetAgriformachineryInfo(distraintId);
                return Ok(model);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetOtherInfo/{distraintId}")]
        public async Task<IActionResult> GetOtherInfo(int distraintId)
        {
            // TODO: If user is authorized or has the guid for paid request
            try
            {
                var other = await _service.GetOtherInfo(distraintId);
                return Ok(other);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(SeizedPropertyAvailabilityRequestModel model)
        {
            try
            {
                // TODO: UserId
                (bool hasToPay, PaymentRequestSendResultModel paymentRequestModel, IEnumerable<SeizedPropertyAvailabilityResultModel> searchModels) = await _service.Search(model, CurrentUserId);
                string message = "";
                if (paymentRequestModel != null)
                {
                    string email = model?.Requester?.Email;
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

                bool isSuccessful = false;
                if (paymentRequestModel != null && paymentRequestModel.IsSuccessStatusCode)
                {
                    isSuccessful = true;
                }

                return Ok(new { items = searchModels, message, isSuccessfull = isSuccessful });
            }
            catch(System.Exception ex)
            {
                Log.Error(ex, $"ERROR searching for seized property availability report");
                return BadRequest("Error searching for seized property availability report");
            }
        }

        [HttpGet("GetPaidReport/{paidGuid}")]
        public async Task<IActionResult> GetPaidReport(string paidGuid)
        {
            try
            {
                var report = await _service.GetPaidReport(paidGuid);
                return Ok(report);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}