using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using NRZ.Models.Auction;
using NRZ.Models.Identity;
using NRZ.Services.Interfaces;
using NRZ.Shared;
using NRZ.Shared.Enums;
using NRZ.Shared.Localization;
using NRZ.Web.Extensions;
using Serilog;

namespace NRZ.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionRegisterController : BaseApiController
    {
        private readonly IAuctionRegisterService _registerService;
        private readonly IPersonService _personService;
        private readonly IEmailService _emailService;

        public AuctionRegisterController(IAuctionRegisterService registerService, IEmailService emailService, IPersonService personService, UserManager<ApplicationUser> userManager, IStringLocalizer<SharedResources> localizer)
        : base(userManager, localizer)
        {
            _registerService = registerService;
            _personService = personService;
            _emailService = emailService;
        }

        [HttpGet]
        [Route("Get/{id}")]
        public IActionResult Get([FromRoute] int id) 
        {
            try
            {
                var result = _registerService.GetById(id);
                return Ok(result);
            }
            catch (Exception x)
            {
                Log.Error($"ERROR getting auction registration for announcement ID = {id}", x.Message);
                return StatusCode(500, x.Message);
            }
        }

        [HttpGet]
        [Route("Announcement/{id}")]
        public IActionResult GetByAnnouncement([FromRoute] int id)
        {
            try
            {
                IQueryable<AnnouncemetRegisterGridModel> result = null;

                if (User.IsInRole(Constants.Role_SysAdmin))
                {
                    result = _registerService.GetByAnnouncementId(id);
                }
                else
                {
                    result = _registerService.GetByAnnouncementAndUser(id, CurrentUserId);
                }
                
                return Ok(result);
            }
            catch (Exception x)
            {
                Log.Error($"ERROR getting auction registration ID = {id}", x.Message);
                return StatusCode(500, x.Message);
            }
        }

        [HttpGet]
        [Route("User/{id}")]
        public IActionResult GetAllByUser([FromRoute] string id)
        {
            try
            {
                var result = _registerService.GetAllByUser(id);
                return Ok(result);
            }
            catch (Exception x)
            {
                Log.Error($"ERROR getting auction registration for user ID = {id}", x.Message);
                return StatusCode(500, x.Message);
            }
        }

        [HttpGet]
        [Route("Creator/{id}")]
        public IActionResult GetAllByCreator([FromRoute] string id)
        {
            try
            {
                var result = _registerService.GetAllByCreator(id);
                return Ok(result);
            }
            catch (Exception x)
            {
                Log.Error($"ERROR getting auction registration for user ID = {id}", x.Message);
                return StatusCode(500, x.Message);
            }
        }

        [HttpGet]
        [Route("{id}/Creator/{userId?}")]
        public IActionResult GetByUser([FromRoute] int id, [FromRoute] string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    userId = CurrentUserId;
                }

                var result = _registerService.GetByAnnouncementAndUser(id, userId);
                return Ok(result);
            }
            catch (Exception x)
            {
                Log.Error($"ERROR getting auction registration for user ID = {id}", x.Message);
                return StatusCode(500, x.Message);
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm] string data, [FromForm]List<IFormFile> files )
        {
            try
            {
                var model = JsonConvert.DeserializeObject<AuctionRegisterModel>(data);

                if (!ValidateObject(model))
                {
                    return BadRequest(_localizer["Error_InvalidData"]);
                }

                if (files != null && files.Count() > 0)
                {
                    model.Attachments = await ParseAttachmentsAsync(files);
                }

                if (model.AppliedByCourtEnforcer)
                {
                    if (string.IsNullOrWhiteSpace(model.ParticipantId))
                    {
                        var person = await _personService.GetByPersonIdentification(model.Participant.IdentificationType, model.Participant.IdentificationNumber);

                        if (person == null || string.IsNullOrWhiteSpace(person.UserId))
                        {
                            //Create application user
                            var user = new ApplicationUser
                            {
                                UserName = model.Participant.FirstName.Trim() + (String.IsNullOrWhiteSpace(model.Participant.MiddleName) ? "" : ("." + model.Participant.MiddleName.Trim())) + "." +  model.Participant.LastName.Trim(),
                                Email = model.Participant.Email,
                                EmailConfirmed = true,
                                ConfirmedByAdmin = true,
                                AuthType = AuthType.USER.ToString(),
                                UserType = UserType.AUCPAR.ToString(),
                                CreatedOn = DateTime.UtcNow
                            };

                            var result = await _userManager.CreateAsync(user);
                            if (!result.Succeeded)
                            {
                                Log.Error("Error creating new user", JsonConvert.SerializeObject(result.Errors));
                                throw new Exception("Error creating new user, " + JsonConvert.SerializeObject(result.Errors));
                            }

                            //Send reset password email so the user can set his/her password
                            await SendResetPasswordEmailAsync(user);

                            model.Participant.UserId = user.Id;
                        }
                        else
                        {
                            model.ParticipantId = person.UserId;
                        }
                    }
                }
                else
                {
                    model.ParticipantId = User.GetUserId();
                }

                await _registerService.CreateAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception x)
            {
                Log.Error($"ERROR creating auction registration", x.Message);
                return StatusCode(500, x.Message);
            }
        }

        [HttpPost]
        [Route("Approve")]
        public async Task<IActionResult> Approve([FromBody] ApprovalModel model)
        {
            try
            {
                await _registerService.ProccessAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception x)
            {
                Log.Error($"ERROR approving auction registration ID = {model.EntityId}", x.Message);
                return StatusCode(500, x.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            try
            {
                await _registerService.DeleteAsync(id, User.GetUserId());
                return Ok();
            }
            catch (Exception x)
            {
                Log.Error($"ERROR deleting auction registration ID = {id}", x.Message);
                return StatusCode(500, x.Message);
            }
        }

        private async Task SendResetPasswordEmailAsync(ApplicationUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = System.Net.WebUtility.UrlEncode(code);
            var callbackUrl = Request.Headers["Referer"];
            callbackUrl += $"#/resetPassword?userId={user.Id}&code={encodedToken}";

            _emailService.SendEmail(user.Email, null, null, _localizer.GetString("NewUser"),
                string.Format(_localizer.GetString("UserCreateAndPasswordResetEmail"), user.UserName, HtmlEncoder.Default.Encode(callbackUrl)));
        }
    }
}
