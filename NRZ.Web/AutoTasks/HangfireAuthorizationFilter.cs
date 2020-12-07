using Hangfire.Dashboard;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace NRZ.Web.AutoTasks
{
    public class HangfireAuthorizationFilter : ControllerBase, IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
            //try
            //{
            //    var httpContext = context.GetHttpContext();
            //    var userRole = httpContext.Request.Cookies["UserRole"];
            //    return false;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        //public bool Authorize(DashboardContext context)
        //{
        //    // Security на дашборда на hangfire.
        //    return context.GetHttpContext().User.Identity.IsAuthenticated;

        //    //return httpContext.User.IsInRole(Role.DevAdmin);
        //}
    }
}
