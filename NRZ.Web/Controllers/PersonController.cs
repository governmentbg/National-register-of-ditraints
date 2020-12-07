using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using NRZ.Models.Person;
using NRZ.Services.Interfaces;
using NRZ.Shared;
using NRZ.Web.Attributes;
using NRZ.Web.Extensions;
using Serilog;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : BaseApiController
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [Audit]
        [HttpGet("GetPersonByUserId/{userId}")]
        public async Task<IActionResult> GetPersonByUserId(string userId)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(userId))
                {

                    throw new NullReferenceException("Id can not be null!");
                }
                var person =await _personService.GetPersonModelAsyncByUserId(userId);
                return Ok(person);
            }
            catch (Exception e)
            {
                Log.Error($"ERROR Person/GetPersonByUserId/{userId}", e);
                return StatusCode(500, e.Message);
            }
        }

        [Audit]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PersonModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model == null)
                    {
                        throw new NullReferenceException("Model can not be null!");
                    }
                    var userId = HttpContext?.User?.GetUserId();
                    await _personService.CreateAsync(model, userId ?? null);

                    return Ok();
                }
                catch (Exception e)
                {
                    Log.Error($"ERROR Person/Create", e);
                    return StatusCode(500, e.Message);
                }
            }
            else
            {
                string msg = $"ERROR Fill all required fields!";
                Log.Error(msg);
                return StatusCode(500, msg);
            }
        }


        [Audit]
        [HttpPut]
        public async Task<IActionResult> Edit(PersonModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model == null)
                    {
                        throw new NullReferenceException("Model can not be null!");
                    }

                    var userId = HttpContext?.User?.GetUserId();
                    if (userId == null)
                    {
                        throw new NullReferenceException("User can not be find!");
                    }

                    bool isAdmin = HttpContext.User.IsInRole(Constants.Role_SysAdmin);

                    if (isAdmin == false && userId != model.UserId)
                    {
                        throw new UnauthorizedAccessException("User has not rights to performs this operation!");
                    }

                    await _personService.EditAsync(model, userId);

                    return Ok();
                }
                catch(UnauthorizedAccessException e)
                {
                    Log.Error($"ERROR Person/Edit/{model.UserId}", e);
                    return StatusCode(403, e.Message);
                }
                catch (Exception e)
                {
                    Log.Error($"ERROR Person/Edit/{model.UserId}", e);
                    return StatusCode(500, e.Message);
                }
            }
            else
            {
                string msg = $"ERROR Fill all required fields!";
                Log.Error(msg);
                return StatusCode(500, msg);
            }
        }

        [HttpGet("getFromRegix/{identifier}")]
        [Audit]
        public async Task<IActionResult> GetPersonFromRegix(string identifier)
        {
            try
            {
                var person = await _personService.GetPersonFromRegixAsync(identifier);
                return Ok(person);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"ERROR getting person from Regix by user {CurrentUserId}/{CurrentUserUsername}");
                return BadRequest("Error getting person from Regix");
            }
        }


    }
}
