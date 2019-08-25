using COF.API;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace COF.API.Core
{
    public class ApiControllerBase : ApiController
    {

        public ApiControllerBase()
        {
            
        }

        //Code removed from brevity

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        protected ApplicationSignInManager SignInManager
        {
            get
            {
                return Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }

        [NonAction]
        public HttpResponseMessage SuccessResult(object result = null)
        {
            var formatter = GetFormatter();
            var resp = Request.CreateResponse(HttpStatusCode.OK, new OkResponseModel
            {
                Status = true,
                Data = result
            }, GetFormatter());


            return resp;
        }

        [NonAction]
        public HttpResponseMessage ErrorResult(string message)
        {
            var messages = new List<string> { message };
            var formatter = GetFormatter();
            var resp = Request.CreateResponse(HttpStatusCode.OK, new ErrorResponseModel
            {
                Status = false,
                ErrorMessages = messages
            }, GetFormatter());
            return resp;
        }

        [NonAction]
        public HttpResponseMessage ErrorResult(List<string> messages)
        {
            var formatter = GetFormatter();
            var resp = Request.CreateResponse(HttpStatusCode.OK, new ErrorResponseModel
            {
                Status = false,
                ErrorMessages = messages
            }, GetFormatter());
            return resp;
        }

        private JsonMediaTypeFormatter GetFormatter()
        {
            return new JsonMediaTypeFormatter()
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = new List<JsonConverter>
                    {
                        new StringEnumConverter()
                    },
                    NullValueHandling = NullValueHandling.Ignore
                }
            };
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

   public class OkResponseModel
   {
        public object Data { get; set; }
        public bool Status { get; set; }
   }

    public class ErrorResponseModel
    {
        public List<string> ErrorMessages { get; set; }
        public bool Status { get; set; }
    }
}