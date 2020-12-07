using NRZ.EAuthentication.Model;
using NRZ.Models.EAuthentication;
using NRZ.Service.EAuthentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NRZ.Models.Settings;
using NRZ.Web.Attributes;
using NRZ.Services.Interfaces;
using NRZ.Web.Services;
using NRZ.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using NRZ.Shared.Localization;
using Microsoft.Extensions.Localization;

namespace NRZ.Web.Controllers
{
    [Audit]
    [Route("[controller]")]
    public class EAuthController : Controller
    {
        // За някои OID-та на заявена услуга се връща резултат с грешка "Некоректни данни",
        // затова по подразбиране се използват тези проверени OID-та.
        public const string RequestedServiceOid = "2.16.100.1.1.1.1.4";  // ИС: Компонент за еднократна автентификация.
        public const string RequestedProviderOid = "2.16.100.1.1.1";  // Администрация: МТИТС.

        private readonly EAuthSettings _settings;
        private readonly IApplicationStoreService _applicationStoreService;
        private readonly AuthService _authService;

        public EAuthController(IOptions<EAuthSettings> settings,
            IApplicationStoreService applicationStoreService,
            AuthService authService)
        {
            _settings = settings.Value;
            _applicationStoreService = applicationStoreService;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("LoginCallback")]
        public async Task<ActionResult> LoginCallback(EAuthCallbackModel model)
        {
            (string returnUrl, string requestId) = await _authService.LoginAsync(model);
            _applicationStoreService.Clear(requestId);

            return Redirect(returnUrl);
        }

        [AllowAnonymous]
        [HttpPost("RegisterCallback")]
        public async Task<ActionResult> RegisterCallback(EAuthCallbackModel model)
        {
            (string returnUrl, string requestId) = await _authService.RegisterAsync(model);
            _applicationStoreService.Clear(requestId);

            return Redirect(returnUrl);
        }

        [AllowAnonymous]
        [HttpGet("personRequest")]
        public ActionResult PersonRequest([FromQuery] string type, [FromQuery] string userType, [FromQuery] string chsiNumber, [FromQuery] string email, [FromQuery] string lang)
        {

            EAuthRequestViewModel model = null;

            // Създава се SAML заявка и се записва в журнал.
            string requestUrl = Url.Action(nameof(PersonRequest), null, null, Request.Scheme);
            string callbackUrl = (type ?? "").Equals("login", System.StringComparison.OrdinalIgnoreCase)
                ? Url.Action(nameof(LoginCallback), null, null, Request.Scheme)
                : Url.Action(nameof(RegisterCallback), null, null, Request.Scheme);

            EAuthService service = new EAuthService();
            model = service.CreateRequestAsync(requestUrl, callbackUrl,
                RequestedServiceOid, RequestedProviderOid, null, false, _settings?.CertificateThumbprint);

            _applicationStoreService.SetUserType(model.RequestId, userType);
            _applicationStoreService.SetChsiNumber(model.RequestId, chsiNumber);
            _applicationStoreService.SetApplicationBaseUrl(model.RequestId, Request.Headers["Referer"].ToString().TrimEnd('/'));
            _applicationStoreService.SetLang(model.RequestId, lang);
            _applicationStoreService.SetEmail(model.RequestId, email);

            return Ok(model);
        }
    }
}
