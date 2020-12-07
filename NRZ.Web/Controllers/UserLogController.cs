using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NRZ.Models.GridModels;
using NRZ.Services.Interfaces;
using NRZ.Web.Attributes;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Audit]
    public class UserLogsController : ControllerBase
    {
        private readonly ILogService logService;
        public UserLogsController(ILogService logService)
        {
            this.logService = logService;
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> Search(GridSearchModel searchModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

           var result = await logService.SearchLogs(searchModel);
            return Ok(result);
        }
    }
}
