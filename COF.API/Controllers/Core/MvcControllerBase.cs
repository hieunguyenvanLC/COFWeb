using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COF.API.Controllers.Core
{
    public class MvcControllerBase : Controller
    {
        public MvcControllerBase()
        {

        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        protected int? ShopId { get; set; }

        public JsonResult HttpGetSuccessResponse(object data, string message = "")
        {
            return Json(new
            {
                status = true,
                message = message,
                data = data
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult HttpPostSuccessResponse(object data, string message)
        {
            return Json(new
            {
                status = true,
                message = message,
                data = data
            });
        }

        public JsonResult HttpGetErrorResponse(object data, string message)
        {
            return Json(new
            {
                status = false,
                errorMessage = message
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HttpPostErrorResponse(object data, string message)
        {
            return Json(new
            {
                status = false,
                errorMessage = message
            });
        }   
    }
    public static class CamelCaseResult
    {
        public static object Convert<TEntity>(TEntity data)
        {
            var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return JsonConvert.DeserializeObject<object>(JsonConvert.SerializeObject(data, Formatting.Indented, jsonSerializerSettings));
        }
    }
}