using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NRZ.Models.EPayment;
using NRZ.Models.Property;
using NRZ.Services.Interfaces;
using NRZ.Ts.Client.Models;
using NRZ.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;
using NRZ.Data;
using NRZ.Data.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntegrationController : ControllerBase
    {
        private readonly IIntegrationService integrationService;
        private readonly ITimestampService timestampService;
        private readonly IEPaymentService paymentService;
        private readonly IEPaymentJobService paymentJobService;

        public IntegrationController(IIntegrationService integrationService, 
            ITimestampService timestampService, 
            IEPaymentService paymentService,
            IEPaymentJobService paymentJobService)
        {
            this.integrationService = integrationService;
            this.timestampService = timestampService;
            this.paymentService = paymentService;
            this.paymentJobService = paymentJobService;
        }

        [HttpPost]
        [Audit]
        [Route("TestConnectionToRegiXVehicle")]
        public async Task<IActionResult> TestConnectionToRegiXVehicle()
        {
            try
            {
                PropertySearchResultModel result = await integrationService.TestConnectionToRegiXVehicle();
                Log.Information($"IntegrationController/TestConnectionToRegiXVehicle - tested connection");

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR testing the connection to RegiX Vehicle");
                return BadRequest("Error testing the connection to RegiX Vehicle");
            }
        }

        [HttpPost]
        [Audit]
        [Route("TestConnectionToRegiXVessels")]
        public async Task<IActionResult> TestConnectionToRegiXVessels()
        {
            try
            {
                PropertySearchResultModel result = await integrationService.TestConnectionToRegiXVessel();
                Log.Information($"IntegrationController/TestConnectionToRegiXVessel - tested connection");

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR testing the connection to RegiX Vessel");
                return BadRequest("Error testing the connection to RegiX Vessel");
            }
        }

        [HttpPost]
        [Audit]
        [Route("TestConnectionToRegiXAircrafts")]
        public async Task<IActionResult> TestConnectionToRegiXAircrafts()
        {
            try
            {
                PropertySearchResultModel result = await integrationService.TestConnectionToRegiXAircraft();
                Log.Information($"IntegrationController/TestConnectionToRegiXAircrafts - tested connection");

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR testing the connection to RegiX Aircrafts");
                return BadRequest("Error testing the connection to RegiX Aircrafts");
            }
        }

        [HttpGet]
        [Audit]
        [Route("TestTimestamp")]
        public IActionResult TestTimestamp([FromQuery] string authority)
        {
            TimestampResult result;

            try
            {
                result = timestampService.GetTimeStamp(new { Title = "This is timestamp test", Time = DateTime.Now }, "NoFile.json", authority);
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, x.Message);
            }

            return Ok(result);
        }

        [HttpPost]
        [Audit]
        [Route("newPaymentRequestForCertificate")]
        public async Task<IActionResult> GeneratePaymentRequestForSeizedPropertyCertificate(int requestId)
        {
            try
            {
                if (requestId <= 0)
                    return BadRequest("Missing certificate request");

                EServicePaymentRequestCreateModel request = await paymentService.GeneratePaymentRequestAsync(Shared.Enums.EServiceType.SEIZEDPROPERTYCERTIFICATE, requestId);
                Log.Information($"IntegrationController/newPaymentRequestForCertificate - created request with Id: {request.Id}");
                return Ok(request);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR generating payment request for seized property certificate");
                return BadRequest("Error generating payment request for seized property certificate");
            }
        }

        [HttpPost]
        [Audit]
        [Route("newPaymentRequestForReport")]
        public async Task<IActionResult> GeneratePaymentRequestForSeizedPropertyReport(int requestId)
        {
            try
            {
                if (requestId <= 0)
                    return BadRequest("Missing report request");

                EServicePaymentRequestCreateModel request = await paymentService.GeneratePaymentRequestAsync(Shared.Enums.EServiceType.SEIZEDPROPERTYBYOWNERREPORT, requestId);
                Log.Information($"IntegrationController/newPaymentRequestForReport - created request with Id: {request.Id}");
                return Ok(request);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR generating payment request for seized property report");
                return BadRequest("Error generating payment request for seized property report");
            }
        }


        [HttpGet]
        [Audit]
        [Route("getJsonPaymentRequest")]
        public async Task<IActionResult> GetJsonPaymentRequest(int id)
        {
            try
            {
                PaymentRequest request = await paymentService.GetPaymentRequestAsync(id);
                if (request == null)
                    return BadRequest("Request not found!");

                string modelStr = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                Log.Information($"IntegrationController/getJsonPaymentRequest - got request with Id: {id} / {modelStr}");
                return Ok(modelStr);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR getting json payment request");
                return BadRequest("Error getting json payment request");
            }
        }

        [HttpPost]
        [Audit]
        [Route("sendPaymentRequest")]
        public async Task<IActionResult> SendPaymentRequest(int id)
        {
            try
            {
                EservicePaymentRequest request = await paymentService.GetServicePaymentRequestAsync(id);
                if (request == null)
                    return BadRequest("Request not found!");

                string result = await paymentJobService.SendPaymentRequest(request.PaymentRequest.ToModel());
                await paymentService.ProcessPaymentRequestSendResult(request, result);
                Log.Information($"IntegrationController/sendPaymentRequest - sent request with Id: {id} / {result}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR sending payment request");
                return BadRequest("Error sending payment request");
            }
        }

        [HttpPost]
        [Audit]
        [Route("receivePaymentRequest")]
        public async Task<IActionResult> ReceivedPaymentRequest([FromBody] PaymentRequestModel content)
        {
            try
            {
                string clientId = Request.Headers["clientId"];
                string secretKey = Request.Headers["secretKey"];

                if (String.IsNullOrWhiteSpace(clientId) || !String.Equals(clientId, "testAisClient"))
                {
                    var error = new
                    {
                        acceptedReceiptJson = "",
                        unacceptedReceiptJson = new
                        {
                            validationTime = DateTime.Now,
                            errors = new List<string>() { "Wrong client id!" }
                        }
                    };
                    string errorStr = JsonConvert.SerializeObject(error, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    return BadRequest(errorStr);
                }

                if (String.IsNullOrWhiteSpace(secretKey) || !String.Equals(secretKey, "8F70C29ACBB38F39B0900C26B3A20B0683E62C97A4E578358904686D0023988D0C9873AD23EE87003B36EE5221617FEC0345E3B1138FE1B57EF5DE4771E3CF42"))
                {
                    var error = new
                    {
                        acceptedReceiptJson = "",
                        unacceptedReceiptJson = new
                        {
                            validationTime = DateTime.Now,
                            errors = new List<string>() { "Wrong secret key!" }
                        }
                    };
                    string errorStr = JsonConvert.SerializeObject(error, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    return BadRequest(errorStr);
                }

                if (content == null)
                {
                    var error = new
                    {
                        acceptedReceiptJson = "",
                        unacceptedReceiptJson = new
                        {
                            validationTime = DateTime.Now,
                            errors = new List<string>() { "Missing content!" }
                        }
                    };
                    string errorStr = JsonConvert.SerializeObject(error, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    return BadRequest(errorStr);
                }

                var result = new
                {
                    acceptedReceiptJson = new
                    {
                        id = content.AisPaymentId,
                        registrationTime = DateTime.Now
                    },
                    unacceptedReceiptJson = ""
                };

                //var testresult = new
                //{
                //    acceptedReceiptJson = "",
                //    unacceptedReceiptJson = new
                //    {
                //        validationTime = DateTime.Now,
                //        errors = new List<string>() { "Test error!" }
                //    }
                //};

                string resultStr = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                Log.Information($"IntegrationController/receivePaymentRequest - result: " +
                    $"{resultStr}");
                return Ok(resultStr);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR receiving payment request");
                return BadRequest("Error receiving payment request");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Audit]
        [Route("paymentRequestStatusChanged")]
        public async Task<IActionResult> PaymentRequestStatusChanged([FromBody] PaymentRequestStatusChangeModel model)
        {
            try
            {
                if (model == null)
                {
                    Log.Information($"IntegrationController/paymentRequestStatusChanged - empty model");
                    return BadRequest("Empty model");
                }

                await paymentService.ChangePaymentRequestStatusAsync(model);
                Log.Information($"IntegrationController/paymentRequestStatusChanged - changed request with Id: {model.Id} / {model.Status} / {model.ChangeTime}");
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR changing payment request status");
                return BadRequest("Error changing payment request status");
            }
        }

        //Тест на ЧСИ
        //[HttpGet]
        //[Audit]
        //[Route("CSI")]
        //public async Task<IActionResult> GetCSI()
        //{
        //    try
        //    {
        //        var result = await integrationService.GetCSIByNumberAndDate("701", "01-01-2020");
        //        //var result = await integrationService.GetCSIByNumber("888", true);

        //        return Ok(result);
        //    }
        //    catch (Exception x)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, x.Message);
        //    }
        //}
    }
}
