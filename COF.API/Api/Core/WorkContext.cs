using COF.BusinessLogic.Services;
using COF.BusinessLogic.Settings;
using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;
using COF.DataAccess.EF;
using System.Data.Entity;
using System.Web.Mvc;

namespace COF.API.Api.Core
{
    public class WorkContext : IWorkContext
    {
        private readonly HttpContextBase _httpContext;
        private readonly IOwinContext _owinContext;
        private readonly DbSet<AppUser> _appUsers;
        private readonly DbSet<Partner> _partners;

        /// <summary>
        /// WorkContext
        /// </summary>
        /// <param name="owinContext"></param>
        /// <param name="userService"></param>(
        public WorkContext(HttpContextBase httpContext, EFContext eFContext)
        {
            _httpContext = httpContext;
            _owinContext = httpContext.GetOwinContext();
            _appUsers = eFContext.Set<AppUser>();
            _partners = eFContext.Set<Partner>();
        }

        public string CurrentUserId
        {
            get
            {
                if (_owinContext.Authentication != null && _owinContext.Authentication.User.Identity.IsAuthenticated)
                {
                    var currentUserId = _owinContext.Authentication.User.Identity.GetUserId();
                    return currentUserId;
                }
                return "";
            }
        }

        private AppUser _currentUser;
        /// <summary>
        /// CurrentUser
        /// </summary>
        public AppUser CurrentUser
        {
            get
            {
                var isAuth = _owinContext.Authentication.User.Identity.IsAuthenticated;
                if (!isAuth)
                    return null;
                var currentUsername = _owinContext.Authentication.User.Identity.GetUserName();

                var user = _appUsers.FirstOrDefault(x => x.UserName == currentUsername);
                _currentUser = user;
                return user;
            }

        }
    }
}
