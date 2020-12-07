using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NRZ.EAuthentication.Model;
using NRZ.Models.EAuthentication;
using NRZ.Models.Identity;
using NRZ.Models.Person;
using NRZ.Models.Settings;
using NRZ.Service.EAuthentication;
using NRZ.Services.Interfaces;
using NRZ.Shared;
using NRZ.Shared.Enums;
using NRZ.Shared.Localization;
using NRZ.Web.Auth;
using Serilog;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace NRZ.Web.Services
{
    public class AuthService
    {
        private const string _loginReturnPath = "/#/login/result";
        private const string _registerReturnPath = "/#/register/result";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenConfig _tokenConfig;
        private readonly IApplicationStoreService _applicationStoreService;
        private readonly IEmailService _emailService;
        private readonly IPersonService _personService;
        protected readonly IStringLocalizer<SharedResources> _localizer;

        public AuthService(UserManager<ApplicationUser> userManager,
            IOptions<TokenConfig> tokenConfig,
            IApplicationStoreService applicationStoreService,
            IEmailService emailService,
            IPersonService personService,
            IStringLocalizer<SharedResources> localizer)
        {
            _userManager = userManager;
            _tokenConfig = tokenConfig.Value;
            _applicationStoreService = applicationStoreService;
            _emailService = emailService;
            _personService = personService;
            _localizer = localizer;
        }

        public async Task<(string returnUrl, string requestId)> LoginAsync(EAuthCallbackModel model)
        {
            EAuthService service = new EAuthService();
            EAuthResponseModel eAuthResponseModel = service.Parse(model);
            string requestId = eAuthResponseModel?.RequestId;
            string returnUrl = _applicationStoreService.GetApplicationBaseUrl(requestId) + _loginReturnPath;
            (string personIdentifier, string holderName, string email) = ExtractPersonData(eAuthResponseModel);

            ApplicationUser user = _userManager.Users.SingleOrDefault(u => u.Certificate_UniqueIdentifier.Trim() == personIdentifier && !u.Deleted);

            if (user == null)
            {
                returnUrl += $"?error=true&message=userNotFound";
                return (returnUrl, requestId);
            }
            else
            {
                /*
                 * Ако потребителя е сменил своето име в подписа, го променяме и в базата
                 * т.е. ако жена се омъжи и смени фамилията си и т.н.
                 * Ако подписа е променен също запазваме данните
                 */
                if (user.Certificate_Name == null
                    || user.Certificate_Name.ToLower().Trim() != (holderName ?? "").ToLower().Trim())
                {
                    //Check if certificate is the same and update user info if needed
                    user.Certificate_Name = holderName;
                    await _userManager.UpdateAsync(user);
                }
            }

            if (!user.EmailConfirmed)
            {
                await SendConfirmEmailAsync(user, _applicationStoreService.GetApplicationBaseUrl(requestId));
                returnUrl += $"?error=true&message=EmailNotConfirmed,ConfirmationEmailResend";
                return (returnUrl, requestId);
            }

            if (!user.ConfirmedByAdmin)
            {
                returnUrl += $"?error=true&message=NotConfirmedByAdmin";
                return (returnUrl, requestId);
            }

            //sign in existing user
            returnUrl = await SignInExistingUser(user, returnUrl);
            return (returnUrl, requestId);
        }

        public async Task<(string returnUrl, string requestId)> RegisterAsync(EAuthCallbackModel model)
        {
            string returnUrl = "";
            string requestId = "";
            try
            {
                if (model == null) throw new ArgumentNullException(nameof(EAuthCallbackModel));

                EAuthService service = new EAuthService();
                EAuthResponseModel eAuthResponseModel = service.Parse(model);
                requestId = eAuthResponseModel?.RequestId;
                returnUrl = _applicationStoreService.GetApplicationBaseUrl(requestId) + _registerReturnPath;
                string userType = _applicationStoreService.GetUserType(requestId);
                string chsiNumber = _applicationStoreService.GetChsiNumber(requestId);
                string lang = _applicationStoreService.GetLang(requestId);
                string email = _applicationStoreService.GetEmail(requestId);
                (string personIdentifier, string holderName, string certEmail) = ExtractPersonData(eAuthResponseModel);

                Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);

                if ((userType == UserType.CHSI.ToString() || userType == UserType.CHSIHelper.ToString()) && chsiNumber == default)
                {
                    returnUrl += $"?error=true";
                    return (returnUrl, requestId);
                }

                //check egn
                bool isDuplicateEgn = _userManager.Users.Any(u => u.Certificate_UniqueIdentifier == (personIdentifier ?? "").ToUpper().Trim() && !u.Deleted);
                if (isDuplicateEgn)
                {
                    returnUrl += $"?error=true&message=duplicateEgn";
                    return (returnUrl, requestId);
                }

                if (string.IsNullOrWhiteSpace(email))
                {
                    email = certEmail;
                }

                //check email
                if (string.IsNullOrWhiteSpace(email))
                {
                    //Redirect to register page with parameter to gather additional info
                    returnUrl += "?requireEmail=true";
                    return (returnUrl, requestId);
                }

                //Create new user
                ApplicationUser user = new ApplicationUser()
                {
                    Certificate = string.IsNullOrWhiteSpace(eAuthResponseModel.SamlResponse) ? null : Encoding.ASCII.GetBytes(eAuthResponseModel.SamlResponse),
                    Certificate_Thumbprint = eAuthResponseModel.RequestId,
                    Email = email,
                    EmailConfirmed = false,
                    UserName = GenerateUserName(holderName),
                    Certificate_Name = holderName.Trim(),
                    Certificate_UniqueIdentifier = personIdentifier?.Trim(),
                    AuthType = AuthType.EAUTH.ToString(),
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
                    //TODO Check CHSI in CHSI register by number
                    //Return error if chsi is not found
                    bool ChsiRegisterCheck = true;

                    user.CheckedInCHSIRegister = ChsiRegisterCheck;
                    user.ConfirmedByAdmin = ChsiRegisterCheck;
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
                    Log.Error("Error creating user with ESign", string.Join(',', ir.Errors.Select(x => x.Description)));
                    returnUrl += $"?error=true";
                    return (returnUrl, requestId);
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
                    await CreatePerson(user.Id, holderName, email);
                    //send confirmation email
                    await SendConfirmEmailAsync(user, _applicationStoreService.GetApplicationBaseUrl(requestId));

                    return (returnUrl, requestId);
                }
                catch (Exception x)
                {
                    //await AuditService.EditContentAndUserAsync(AuditId, AuditContentType.Activity,
                    //        AuditMessages.Failure.ToString(), user.Id, user.UserName);

                    Log.Error("Error creating person with EAuth", x);
                    IdentityResult deleteResult = await _userManager.DeleteAsync(user);
                    returnUrl += $"?error=true";
                    return (returnUrl, requestId);
                }

            }
            catch (Exception x)
            {
                if (x is ArgumentNullException)
                {
                    Log.Error("Empty EAuth response " + x.Message);
                    returnUrl += $"?error=true";
                    return (returnUrl, requestId);
                }

                Log.Error(x, "Error parsing EAuth response on register");
                //cannot parse EAuth response
                returnUrl += $"?error=true";
                return (returnUrl, requestId);
            }
        }

        public async Task<string> SignInExistingUser(ApplicationUser user, string returnUrl)
        {
            try
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var token = JWTManager.GenerateToken(user, userRoles, _tokenConfig);
                //await AuditService.EditContentAndUserAsync(AuditId,
                //                                           AuditContentType.Activity,
                //                                           AuditMessages.Success.ToString(),
                //                                           user.Id,
                //                                           user.UserName);

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
                return returnUrl;
            }
            catch
            {
                //await AuditService.EditContentAndUserAsync(AuditId, AuditContentType.Activity,
                //    AuditMessages.Failure.ToString(), user.Id, user.UserName);

                returnUrl += $"?error=true";
                return returnUrl;
            };
        }

        public async Task SendConfirmEmailAsync(ApplicationUser user, string returnUrl)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = $"{returnUrl}#account/emailConfirm?userId={user.Id}&code={code}";

            //TODO read email body from db
            _emailService.SendEmail(user.Email, null, null, _localizer.GetString("Email_ConfirmTitle"),
                string.Format(_localizer.GetString("Email_ConfirmBody"), HtmlEncoder.Default.Encode(callbackUrl)));

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

        private (string personIdentifier, string holderName, string email) ExtractPersonData(EAuthResponseModel eAuthResponseModel)
        {
            if (eAuthResponseModel == null) throw new ArgumentNullException(nameof(EAuthResponseModel));

            string holderName = eAuthResponseModel?.PersonNamesLatin;
            string email = eAuthResponseModel?.Email;
            string personIdentifier = null;

            if (!string.IsNullOrWhiteSpace(eAuthResponseModel?.PersonIdentifier))
            {
                string[] split = eAuthResponseModel.PersonIdentifier.Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
                personIdentifier = split.Length == 0
                    ? ""
                    : (split.Length == 1 ? split[0].Trim() : split.LastOrDefault()?.Trim());
            }

            return (personIdentifier, holderName, email);
        }
    }
}
