using COF.BusinessLogic.Settings;
using COF.DataAccess.EF.Models;

namespace COF.API.Api.Core
{
    //public class WorkContext : IWorkContext
    //{
    //    private readonly HttpContextBase _httpContext;
    //    private readonly IOwinContext _owinContext;
    //    private readonly DbSet<AppUser> _appUsers;
    //    private readonly DbSet<Partner> _partners;

    //    /// <summary>
    //    /// WorkContext
    //    /// </summary>
    //    /// <param name="owinContext"></param>
    //    /// <param name="userService"></param>(
    //    public WorkContext(HttpContextBase httpContext, EFContext eFContext)
    //    {
    //        _httpContext = httpContext;
    //        _owinContext = httpContext.GetOwinContext();
    //        _appUsers = eFContext.Set<AppUser>();
    //        _partners = eFContext.Set<Partner>();
    //    }

    //    public string CurrentUserId
    //    {
    //        get
    //        {
    //            if (_owinContext.Authentication != null && _owinContext.Authentication.User.Identity.IsAuthenticated)
    //            {
    //                var currentUserId = _owinContext.Authentication.User.Identity.GetUserId();
    //                return currentUserId;
    //            }
    //            return "";
    //        }
    //    }

    //    private AppUser _currentUser;
    //    /// <summary>
    //    /// CurrentUser
    //    /// </summary>
    //    public AppUser CurrentUser
    //    {
    //        get
    //        {
    //            var isAuth = _owinContext.Authentication.User.Identity.IsAuthenticated;
    //            if (!isAuth)
    //                return null;
    //            var currentUsername = _owinContext.Authentication.User.Identity.GetUserName();

    //            var user = _appUsers.FirstOrDefault(x => x.UserName == currentUsername);
    //            _currentUser = user;
    //            return user;
    //        }

    //    }
    //}

    public class WorkContext : IWorkContext
    {
        public WorkContext()
        {
           
        }

        public string CurrentUserId => "Test";

        private AppUser _currentUser;

        /// <summary>
        /// CurrentUser
        /// </summary>
        public AppUser CurrentUser => null;
    }
}
