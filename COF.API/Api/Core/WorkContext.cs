using COF.BusinessLogic.Services;
using COF.BusinessLogic.Settings;
using COF.DataAccess.EF;
using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Http;

namespace COF.API.Api.Core
{
    public class WorkContext : IWorkContext
    {
        private readonly IOwinContext _owinContext;
        private readonly DbSet<AppUser> _userDbSet;

        /// <summary>
        /// WorkContext
        /// </summary>
        /// <param name="owinContext"></param>
        /// <param name="userService"></param>
        public WorkContext(EFContext context)
        {
            _owinContext = (IOwinContext) GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOwinContext));
            _userDbSet = context.Set<AppUser>();
        }

        public string CurrentUserId
        {
            get
            {
                if (_owinContext.Authentication != null && _owinContext.Authentication.User.Identity.IsAuthenticated)
                {
                    var currentUserId = _owinContext.Authentication.User.GetValueOfClaim(ClaimName.UserIdKey);
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
                var currentUsername = _owinContext.Authentication.User.GetValueOfClaim(ClaimName.UserNameKey);

                var user = _userDbSet.FirstOrDefault(x => x.UserName == currentUsername);
                _currentUser = user;
                return user;
            }

        }
    }
    /// Static class for getting principal
    /// </summary>
    public static class GenericPrincipalExtensions
    {
        /// <summary>
        /// Get value of claim
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claimName"></param>
        /// <returns></returns>
        public static string GetValueOfClaim(this IPrincipal user, string claimName)
        {
            if (user.Identity.IsAuthenticated)
            {
                var claimsIdentity = user.Identity as ClaimsIdentity;
                if (claimsIdentity == null) return "";
                foreach (var claim in claimsIdentity.Claims)
                {
                    if (claim.Type == claimName)
                        return claim.Value;
                }
                return "";
            }
            else
                return "";
        }
    }


    public static class ClaimName
    {
        public static string RefreshTokenKey = "refresh_token_Id";
        public static string UserNameKey = "Username";
        public static string UserIdKey = "UserId";
        public static string EmailKey = "Email";

    }
}
