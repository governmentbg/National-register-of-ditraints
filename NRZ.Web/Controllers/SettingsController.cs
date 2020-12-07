using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NRZ.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : BaseApiController
    {
        private readonly IOptions<ApplicationSettings> _appSettings;
        private readonly IOptions<IntegrationSettings> _integrationSettings;

        public SettingsController(IOptions<ApplicationSettings> appSettings, IOptions<IntegrationSettings> integrationSettings)
        {
            _appSettings = appSettings;
            _integrationSettings = integrationSettings;
        }

        [AllowAnonymous]
        [HttpGet("Version")]
        public string GetVersion()
        {
            return _appSettings.Value.Version;
        }

        [AllowAnonymous]
        [HttpGet("UseRegix")]
        public bool GetUseRegix()
        {
            return _integrationSettings.Value.UseRegiX;
        }

        [AllowAnonymous]
        [HttpGet("today")]
        public DateTime GetToday()
        {
            return DateTime.Today.Date;
        }

    }
}
