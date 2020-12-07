using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NRZ.Models.Identity;
using NRZ.Models.Property;
using NRZ.Models.Settings;
using NRZ.Services.Interfaces;
using NRZ.Shared.Enums;
using NRZ.Shared.Localization;
using NRZ.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using NRZ.Data;
using NRZ.Web.Extensions;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : BaseApiController
    {
        private const string AuthHeaderName = "Authorization";
        private readonly TokenConfig _tokenConfig;
        private readonly IPropertyService _propertyService;

        public PropertyController(UserManager<ApplicationUser> userManager,
            IOptions<TokenConfig> tokenConfig,
            IStringLocalizer<SharedResources> localizer,
            IPropertyService propertyService
            )
            : base(userManager, localizer)
        {
            _tokenConfig = tokenConfig.Value;
            _propertyService = propertyService;
        }



        [HttpPost("searchInDB/{propertyTypeCode}")]
        [Audit]
        public async Task<IActionResult> SearchInDB(string propertyTypeCode, [FromBody] PropertySearchRequestModel model)
        {
            try
            {
                if (model == null)
                {
                    Log.Information($"PropertyController/SearchInDB/{CurrentUserId}/{CurrentUserUsername} - Missing PropertySearchRequestModel");
                    return BadRequest("Missing PropertySearchRequestModel");
                }
                    

                if (!Enum.TryParse(propertyTypeCode.ToUpper(), out Shared.Enums.PropertyType propType))
                {
                    Log.Information($"PropertyController/SearchInDB/{CurrentUserId}/{CurrentUserUsername} - Unrecognized property type");
                    return BadRequest("Unrecognized property type");
                }                    

                IEnumerable<object> properties = null;
                switch(propType)
                {
                    case Shared.Enums.PropertyType.OTHER:
                        properties = await _propertyService.SearchOtherPropertyInDBAsync(model);
                        break;
                    case Shared.Enums.PropertyType.VEHICLE:
                        properties = await _propertyService.SearchVehicleInDBAsync(model);
                        break;                    
                    case Shared.Enums.PropertyType.AIRCRAFT:
                        properties = await _propertyService.SearchAircraftInDBAsync(model);
                        break;
                    case Shared.Enums.PropertyType.VESSEL:
                        properties = await _propertyService.SearchVesselInDBAsync(model);
                        break;
                    case Shared.Enums.PropertyType.AGRIFORMACHINERY:
                        properties = await _propertyService.SearchAgriculturalMachineryInDBAsync(model);
                        break;
                    case Shared.Enums.PropertyType.REALESTATE:
                        //properties = await _propertyService.SearchInDBAsync(propType, model);
                        break;
                    default:
                        break;
                }                

                return Ok(properties);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR searching for properties in DB: {propertyTypeCode} / {model?.Identifier} by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error searching for properties in DB");
            }
        }


        [HttpPost("searchInAdministrator/{propertyTypeCode}")]
        [Audit]
        public async Task<IActionResult> SearchInDataAdministrator(string propertyTypeCode, [FromBody] PropertySearchRequestModel model)
        {
            try
            {
                if (model == null)
                {
                    Log.Information($"PropertyController/SearchInDataAdministrator/{CurrentUserId}/{CurrentUserUsername} - Missing property search model");
                    return BadRequest("Missing property search model");
                }   

                if (!Enum.TryParse(propertyTypeCode.ToUpper(), out Shared.Enums.PropertyType propType))
                {
                    Log.Information($"PropertyController/SearchInDataAdministrator/{CurrentUserId}/{CurrentUserUsername} - Unrecognized property type");
                    return BadRequest("Unrecognized property type");
                }                    

                var properties = await _propertyService.SearchInDataAdministratorAsync(propType, model);

                return Ok(properties);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR searching for properties in data administrator: {propertyTypeCode} / {model?.Identifier} by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error searching for properties in data administrator");
            }
        }

        [Audit]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PropertyModel model)
        {
            ModelState.Remove("Address.StreetAddress");
            if (!ModelState.IsValid)
            {
                string msg = $"{_localizer.GetString("InvalidModel").Value} {nameof(PropertyModel)}!<br />{ModelState.GetErrors()}";
                Log.Information($"PropertyController/Create/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return BadRequest(msg);
            }

            try
            {
                var userId = this.User.GetUserId();
                PropertyModel entity = await _propertyService.AddAsync(model, userId);

                string msg = $"{_localizer.GetString("Create success").Value}! (Id: {entity.Id})";
                Log.Information($"PropertyController/Create/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return Ok(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR creating property by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error creating property");
            }
        }

        [Audit]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] PropertyModel model)
        {
            if (!ModelState.IsValid)
            {
                string msg = $"{_localizer.GetString("InvalidModel").Value} {nameof(PropertyModel)}!<br />{ModelState.GetErrors()}";
                Log.Information($"PropertyController/Update/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return BadRequest(msg);
            }

            try
            {
                var userId = this.User.GetUserId();
                PropertyModel entity = await _propertyService.UpdateAsunc(model, userId);

                string msg = $"{_localizer.GetString("Update success").Value}! (Id: {entity.Id})";
                Log.Information($"PropertyController/Update/{CurrentUserId}/{CurrentUserUsername} - {msg}");
                return Ok(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR creating property by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error creating property");
            }
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult Test()
        {
            try
            {
                return Ok("Test ok");
            }
            catch
            {
                return BadRequest("Test error");
            }
        }

    }
}
