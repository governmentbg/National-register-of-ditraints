using Microsoft.AspNetCore.Mvc;
using NRZ.Web.Attributes;
using System.Threading.Tasks;
using NRZ.Models.AgriculturalMachinery;
using NRZ.Web.Extensions;
using NRZ.Services.Interfaces;
using System;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Audit]
    public class AgriculturalMachineryController : ControllerBase
    {
        private readonly IAgriculturalMachineryService agriculturalMachineryService;

        public AgriculturalMachineryController(IAgriculturalMachineryService agriculturalMachineryService)
        {
            this.agriculturalMachineryService = agriculturalMachineryService;
        }

        [HttpPost]
        [Route("Create")]
        [Audit]
        public async Task<IActionResult> Create(AgriculturalMachineryModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = this.User.GetUserId();

                    await this.agriculturalMachineryService.CreateAsync(model, userId);
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
            }
            else
            {
                var msg = ModelState.GetErrors();
                var error = StatusCode(500, msg);

                return error;
            }

            return Ok();
        }

        [HttpPost]
        [Route("GetAllMachineries")]
        [Audit]
        public async Task<IActionResult> GetAllMachineries(AgriculturalMachineryListingRequestModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(this.agriculturalMachineryService.GetAll(model));

                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);

                }
            }
            else
            {
                var msg = ModelState.GetErrors();
                var error = StatusCode(500, msg);

                return error;
            }

        }


        [HttpGet("GetMachineById/{itemId}")]
        [Audit]
        public async Task<IActionResult> GetMachineById(int itemId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var machine = await this.agriculturalMachineryService.GetMachineByIdAsync(itemId);
                    return Ok(machine);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                var msg = ModelState.GetErrors();
                var error = StatusCode(500, msg);

                return error;
            }
        }

        [HttpDelete("DeleteMachineById/{id}")]
        [Audit]
        public async Task<IActionResult> DeleteMachineById(int id) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = this.User.GetUserId();


                    if (userId == null)
                    {
                        throw new NullReferenceException("User does not exist!");
                    }

                    await this.agriculturalMachineryService.DeleteAsync(id, userId);
                    return Ok();
                }
                catch (Exception e )
                {
                    var msg = e;
                    var error = StatusCode(500, msg);

                    return error;
                }
            }
            else
            {
                var msg = ModelState.GetErrors();
                var error = StatusCode(500, msg);

                return error;
            }
        }

        [HttpPut("EditMachine")]
        [Audit]
        public async Task<IActionResult> EditMachine(AgriculturalMachineryWithOwnerNameModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = this.User.GetUserId();

                    if (userId == null)
                    {
                        throw new NullReferenceException("User does not exist");
                    }

                    await this.agriculturalMachineryService.EditMachineAsync(model, userId);
                    return Ok();
                }
                catch (Exception e)
                {
                    var msg = e;
                    var error = StatusCode(500, msg);

                    return error;
                }
            }
            else
            {
                var msg = ModelState.GetErrors();
                var error = StatusCode(500, msg);

                return error;
            }
        }
    }
}
