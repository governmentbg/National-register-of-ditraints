using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NRZ.Models;
using NRZ.Models.Identity;
using NRZ.Shared.Localization;
using NRZ.Web.Attributes;
using NRZ.Web.Extensions;

namespace NRZ.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Audit]
    public class BaseApiController : ControllerBase
    {
        protected readonly IStringLocalizer<SharedResources> _localizer;
        protected readonly UserManager<ApplicationUser> _userManager;

        protected string CurrentUserId => HttpContext?.User?.GetUserId();
        protected string CurrentUserEmail => HttpContext?.User?.GetUserEmail();
        protected string CurrentUserUsername => HttpContext?.User?.GetUserUsername();

        public BaseApiController(UserManager<ApplicationUser> userManager = null,
            IStringLocalizer<SharedResources> localizer = null)
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        protected bool ValidateObject<T>(T model)
        {
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(model, context, validationResults, true);
        }

        [NonAction]
        protected async Task<List<AttachmentModel>> ParseAttachmentsAsync(IEnumerable<IFormFile> files)
        {
            List<AttachmentModel> docs = new List<AttachmentModel>();

            foreach (var file in files)
            {
                docs.Add(await ParseAttachmentAsync(file));
            }

            return docs;
        }

        [NonAction]
        protected async Task<AttachmentModel> ParseAttachmentAsync(IFormFile file)
        {
            var result = new AttachmentModel();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);


                result.FileName = file.FileName;
                result.Type = file.ContentType;
                result.Content = stream.ToArray();
            }

            return result;
        }
    }
}
