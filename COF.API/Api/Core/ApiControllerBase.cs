using COF.API;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
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

    }
}