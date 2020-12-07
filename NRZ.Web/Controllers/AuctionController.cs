using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using NRZ.Web.Extensions;
using Serilog;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuctionsController : BaseApiController
    {
        private readonly IAuctionService _auctionService;
        public AuctionsController(IAuctionService auctionService,
                                 IStringLocalizer<SharedResources> localizer)
            :base(null, localizer)
        {
            _auctionService = auctionService;
        }

        [Route("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            try
            {
                    return Ok(_auctionService.Get(id));
            }
            catch (Exception x)
            {
                Log.Error(x, $"Error getting auction with ID = {id}");
                return StatusCode(500, x.Message);
            }
        }

        [AllowAnonymous]
        [Route("Future")]
        public IActionResult GetFutureAuctions([FromQuery] bool creator = false)
        {
            try
            {
                string userId = User.Identity.IsAuthenticated ? User.GetUserId() : null;
                return Ok(_auctionService.GetFutureAuctions(creator ? null : userId , creator ? userId : null));
            }
            catch (Exception x)
            {
                Log.Error(x, $"Error getting future auctions");
                return StatusCode(500, x.Message);
            }
        }

        [AllowAnonymous]
        [Route("Finished")]
        public IActionResult GetFinishedAuctions([FromQuery] bool creator = false)
        {
            try
            {
                string userId = User.Identity.IsAuthenticated ? User.GetUserId() : null;
                return Ok(_auctionService.GetFinishedAuctions(creator ? null : userId, creator ? userId : null));
            }
            catch (Exception x)
            {
                Log.Error(x, $"Error getting finished auctions");
                return StatusCode(500, x.Message);
            }
        }

        [AllowAnonymous]
        [Route("Ongoing")]
        public IActionResult GetOngoingAuctions([FromQuery] bool creator = false)
        {
            try
            {
                string userId = User.Identity.IsAuthenticated ? User.GetUserId() : null;
                return Ok(_auctionService.GetOngoingAuctions(creator ? null : userId, creator ? userId : null));
            }
            catch (Exception x)
            {
                Log.Error(x, $"Error getting ongoing auctions");
                return StatusCode(500, x.Message);
            }
        }

        [Route("User/{id?}")]
        public IActionResult GetByUser([FromRoute] string id)
        {
            try
            {
                return Ok(_auctionService.GetByUserId(User.GetUserId()));
            }
            catch (Exception x)
            {
                Log.Error(x, $"Error getting all auctions");
                return StatusCode(500, x.Message);
            }
        }
    }
}
