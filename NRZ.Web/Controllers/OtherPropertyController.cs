using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Models.Identity;
using NRZ.Models.Property;
using NRZ.Models.Settings;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using NRZ.Web.Attributes;
using NRZ.Web.Extensions;
using Serilog;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherPropertyController : BaseApiController
    {
        private readonly IPropertyService _service;
        public OtherPropertyController(IPropertyService service,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<SharedResources> localizer)
            : base(userManager, localizer)
        {
            _service = service;
        }

        [Audit]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] OtherPropertyModel model)
        {
            if (!ModelState.IsValid)
            {
                string msg = $"{_localizer.GetString("InvalidModel").Value} {nameof(OtherPropertyModel)}!<br />{ModelState.GetErrors()}";
                Log.Information($"OtherPropertyController/Create/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return BadRequest(msg);
            }

            try
            {
                OtherPropertyModel entity = await _service.AddOtherPropertyAsync(model);

                string msg = $"{_localizer.GetString("Create success").Value}! (Id: {entity.Id})";
                Log.Information($"OtherPropertyController/Create/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return Ok(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR creating otherProperty by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error creating otherProperty");
            }
        }
    }
}
