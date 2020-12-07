using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NRZ.Models;
using NRZ.Models.Identity;
using NRZ.Models.Person;
using NRZ.Models.Settings;
using NRZ.Services.Interfaces;
using NRZ.Shared;
using NRZ.Shared.Enums;
using NRZ.Shared.Localization;
using NRZ.Web.Attributes;
using NRZ.Web.Extensions;
using Serilog;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private const string AuthHeaderName = "Authorization";
        private readonly TokenConfig _tokenConfig;
        private readonly IPersonService _personService;
        private readonly IEmailService _emailService;

        //private readonly IClaimService _claimService;
        //private readonly IApplicationSettingsCacheService _applicationSettingsCacheService;

        public AccountController(UserManager<ApplicationUser> userManager,
            IOptions<TokenConfig> tokenConfig,
            IStringLocalizer<SharedResources> localizer,
            IPersonService personService,
            IEmailService mailService
            )
            : base(userManager, localizer)
        {
            _tokenConfig = tokenConfig.Value;
            _personService = personService;
            _emailService = mailService;
        }

        [AllowAnonymous]
        [Audit]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = await _userManager.FindByNameAsync(model.UserName);
                    if (user != null && !user.Deleted || user.AuthType != AuthType.USER.ToString())
                    {
                        if (!user.EmailConfirmed)
                        {
                            return StatusCode(StatusCodes.Status403Forbidden, _localizer["Error_EmailConfirmed"]);
                        }

                        bool checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
                        if (checkPassword)
                        {
                            try
                            {
                                var userRoles = await _userManager.GetRolesAsync(user);
                                var token = GenerateToken(user, userRoles);
                                await InvalidateToken(Request.Headers[AuthHeaderName]);

                                //var userClaims = await _claimService.ValidClaimsForUserAsync(user.Id);
                                Log.Information("User logged in successfully");
                                return Ok(new
                                {
                                    token = new JwtSecurityTokenHandler().WriteToken(token),
                                    userName = user.UserName,
                                    userType = user.UserType,
                                    authType = user.AuthType,
                                    email = user.Email,
                                    tokenExpiration = token.ValidTo,
                                    roles = userRoles,
                                    //userClaims = _claimService.ClaimsAsDictionary(userClaims),
                                    userId = user.Id
                                });
                            }
                            catch (Exception e)
                            {
                                Log.Information("User login error", e);
                                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                            }
                        }
                    }

                    return BadRequest("Wrong user name and/or password");
                }
                catch (Exception x)
                {
                    //TODO Log
                    return StatusCode(500, x.Message);
                }
            }

            return BadRequest("Fill all required fields");
        }

        [AllowAnonymous]
        [Audit]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid && model.UserType == UserType.AUCPAR.ToString())
            {
                try
                {
                    var userExists = await _userManager.Users.AnyAsync(x => x.UserName == model.UserName);
                    if (userExists)
                    {
                        string msg = $"Duplicate user"/*{_localizer.GetString("EmailUsedByAnotherUser").Value} ({model.Email})!*/;
                        return BadRequest(new Response() { Message = msg, StatusCode = 400 });
                    }

                    //Create application user
                    var user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        ConfirmedByAdmin = true,
                        AuthType = AuthType.USER.ToString(),
                        UserType = model.UserType,
                        CreatedOn = DateTime.UtcNow
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                    {
                        return BadRequest(new Response() { Message = "Error creating new user", StatusCode = 400 });
                    }

                    try
                    {
                        //TODO Assign to role
                        var roleAddResult = await _userManager.AddToRoleAsync(user, Constants.Role_AuctionParticipant);

                        if (!roleAddResult.Succeeded)
                        {
                            Log.Error("Error adding user to role", string.Join(',', roleAddResult.Errors.Select(x => x.Description)));
                            throw new Exception("Error adding user to role. " + string.Join(',', roleAddResult.Errors.Select(x => x.Description)));
                        }

                        //Create Person
                        PersonModel person = new PersonModel()
                        {
                            FirstName = model.FirstName,
                            MiddleName = model.MiddleName,
                            LastName = model.LastName,
                            Email = model.Email,
                            UserId = user.Id
                        };
                        await _personService.CreateAsync(person, user.Id);

                        //Send email
                        await SendConfirmEmailAsync(user);
                        return Ok();
                    }
                    catch (Exception x)
                    {
                        await _userManager.DeleteAsync(user);
                        Log.Error("Error creating Person on register", x.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                catch (Exception x)
                {
                    Log.Error("Error registering user", x.Message);
                    return BadRequest(x.Message);
                }
            }

            return BadRequest();
        }

        [Audit]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var (success, msg) = await InvalidateToken();

            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }

        [NonAction]
        private JwtSecurityToken GenerateToken(ApplicationUser user, IEnumerable<string> roles)
        {
            var claims = new List<System.Security.Claims.Claim>
                            {
                                new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, user.Id),
                                new System.Security.Claims.Claim(ClaimTypes.Name, user.UserName),
                                new System.Security.Claims.Claim(ClaimTypes.Email, user.Email)
                            };

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new System.Security.Claims.Claim(ClaimTypes.Role, role));
                }
            }

            var signingKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Secret));

            var token = new JwtSecurityToken
            (
                issuer: _tokenConfig.Issuer,
                audience: _tokenConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_tokenConfig.ExpirationHours),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        [NonAction]
        private async Task<(bool success, string msg)> InvalidateToken()
        {
            return await InvalidateToken(Request.Headers[AuthHeaderName]);
        }

        [NonAction]
        private async Task<(bool success, string msg)> InvalidateToken(string authorizationHeader)
        {
            if (string.IsNullOrWhiteSpace(authorizationHeader))
            {
                return (success: true, msg: "Nothing to invalidate");
            }

            string tokenString = authorizationHeader.Split(" ", StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            if (string.IsNullOrWhiteSpace(tokenString))
            {
                return (success: false, msg: $"Invalid {AuthHeaderName} request header value");
            }

            try
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenString);

                //TODO
                //await _tokenService.PutTokenInBlacklist(tokenString, token.ValidTo);

                return (success: true, msg: "");
            }
            catch (ArgumentNullException)
            {
                return (success: false, msg: "The Authorization token is null ot empty");
            }
            catch (ArgumentException)
            {
                return (success: false,
                    msg: "The Authorization token length exceeds the maximum token size or the token can`t be read ");
            }
            catch (Exception x)
            {
                return (success: false, msg: x.Message);
            }
        }

        [HttpPut("ChangePassword")]
        [Authorize]
        [Audit]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            string msg;
            if (!ModelState.IsValid)
            {
                msg = $"{_localizer.GetString("Invalid model validation").Value}!<br />{ModelState.GetErrors()}";
                return BadRequest(msg);
            }
            else
            {
                if (!String.Equals(model.NewPassword, model.NewPasswordConfirm))
                {
                    msg = "New password and confirmation do not match!";
                    return BadRequest(msg);
                }
            }

            try
            {
                var email = HttpContext?.User?.GetUserEmail();
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    try
                    {
                        IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            return Ok();
                        }
                        else
                        {
                            msg = String.Join(';', result.Errors.Select(x => x.Code + " / " + x.Description).ToArray());
                            return BadRequest(msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = $"{ex.Message} {ex.InnerException?.Message}";
                        return BadRequest(msg);
                    }
                }
                else
                {
                    return BadRequest($"User could not be verified! ({email})");
                }

            }
            catch (Exception e)
            {
                msg = $"BadRequest ({e.Message} {e.InnerException?.Message})";
                return BadRequest(msg);
            }
        }

        [HttpPut("ChangePasswordAdmin")]
        [Authorize]
        [Audit]
        public async Task<IActionResult> ChangePasswordAdmin(ChangePasswordAdminModel model)
        {
            string msg;
            if (!ModelState.IsValid)
            {
                msg = $"{_localizer.GetString("Invalid model validation").Value}!<br />{ModelState.GetErrors()}";
                return BadRequest(msg);
            }
            else
            {
                if (!String.Equals(model.NewPassword, model.NewPasswordConfirm))
                {
                    msg = "New password and confirmation do not match!";
                    return BadRequest(msg);
                }
            }

            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                bool isAdmin = HttpContext.User.IsInRole(Constants.Role_SysAdmin);

                if (isAdmin == false && user.Id != model.UserId)
                {
                    throw new UnauthorizedAccessException("User has not rights to performs this operation!");
                }

                if (user != null)
                {
                    try
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                        var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                        if (result.Succeeded)
                        {
                            return Ok();
                        }
                        else
                        {
                            msg = String.Join(';', result.Errors.Select(x => x.Code + " / " + x.Description).ToArray());
                            return BadRequest(msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = $"{ex.Message} {ex.InnerException?.Message}";
                        return BadRequest(msg);
                    }
                }
                else
                {
                    return BadRequest($"User could not be verified! ({user.Email})");
                }

            }
            catch (Exception e)
            {
                msg = $"BadRequest ({e.Message} {e.InnerException?.Message})";
                return BadRequest(msg);
            }
        }

        [AllowAnonymous]
        [HttpPost("sendResetPasswordMail/{userName?}")]
        [Audit]
        public async Task<IActionResult> SendResetPasswordMail([FromBody] ApplicationUser model)
        {
            if (model == null || String.IsNullOrEmpty(model.UserName))
                return BadRequest("Missing username!");

            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    await SendResetPasswordEmailAsync(user);

                    string msg = $"Reset token generated!";
                    return Ok(msg);
                }
                else
                {
                    return BadRequest("User not found");
                }
            }
            catch (Exception e)
            {
                string msg = $"BadRequest ({e.Message}; {e.InnerException?.Message})";
                return BadRequest(msg);
            }


        }

        [AllowAnonymous]
        [Audit]
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ForgottenPasswordModel model)
        {
            string msg;

            if (!ModelState.IsValid)
            {
                msg = $"{_localizer.GetString("Invalid model validation").Value}!<br />{ModelState.GetErrors()}";
                return BadRequest(msg);
            }
            else
            {
                if (!String.Equals(model.NewPassword, model.NewPasswordConfirm))
                {
                    msg = "New password and confirmation do not match!";
                    return BadRequest(msg);
                }
            }

            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    var code = model.Token.Replace(" ", "+");
                    var resetResult = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
                    if (resetResult.Succeeded)
                    {
                        //return Ok("Password is reset");

                        var userRoles = await _userManager.GetRolesAsync(user);
                        var token = GenerateToken(user, userRoles);
                        await InvalidateToken(Request.Headers[AuthHeaderName]);

                        var data = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            userName = user.UserName,
                            tokenExpiration = token.ValidTo,
                            userId = user.Id
                        };

                        return Ok(data);
                    }
                    else
                    {
                        msg = $"Reset password errors: {String.Join("; ", resetResult.Errors.Select(x => x.Code + "/" + x.Description))}";
                        return BadRequest(msg);
                    }
                }
                else
                {
                    return BadRequest("User not found");
                }
            }
            catch (Exception e)
            {
                msg = $"BadRequest ({e.Message} {e.InnerException?.Message})";
                return BadRequest(msg);
            }

        }

        [Audit]
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return BadRequest($"Error confirming email for user with ID '{userId}':");
            }

            return Ok(new { userType = user.UserType, isApproved = user.ConfirmedByAdmin });
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

        private async Task SendResetPasswordEmailAsync(ApplicationUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = System.Net.WebUtility.UrlEncode(code);
            var callbackUrl = Request.Headers["Referer"];
            callbackUrl += $"#/resetPassword?userId={user.Id}&code={encodedToken}";

            _emailService.SendEmail(user.Email, null, null, _localizer.GetString("ResetPasswordEmailTitle"),
                string.Format(_localizer.GetString("ResetPasswordEmail"), HtmlEncoder.Default.Encode(callbackUrl)));
        }
    }
}
