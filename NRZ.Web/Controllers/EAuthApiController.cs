using NRZ.EAuthentication.Model;
using NRZ.Models.EAuthentication;
using NRZ.Service.EAuthentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NRZ.Models.Settings;

namespace NRZ.Web.Controllers
{
    // Видимо не се ползва. Да се изтрие.
    [Route("[controller]")]
    public class EAuthApiController : Controller
    {
        private const string _loginReturnPath = "/#/login/result";
        private const string _registerReturnPath = "/#/register/result";

        // За някои OID-та на заявена услуга се връща резултат с грешка "Некоректни данни",
        // затова по подразбиране се използват тези проверени OID-та.
        public const string RequestedServiceOid = "2.16.100.1.1.1.1.4";  // ИС: Компонент за еднократна автентификация.
        public const string RequestedProviderOid = "2.16.100.1.1.1";  // Администрация: МТИТС.

        private readonly EAuthSettings _settings;

        public EAuthApiController(IOptions<EAuthSettings> settings)
        {
            _settings = settings.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LoginCallback(EAuthCallbackModel model)
        {
            // Записване на отговора в журнал.
            EAuthService service = new EAuthService();
            EAuthResponseModel eAuthResponseModel = service.Parse(model);

            // Изходът от този action не се използва. Целта е само да се зареди локален адрес в callback iframe-а.
            // Това се интерпретира от Java Script-а като успешен край на автентикацията и следва пренасочване към друг адрес.
            //
            // До февруари 2019 г. този метод връщаше празен резултат и това вършеше работа.
            //return new EmptyResult();
            // Chrome версия 72+ обаче започна да интерпретира security header-а X-Content-Type-Options=nosniff по друг начин.
            // Когато резултатът е празен, вече не зарежда резултата на екрана(или в iframe-а), а изтегля празен файл.
            // Така не се задейства никакъв Java Script, iframe-ът не разбира, че е зареден локален адрес, и автентикацията
            // не завършва в Chrome 72+. Tрябва се върне някакъв HTML, колкото да се задейства onload handler-ът на iframe-а.

            //return PartialView();
            return View(eAuthResponseModel);
        }

        [HttpGet("personRequest")]
        [AllowAnonymous]
        public ActionResult PersonRequest()
        {
            EAuthRequestViewModel model = null;

            // Създава се SAML заявка и се записва в журнал.
            string requestUrl = Url.Action(nameof(PersonRequest), null, null, Request.Scheme);
            string callbackUrl = Url.Action(nameof(LoginCallback), null, null, Request.Scheme);

            EAuthService service = new EAuthService();
            model = service.CreateRequestAsync(requestUrl, callbackUrl,
                RequestedServiceOid, RequestedProviderOid, null, false, _settings?.CertificateThumbprint);

            return Ok(model);
        }
    }
}
