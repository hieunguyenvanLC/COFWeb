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
        private readonly IPrincipal _principal;
        private readonly DbSet<AppUser> _appUsers;

        /// <summary>
        /// WorkContext
        /// </summary>
        /// <param name="owinContext"></param>
        /// <param name="userService"></param>(
        public WorkContext( EFContext eFContext)
        {
            _principal = DependencyResolver.Current.GetService<IPrincipal>();
            _appUsers = eFContext.Set<AppUser>();
        }

        public string CurrentUserId
        {
            get
            {
                return _principal.Identity.GetUserId();
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
                var isAuth = _principal.Identity.IsAuthenticated;
                if (!isAuth)
                    return null;
                var currentUsername = _principal.Identity.GetUserName();

                var user = _appUsers.FirstOrDefault(x => x.UserName == currentUsername);
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
