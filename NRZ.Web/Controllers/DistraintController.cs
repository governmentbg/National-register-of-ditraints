using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NRZ.Models.Distraint;
using NRZ.Models.Identity;
using NRZ.Models.Settings;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using NRZ.Web.Attributes;
using NRZ.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistraintController : BaseApiController
    {
        private const string AuthHeaderName = "Authorization";
        private readonly TokenConfig _tokenConfig;
        private readonly IDistraintService _distraintService;

        public DistraintController(UserManager<ApplicationUser> userManager,
            IOptions<TokenConfig> tokenConfig,
            IStringLocalizer<SharedResources> localizer,
            IDistraintService distraintService

            )
            : base(userManager, localizer)
        {
            _tokenConfig = tokenConfig.Value;
            _distraintService = distraintService;
        }


        [HttpGet("list")]
        [Audit]
        public IActionResult List()
        {
            try
            {
                var distraints = _distraintService.GetAll();
                return Ok(distraints);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR getting list with distraints by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error getting list with distraints");
            }
        }

        [HttpGet("search/{searchText}")]
        [Audit]
        public IActionResult Search(string searchText)
        {
            try
            {
                var distraints = _distraintService.Search(searchText);
                return Ok(distraints);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR searching for distraints: {searchText} by user /{CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error searching for distraints");
            }
        }


        [HttpPost("create")]
        [Audit]
        public async Task<IActionResult> Create([FromBody] DistraintCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                string msg = $"{_localizer.GetString("Invalid DistraintCreateModel").Value}!<br />{ModelState.GetErrors()}";
                Log.Information($"DistraintController/Create/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return BadRequest(msg);
            }

            try
            {
                var distraint = await _distraintService.AddAsync(model);

                string msg = $"{_localizer.GetString("Create success").Value}! (Id: {distraint.Id})";
                Log.Information($"DistraintController/Create/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return Ok(distraint.Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR creating distraint by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error creating distraint");
            }
        }

        [HttpGet("get/{id}")]
        [Audit]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var distraint = await _distraintService.GetByIdAsync(id);
                return Ok(distraint);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR getting distraint: {id} by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error getting distraint");
            }
        }


        [HttpPost("enforce")]
        [Audit]
        public async Task<IActionResult> Enforce([FromBody] DistraintEnforceModel model)
        {
            if (!ModelState.IsValid)
            {
                string msg = $"{_localizer.GetString("Invalid DistraintEnforceModel").Value}!<br />{ModelState.GetErrors()}";
                Log.Information($"DistraintController/Enforce/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return BadRequest(msg);
            }

            try
            {
                var distraint = await _distraintService.EnforceAsync(model);

                string msg = $"{_localizer.GetString("Distraint enforced successfully").Value}! (Id: {distraint.Id})";
                Log.Information($"DistraintController/Enforce/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return Ok(distraint.Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR enforcing distraint by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error enforcing distraint");
            }
        }

        [HttpPost("revoke")]
        [Audit]
        public async Task<IActionResult> Revoke([FromBody] DistraintRevokeModel model)
        {
            if (!ModelState.IsValid)
            {
                string msg = $"{_localizer.GetString("Invalid DistraintRevokeModel").Value}!<br />{ModelState.GetErrors()}";
                Log.Information($"DistraintController/Revoke/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return BadRequest(msg);
            }

            try
            {
                var distraint = await _distraintService.RevokeAsync(model);

                string msg = $"{_localizer.GetString("Distraint revoked successfully").Value}! (Id: {distraint.Id})";
                Log.Information($"DistraintController/Revoke/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return Ok(distraint.Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR revoking distraint by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error revoking distraint");
            }
        }


        [HttpPost("exempt")]
        [Audit]
        public async Task<IActionResult> Exempt([FromBody] DistraintExemptModel model)
        {
            if (!ModelState.IsValid)
            {
                string msg = $"{_localizer.GetString("Invalid DistraintExemptModel").Value}!<br />{ModelState.GetErrors()}";
                Log.Information($"DistraintController/Exempt/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return BadRequest(msg);
            }

            try
            {
                var distraint = await _distraintService.ExemptAsync(model);

                string msg = $"{_localizer.GetString("Distraint exempted successfully").Value}! (Id: {distraint.Id})";
                Log.Information($"DistraintController/Exempt/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return Ok(distraint.Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR exempting distraint by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error exempting distraint");
            }
        }


    }
}
