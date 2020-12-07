using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Models.Identity;
using NRZ.Models.Role;
using NRZ.Models.User;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using Serilog;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BaseApiController
    {
        private readonly IRoleService _roleService;
        private RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<ApplicationUser> userManager,
            IStringLocalizer<SharedResources> localizer,
            IRoleService roleService, RoleManager<IdentityRole> roleManager) : base(userManager, localizer)
        {
            _roleService = roleService;
            _roleManager = roleManager;
        }

        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles()
        {
            try
            {
                var roles = _roleService.All();
                return Ok(roles);
            }
            catch (Exception x)
            {
                Log.Error($"ERROR Roles/GetUserRoles");
                return StatusCode(500, x.Message);
            }
        }

        [Route("GetRolesByUserId/{id?}")]
        public IActionResult GetRolesByUserId(string id)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(id))
                {
                    throw new NullReferenceException(_localizer["LoadingDataError"]);
                }
                var roles = _roleService.GetRolesByUserId(id);
                return Ok(roles);
            }
            catch (Exception x)
            {
                Log.Error($"ERROR Roles/GetRolesByUserId/{0}", id);
                return StatusCode(500, x.Message);
            }
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (role == null)
                    {
                        throw new NullReferenceException(_localizer["InvalidModel"]);
                    }
                    var aspNetRole = await _roleManager.CreateAsync(role);
                    return Ok(aspNetRole);
                }
                catch (Exception x)
                {
                    Log.Error($"ERROR Roles/CreateRole");
                    return StatusCode(500, x.Message);
                }
            }
            else
            {
                Log.Error($"ERROR Roles/CreateRole");
                return StatusCode(500, _localizer["InvalidModel"]);
            }

        }

        [HttpPut]
        [Route("EditRole")]
        public async Task<IActionResult> EditRole([FromBody] AspNetRoleModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model == null)
                    {
                        throw new NullReferenceException(_localizer["InvalidModel"]);
                    }

                    await _roleService.EditRole(model);
                    return Ok();
                }
                catch (Exception x)
                {
                    Log.Error($"ERROR Roles/CreateRole");
                    return StatusCode(500, x.Message);
                }
            }
            else
            {
                return BadRequest(_localizer["FillRequiredFields"]);
            }

        }

        [HttpPut]
        [Route("EditUserRoles")]
        public async Task<IActionResult> EditUserRoles([FromBody] AspNetUserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model == null)
                    {
                        throw new NullReferenceException(_localizer["InvalidModel"]);
                    }

                    await _roleService.EditUserRoleByUser(model);
                    return Ok();
                }
                catch (Exception x)
                {
                    Log.Error($"ERROR Roles/CreateRole");
                    return StatusCode(500, x.Message);
                }
            }
            else
            {
                return BadRequest(_localizer["FillRequiredFields"]);
            }

        }
    }
}
