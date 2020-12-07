using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRZ.Web.Extensions
{
    public static class ApiControllerExtensions
    {
        public static string GetErrors(this ModelStateDictionary modelState)
        {
            var errors = modelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            return string.Join("<br />", errors);
        }

        public static string GetLanguageHeader(this Microsoft.AspNetCore.Http.HttpContext context)
        {
            return context.Request.Headers["Accept-Language"].ToString();
        }
    }
}
