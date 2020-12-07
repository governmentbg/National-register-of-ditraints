using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NRZ.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal) =>
            principal.FindFirstValue(ClaimTypes.NameIdentifier);

        public static string GetUserEmail(this ClaimsPrincipal principal) =>
            principal.FindFirstValue(ClaimTypes.Email);

        public static string GetUserUsername(this ClaimsPrincipal principal) =>
            principal.FindFirstValue(ClaimTypes.Name);
    }
}
