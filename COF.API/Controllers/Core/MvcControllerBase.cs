using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
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
    }
}