using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using NRZ.Data;
using NRZ.Models.Identity;
using NRZ.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRZ.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuditAttribute : TypeFilterAttribute

    {
        public AuditAttribute() : base(typeof(AuditAttributeImpl))
        {
            Arguments = new object[]
            {
                
            };
        }

        private class AuditAttributeImpl : IAsyncActionFilter
        {
            private readonly ILogService _logService;
            private readonly UserManager<ApplicationUser> _userManager;

            private long AuditId { get; set; }
            private DateTime StartTime { get; set; }

            public AuditAttributeImpl(ILogService logService, UserManager<ApplicationUser> userManager)
            {
                _logService = logService;
                _userManager = userManager;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                StartTime = DateTime.Now;
                var audit = CreateModel(context);

                if (audit != null)
                {
                    AuditId = await _logService.Track(audit);

                    // Достъп до полето се осъществява чрез RouteData.Values["AuditID"];
                    if (!context.RouteData.Values.ContainsKey("AuditID"))
                    {
                        context.RouteData.Values.Add("AuditID", AuditId);
                    }
                    else
                    {
                        context.RouteData.Values["AuditID"] = AuditId;
                    }
                }

                var resultContext = await next();
                if (AuditId > 0)
                {
                    var duration = (DateTime.Now - StartTime);
                    await _logService.UpdateDurationAndStatusCodeAsync(AuditId, duration, context.HttpContext.Response.StatusCode);
                }
            }

            private UserLogs CreateModel(ActionExecutingContext context)
            {
                string userId = null;
                var user = context.HttpContext.User;
                var identity = user?.Identity;
                if (identity != null && identity.IsAuthenticated && _userManager != null)
                {
                    userId = _userManager.GetUserId(user);
                }

                var request = context.HttpContext.Request;

                UserLogs audit = new UserLogs()
                {
                    ActionName = (string)context.RouteData.Values["action"],
                    Controller = (string)context.RouteData.Values["controller"],
                    Data = JsonConvert.SerializeObject(new { request.Cookies, request.Headers, request.QueryString }),
                    Ip = request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    RequestMethod = request.Method,
                    ServerTimeUtc = DateTime.UtcNow,
                    UserId = userId
                };

                return audit;
            }
        }
    }
}
