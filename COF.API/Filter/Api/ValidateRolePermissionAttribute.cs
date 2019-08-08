using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace COF.API.Filter.Api
{
    public class ValidateRolePermissionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// RoleClaim
        /// </summary>
        public string RoleClaim { get; set; }

        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            if (principal != null)
            {
                var currentRoleInClaim = principal.FindFirst(ClaimTypes.Role);

                if (currentRoleInClaim == null || string.IsNullOrEmpty(currentRoleInClaim.Value))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        new
                        {
                            message = "You dont have permission for execute this action"
                        },
                        actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                        );
                }
                else
                {
                    var listRole = RoleClaim.Split(',');
                    foreach (var role in listRole)
                    {
                        if (currentRoleInClaim.Value.IndexOf(role, StringComparison.Ordinal) > -1)
                        {
                            return;
                        }
                    }
                    actionContext.Response = actionContext.Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        new
                        {
                            message = "You dont have permission for execute this action"
                        },
                        actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                        );

                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.Unauthorized,
                    new
                    {
                        message = "You dont have permission for execute this action"
                    },
                    actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                    );
            }

        }

    }
}