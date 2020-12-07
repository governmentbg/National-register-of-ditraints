using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NRZ.Models.Identity;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using NRZ.Web.Attributes;
using NRZ.Web.Extensions;
using Serilog;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        public UserController(UserManager<ApplicationUser> userManager,
             IStringLocalizer<SharedResources> localizer,
            IUserService userService) : base(userManager, localizer)
        {
            _userService = userService;
        }


        [Route("Get/{id?}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return Ok(await _userService.GetAll().ToListAsync());
                }
                else
                {
                    return Ok(await _userService.GetByIdAsync(id));
                }
            }
            catch (Exception x)
            {
                Log.Error($"ERROR User/Get/{id}", x);
                return StatusCode(500, x.Message);
            }
        }

        [AllowAnonymous]
        [Route("GetByUserName/{userName?}")]
        public async Task<IActionResult> GetByUserName([FromRoute] string userName)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    return Ok(await _userService.GetByUserNameAsync(userName.Trim().ToLower()));
                }
                return Ok(null);
            }
            catch (Exception x)
            {
                Log.Error($"ERROR User/GetByUserName/{userName}", x);
                return StatusCode(500, x.Message);
            }
        }

        [Route("GetUserRolesById/{id?}")]
        public async Task<IActionResult> GetUserRolesById([FromRoute] string id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    var user = await _userService.GetUserRoleByIdAsync(id);
                    return Ok(user);
                }
                else
                {
                    return Ok(new ApplicationUserRoles());
                }
            }
            catch (Exception x)
            {
                Log.Error($"ERROR User/GetUserRolesById/{id}", x);
                return StatusCode(500, x.Message);
            }
        }

        [HttpPost]
        [Route("Approve/{id}")]
        public async Task<IActionResult> Approve([FromRoute] string id)
        {
            try
            {
                await _userService.AproveUser(id, User.GetUserId());
                return Ok();
            }
            catch (Exception x)
            {
                Log.Error($"ERROR User/Approve/{id}", x);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("Deactivate/{id}")]
        public async Task<IActionResult> Deactivate([FromRoute] string id)
        {
            try
            {
                await _userService.DeleteUser(id, User.GetUserId());
                return Ok();
            }
            catch (Exception x)
            {
                Log.Error($"ERROR User/Deactivate/{id}", x);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("Activate/{id}")]
        public async Task<IActionResult> Activate([FromRoute] string id)
        {
            try
            {
                await _userService.ActivateUser(id, User.GetUserId());
                return Ok();
            }
            catch (Exception x)
            {
                Log.Error($"ERROR User/Activate/{id}", x);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("GetUsersByRoleName/{roleName}")]
        public async Task<IActionResult> GetUsersByRoleName(string roleName)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(roleName))
                {
                    throw new NullReferenceException(_localizer["LoadingDataError"]);
                }
                var users = await _userManager.GetUsersInRoleAsync(roleName);
                return Ok(users);

            }
            catch (Exception x)
            {
                Log.Error($"ERROR Roles/CreateRole");
                return StatusCode(500, x.Message);
            }
        }
    }
}
