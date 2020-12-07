using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NRZ.Models.Identity;
using NRZ.Models.Nomenclatures;
using NRZ.Models.Settings;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using Serilog;
using System;
using System.Threading.Tasks;

namespace NRZ.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NomenclatureController : BaseApiController
    {
        private const string AuthHeaderName = "Authorization";
        private readonly TokenConfig _tokenConfig;
        private readonly INomenclatureService _nomenclatureService;

        public NomenclatureController(UserManager<ApplicationUser> userManager,
            IOptions<TokenConfig> tokenConfig,
            IStringLocalizer<SharedResources> localizer,
            INomenclatureService nomenclatureService

            )
            : base(userManager, localizer)
        {
            _tokenConfig = tokenConfig.Value;
            _nomenclatureService = nomenclatureService;
        }

        [AllowAnonymous]
        [HttpGet("getPropertyTypes")]
        public async Task<IActionResult> GetPropertyTypes()
        {
            try
            {
                var propertyTypes = await _nomenclatureService.GetAllPropertyTypesAsync();

                return Ok(propertyTypes);
            }
            catch(Exception ex)
            {
                return BadRequest("Error getting property types: " + ex.Message + "/" + ex.InnerException?.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("getPropertyTypesForDistraint")]
        public async Task<IActionResult> GetPropertyTypesForDistraint()
        {
            try
            {
                var propertyTypes = await _nomenclatureService.GetPropertyTypesForDistraintAsync();

                return Ok(propertyTypes);
            }
            catch (Exception ex)
            {
                return BadRequest("Error getting property types for distraint: " + ex.Message + "/" + ex.InnerException?.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("UserTypes")]
        public IActionResult UserTypes()
        {
            try
            {
                var types = _nomenclatureService.UserTypes();
                return Ok(types);
            }
            catch(Exception x)
            {
                Log.Error("ERROR getting nomenclature UserTypes", x);
                return BadRequest("Error getting property types");
            }
        }

        [AllowAnonymous]
        [HttpGet("UserRegisterTypes")]
        public IActionResult UserRegisterTypes()
        {
            try
            {
                var types = _nomenclatureService.UserRegisterTypes();
                return Ok(types);
            }
            catch(Exception x)
            {
                Log.Error("ERROR getting nomenclature UserRegisterTypes", x);
                return BadRequest("Error getting property types");
            }
        }

        [AllowAnonymous]
        [HttpGet("AuctionOrderTypes")]
        public IActionResult AuctionOrderTypes()
        {
            try
            {
                var types = _nomenclatureService.AuctionOrderTypes();
                return Ok(types);
            }
            catch (Exception x)
            {
                Log.Error("ERROR getting nomenclature AuctionOrder", x);
                return BadRequest("Error getting auction order types");
            }
        }

        [AllowAnonymous]
        [HttpGet("Regions")]
        public IActionResult Regions([FromQuery]string search = null)
        {
            try
            {
                var types = _nomenclatureService.Regions(search);
                return Ok(types);
            }
            catch (Exception x)
            {
                Log.Error("ERROR getting nomenclature Regions", x);
                return BadRequest("ERROR getting nomenclature Regions");
            }
        }

        [AllowAnonymous]
        [HttpGet("Municipalities")]
        public IActionResult Municipalities([FromQuery] int? regionId = null, [FromQuery] string search = null)
        {
            try
            {
                if (!regionId.HasValue)
                {
                    return Ok(Array.Empty<NomenclatureBaseModel>());
                }

                var types = _nomenclatureService.Municipalities(regionId, search);
                return Ok(types);
            }
            catch (Exception x)
            {
                Log.Error("ERROR getting nomenclature Municipalities", x);
                return BadRequest("ERROR getting nomenclature Municipalities");
            }
        }

        [AllowAnonymous]
        [HttpGet("Cities")]
        public IActionResult Cities([FromQuery] int? municipalityId = null, [FromQuery] string search = null)
        {
            try
            {
                if (!municipalityId.HasValue)
                {
                    return Ok(Array.Empty<NomenclatureBaseModel>());
                }

                var types = _nomenclatureService.Cities(municipalityId, search);
                return Ok(types);
            }
            catch (Exception x)
            {
                Log.Error("ERROR getting nomenclature Cities", x);
                return BadRequest("ERROR getting nomenclature Cities");
            }
        }

        [AllowAnonymous]
        [HttpGet("RegixCompanyStatuses")]
        public async Task<IActionResult> GetRegixCompanyStatuses()
        {
            try
            {
                var statuses = await _nomenclatureService.GetRegixCompanyStatuses();

                return Ok(statuses);
            }
            catch (Exception ex)
            {
                return BadRequest("Error getting Regix company statuses: " + ex.Message + "/" + ex.InnerException?.Message);
            }
        }

        
        [AllowAnonymous]
        [HttpGet("TimestampAuthorities")]
        public IActionResult GetTimestampAuthorities()
        {

            try
            {
                var statuses = _nomenclatureService.GetTimestampAuthorities();

                return Ok(statuses);
            }
            catch (Exception ex)
            {
                return BadRequest("Error getting Regix company statuses: " + ex.Message + "/" + ex.InnerException?.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("AssetTypes")]
        public async Task<IActionResult> GetAssetTypes()
        {
            try
            {
                var statuses = await _nomenclatureService.GetAssetTypesAsync();

                return Ok(statuses);
            }
            catch (Exception ex)
            {
                return BadRequest("Error getting Asset types: " + ex.Message + "/" + ex.InnerException?.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("PropertyConstructionType")]
        public async Task<IActionResult> GetPropertyConstructionType()
        {
            try
            {
                var statuses = await _nomenclatureService.GetPropertyConstructionTypeAsync();

                return Ok(statuses);
            }
            catch (Exception ex)
            {
                return BadRequest("Error getting Property construction types: " + ex.Message + "/" + ex.InnerException?.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("PropertyExtras")]
        public async Task<IActionResult> GetPropertyExtras()
        {
            try
            {
                var statuses = await _nomenclatureService.GetPropertyExtrasAsync();

                return Ok(statuses);
            }
            catch (Exception ex)
            {
                return BadRequest("Error getting Property extras: " + ex.Message + "/" + ex.InnerException?.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("RealEstateTypes")]
        public async Task<IActionResult> GetRealEstateTypes()
        {
            try
            {
                var statuses = await _nomenclatureService.GetRealEstateTypesAsync();

                return Ok(statuses);
            }
            catch (Exception ex)
            {
                return BadRequest("Error getting Real estate types: " + ex.Message + "/" + ex.InnerException?.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("RequesterTypes")]
        public async Task<IActionResult> RequesterTypes()
        {
            try
            {
                var types = await _nomenclatureService.RequesterTypes();
                return Ok(types);
            }
            catch (Exception x)
            {
                Log.Error("ERROR getting nomenclature RequesterTypes", x);
                return BadRequest("Error getting property types");
            }
        }
    }
}
