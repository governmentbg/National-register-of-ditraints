using Microsoft.AspNetCore.Mvc;
using NRZ.Services.Interfaces;
using NRZ.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : BaseApiController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }


        [HttpGet("getFromRegix/{identifier}")]
        [Audit]
        public async Task<IActionResult> GetCompanyFromRegix(string identifier)
        {
            try
            {
                var company = await _companyService.GetCompanyFromRegixAsync(identifier);
                return Ok(company);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR getting company from Regix by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error getting company from Regix");
            }
        }
    }
}
