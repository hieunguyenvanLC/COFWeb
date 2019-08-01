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

        public JsonResult HttpGetSuccessResponse(object data = null, string message = "")
        {
            return Json(new
            {
                status = true,
                message = message,
                data = data
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult HttpPostSuccessResponse(object data = null, string message = "")
        {
            return Json(new
            {
                status = true,
                message = message,
                data = data
            });
        }

        public JsonResult HttpGetErrorResponse(string message)
        {
            return Json(new
            {
                status = false,
                errorMessage = message
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HttpPostErrorResponse(string message)
        {
            return Json(new
            {
                status = false,
                errorMessage = message
            });
        }   

        protected string ModelStateErrorMessage()
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys.Where(key => ModelState[key].Errors.Count > 0))
                {
                   var errorMessage = (!string.IsNullOrEmpty(ModelState[key].Errors[0].ErrorMessage)
                        ? ModelState[key].Errors[0].ErrorMessage
                        : ModelState[key].Errors[0].Exception.Message);
                    return errorMessage;
                }
            }
            return "";
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