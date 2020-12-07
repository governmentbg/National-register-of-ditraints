using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NRZ.Models.CSI;
using NRZ.Models.Identity;
using NRZ.Models.Person;
using NRZ.Models.Settings;
using NRZ.Services.Interfaces;
using NRZ.Shared;
using NRZ.Shared.Enums;
using NRZ.Shared.Localization;
using NRZ.Web.Attributes;
using NRZ.Web.Auth;
using Serilog;

namespace NRZ.Web.Controllers
{
    [Audit]
    public class CertificateController : Controller
    {
        private const string _loginReturnPath = "/#/login/result";
        private const string _registerReturnPath = "/#/register/result";
        private readonly TokenConfig _tokenConfig;
        //private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPersonService _personService;
        //private readonly IClaimService _claimService;
        protected readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IEmailService _emailService;
        private readonly IIntegrationService _integrationService;

        public CertificateController(
            UserManager<ApplicationUser> userManager,
            IPersonService personService,
            //IClaimService claimService,
            IOptions<TokenConfig> tokenConfig,
            //ITokenService tokenService,
            IEmailService emailService,
            IIntegrationService integrationService,
            IStringLocalizer<SharedResources> localizer)
        {
            _tokenConfig = tokenConfig.Value;
            //_tokenService = tokenService;
            _userManager = userManager;
            _personService = personService;
            //_claimService = claimService;
            _localizer = localizer;
            _emailService = emailService;
            _integrationService = integrationService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            string referer = Request.Headers["Referer"].ToString().TrimEnd('/');
            string returnUrl = referer + _loginReturnPath;

            if (HttpContext.Connection.ClientCertificate == null)
            {
                returnUrl += $"?error=true";
                return Redirect(returnUrl);
            }

            try
            {
                //Parse client certificate
                X509Certificate2 certificate = HttpContext.Connection.ClientCertificate;
                ParseResiult parseResult = DigitalSignatureParser.DecodeCert(certificate);

                if (parseResult.Success)
                {
                    ApplicationUser user = _userManager.Users.SingleOrDefault(u => u.Certificate_UniqueIdentifier.Trim() == parseResult.HolderEGN.Trim() && !u.Deleted);

                    if (user == null)
                    {
                        returnUrl += $"?error=true&message=userNotFound";
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        /*
                         * Ако потребителя е сменил своето име в подписа, го променяме и в базата
                         * т.е. ако жена се омъжи и смени фамилията си и т.н.
                         * Ако подписа е променен също запазваме данните
                         */
                        if (user.Certificate_Name == null
                            || user.Certificate_Name.ToLower().Trim() != parseResult.HolderName.ToLower().Trim()
                            || user.Certificate_Thumbprint != certificate.Thumbprint)
                        {
                            //Check if certificate is the same and update user info if needed
                            user.Certificate = certificate.RawData;
                            user.Certificate_Thumbprint = certificate.Thumbprint;
                            user.Certificate_Name = parseResult.HolderName;
                            await _userManager.UpdateAsync(user);
                        }
                    }

                    if (!user.EmailConfirmed)
                    {
                        await SendConfirmEmailAsync(user);
                        returnUrl += $"?error=true&message=EmailNotConfirmed,ConfirmationEmailResend";
                        return Redirect(returnUrl);
                    }

                    if (!user.ConfirmedByAdmin)
                    {
                        returnUrl += $"?error=true&message=notConfirmedByAdmin";
                        return Redirect(returnUrl);
                    }

                    //sign in existing user
                    return await SignInExistingUser(user, returnUrl);
                }

                Log.Error("Error parsing login certificate" + String.Join(", ", parseResult.Errors));
                returnUrl += $"?error=true";
                return Redirect(returnUrl);
            }
            catch (Exception x)
            {
                Log.Error(x, "Error parsing login certificate");
                returnUrl += $"?error=true";
                return Redirect(returnUrl);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Register([FromQuery] string userType, [FromQuery] string email, [FromQuery] string chsiNumber, [FromQuery] string lang = "bg")
        {
            string referer = Request.Headers["Referer"].ToString().TrimEnd('/');
            string returnUrl = referer + _registerReturnPath;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);

            if (HttpContext.Connection.ClientCertificate == null || string.IsNullOrWhiteSpace(userType))
            {
                returnUrl += $"?error=true";
                return Redirect(returnUrl);
            }

            if ((userType == UserType.CHSI.ToString() || userType == UserType.CHSIHelper.ToString()) && string.IsNullOrWhiteSpace(chsiNumber))
            {
                returnUrl += $"?error=true";
                return Redirect(returnUrl);
            }

            try
            {
                X509Certificate2 certificate = HttpContext.Connection.ClientCertificate;
                ParseResiult parseResult = DigitalSignatureParser.DecodeCert(certificate);

                if (parseResult.Success)
                {
                    //check egn
                    bool isDuplicateEgn = _userManager.Users.Any(u => u.Certificate_UniqueIdentifier == parseResult.HolderEGN.ToUpper().Trim() && !u.Deleted);

                    if (isDuplicateEgn)
                    {
                        returnUrl += $"?error=true&message=duplicateEgn";
                        return Redirect(returnUrl);
                    }

                    // if user did not pass an email we put the one from the digital signature
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        email = parseResult.HolderEmail;
                    }

                    //check email 
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        //Redirect to register page with parameter to gather additional info
                        returnUrl += "?requireEmail=true";
                        return Redirect(returnUrl);
                    }

                    //Create new user
                    ApplicationUser user = new ApplicationUser()
                    {
                        Certificate = certificate.RawData,
                        Certificate_Thumbprint = certificate.Thumbprint,
                        Email = email,
                        EmailConfirmed = false,
                        UserName = GenerateUserName(parseResult.HolderName),
                        Certificate_Name = parseResult.HolderName.Trim(),
                        Certificate_UniqueIdentifier = parseResult.HolderEGN.Trim(),
                        AuthType = AuthType.ESIGN.ToString(),
                        UserType = userType,
                        ConfirmedByAdmin = false,
                        CreatedOn = DateTime.UtcNow
                    };

                    string userRole = Constants.Role_AuctionOrgaziner;

                    if (userType == UserType.AUCPAR.ToString())
                    {
                        user.ConfirmedByAdmin = true;
                        userRole = Constants.Role_AuctionParticipant;
                    }
                    else if (userType == UserType.CHSI.ToString())
                    {
                        user.CHSINumber = chsiNumber;
                        //Check CHSI in CHSI register by number
                        await CheckCHSI(user, returnUrl);
                    }
                    else if (userType == UserType.CHSIHelper.ToString())
                    {
                        user.CHSINumber = chsiNumber;
                        //TODO Check CHSI helpers in CHSI register by number
                        //Return error if chsi is not found
                        bool ChsiRegisterCheck = true;

                        user.CheckedInCHSIRegister = ChsiRegisterCheck;
                        user.ConfirmedByAdmin = ChsiRegisterCheck;
                    }

                    IdentityResult ir = await _userManager.CreateAsync(user);

                    if (!ir.Succeeded)
                    {
                        //create new user fail
                        Log.Error("Error creating user with ESign", String.Join(',', ir.Errors.Select(x => x.Description)));
                        returnUrl += $"?error=true";
                        return Redirect(returnUrl);
                    }

                    //Assign to role
                    var roleAddResult = await _userManager.AddToRoleAsync(user, userRole);

                    if (!roleAddResult.Succeeded)
                    {
                        Log.Error($"Error adding user ID= {user.Id} to role {userRole}", string.Join(',', roleAddResult.Errors.Select(x => x.Description)));
                    }

                    try
                    {
                        //create person
                        await CreatePerson(user.Id, parseResult.HolderName, email);
                        //send confirmation email
                        await SendConfirmEmailAsync(user);

                        return Redirect(returnUrl);
                    }
                    catch (Exception x)
                    {
                        //await AuditService.EditContentAndUserAsync(AuditId, AuditContentType.Activity,
                        //        AuditMessages.Failure.ToString(), user.Id, user.UserName);

                        Log.Error("Error creating person with ESign", x);
                        IdentityResult deleteResult = await _userManager.DeleteAsync(user);
                        returnUrl += $"?error=true";
                        return Redirect(returnUrl);
                    }
                }

                Log.Error("Error parsing certificate" + String.Join(", ", parseResult.Errors));
                returnUrl += $"?error=true";
                return Redirect(returnUrl);
            }
            catch (Exception x)
            {
                //cannot parse certificate
                Log.Error(x, "Error parsing certificate on register");
                returnUrl += $"?error=true";
                return Redirect(returnUrl);
            }
        }

        private async Task<IActionResult> SignInExistingUser(ApplicationUser user, string returnUrl)
        {
            try
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var token = JWTManager.GenerateToken(user, userRoles, _tokenConfig);

                //await InvalidateToken(Request.Headers[AuthHeaderName]);
                //var userClaims = await _claimService.ValidClaimsForUserAsync(user.Id);

                string infoAsJson = JsonConvert.SerializeObject(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    userName = user.UserName,
                    userEmail = user.Email,
                    userId = user.Id,
                    tokenExpiration = token.ValidTo,
                    roles = userRoles,
                    //userClaims = _claimService.ClaimsAsDictionary(userClaims)
                });

                returnUrl += $"?auth={infoAsJson}";
                return Redirect(returnUrl);
            }
            catch
            {
                //await AuditService.EditContentAndUserAsync(AuditId, AuditContentType.Activity,
                //    AuditMessages.Failure.ToString(), user.Id, user.UserName);

                returnUrl += $"?error=true";
                return Redirect(returnUrl);
            };
        }

        private async Task SendConfirmEmailAsync(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Request.Headers["Referer"];
            callbackUrl += $"#account/emailConfirm?userId={user.Id}&code={code}";

            //TODO read email body from db
            _emailService.SendEmail(user.Email, null, null, _localizer.GetString("Email_ConfirmTitle"),
                string.Format(_localizer.GetString("Email_ConfirmBody"), HtmlEncoder.Default.Encode(callbackUrl)));

        }

        private string GenerateUserName(string holderName)
        {
            string username = holderName.Replace(' ', '.');
            bool usersWithSameName = _userManager.Users
                                                 .Any(x => x.NormalizedUserName.Trim() == username.ToUpper().Trim() && !x.Deleted);

            if (usersWithSameName)
            {
                //Add random number to username when there are more than one user with same name
                Random rand = new Random(999999);
                username += rand.Next().ToString().Trim();
            }

            return username;
        }

        private async Task CreatePerson(string userId, string fullName, string email)
        {
            string[] holderNames = fullName.Split(' ');

            PersonModel person = new PersonModel()
            {
                FirstName = holderNames[0],
                MiddleName = holderNames.Length == 3 ? holderNames[1] : null,
                LastName = holderNames.Length == 3 ? holderNames[2] : holderNames[1],
                Email = email,
                UserId = userId
            };

            await _personService.CreateAsync(person);
        }

        private bool ValidateCHSI(CSIModel model, string fullName)
        {
            bool isValidCHSI = (model.Date_from.HasValue && model.Date_from < DateTime.Now)
                            && (!model.Date_to.HasValue || model.Date_to > DateTime.Now);

            if (isValidCHSI)
            {
                string[] holderNames = fullName.Split(' ');
                string[] registerName = model.Name_en.Split(' ');

                isValidCHSI = holderNames[0].ToUpper() == registerName[0].ToUpper()
                           && holderNames[1].ToUpper() == registerName[1].ToUpper()
                           && holderNames[2].ToUpper() == registerName[2].ToUpper();
            }

            return isValidCHSI;           
        }

        private async Task CheckCHSI(ApplicationUser user, string returnUrl)
        {
            try
            {
                var chsi = await _integrationService.GetCSIByNumberAndDate(user.CHSINumber.ToString(), DateTime.Now);
                if (chsi != null)
                {
                    bool isValidCHSI = ValidateCHSI(chsi, user.Certificate_Name);
                    user.CheckedInCHSIRegister = true;
                    user.ConfirmedByAdmin = isValidCHSI;

                    if (!isValidCHSI)
                    {
                        //not valid chsi
                        returnUrl += $"?warning=true&warningMessage=chsiNotValid&number={user.CHSINumber}";
                    }
                }
                else
                {
                    //Not found in chsi register
                    returnUrl += $"?warning=true&warningMessage=chsiNotFound&number={user.CHSINumber}";
                    user.CheckedInCHSIRegister = true;
                    user.ConfirmedByAdmin = false;
                }
            }
            catch (Exception x)
            {
                //chsi register error
                Log.Error(x, "ERROR checking CHSI in CHSI register");
                user.CheckedInCHSIRegister = false;
                user.ConfirmedByAdmin = false;
                returnUrl += $"?warning=true&warningMessage=chsiCheckError&number={user.CHSINumber}";
            }
        }
    }
}
