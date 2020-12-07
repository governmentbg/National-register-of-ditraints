using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NRZ.Models.Auction;
using NRZ.Services.Interfaces;
using NRZ.Web.Attributes;
using NRZ.Web.Extensions;
using Serilog;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : BaseApiController
    {
        private readonly IAnnouncementsService _announcementService;

        public AnnouncementsController(IAnnouncementsService announcementsService)
        {
            _announcementService = announcementsService;
        }

        [HttpGet]
        [Route("{id?}")]
        public async Task<IActionResult> Get([FromRoute] int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    return Ok(await _announcementService.Get(id.Value));
                }
                else
                {
                    return Ok(_announcementService.GetAll().OrderByDescending(x => x.Id));
                }
            }
            catch (Exception x)
            {
                Log.Error($"ERROR Getting announcement with ID {id}", x);
                return StatusCode(500, x.Message);
            }
        }

        [HttpGet]
        [Route("user/{id}")]
        public IActionResult GetByUser([FromRoute] string id)
        {
            try
            {
                return Ok(_announcementService.GetByUserId(id).OrderByDescending(x => x.Id));
            }
            catch (Exception x)
            {
                Log.Error($"ERROR Getting announcements with for user ID {id}", x);
                return StatusCode(500, x.Message);
            }
        }

        [Audit]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] AuctionAnnouncementModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (Request.Form.Files.Count() > 0)
                    {
                        model.Attachments = await ParseAttachmentsAsync(Request.Form.Files);
                    }

                    await _announcementService.Create(model, User.GetUserId());
                    return Ok();
                }
                catch (Exception x)
                {
                    Log.Error("ERROR Creating announcement", x);
                    return StatusCode(500, x.Message);
                }
            }

            return BadRequest();
        }

        [Audit]
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] AuctionAnnouncementModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Request.Form.Files.Count() > 0)
                    {
                        var newAttachments = await ParseAttachmentsAsync(Request.Form.Files);
                        var allAttachments = model.Attachments.ToList();
                        allAttachments.AddRange(newAttachments);

                        model.Attachments = allAttachments;
                    }

                    await _announcementService.Edit(model, User.GetUserId());
                    return Ok();
                }
                catch (Exception x)
                {
                    Log.Error($"ERROR Updating announcement with ID {model.Id}", x);
                    return StatusCode(500, x.Message);
                }
            }

            return BadRequest();
        }

        [Audit]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _announcementService.Delete(id, User.GetUserId());
                return Ok();
            }
            catch (Exception x)
            {
                Log.Error($"ERROR Delete entry {id}", x);
                return StatusCode(500, x.Message);
            }
        }
    }
}
