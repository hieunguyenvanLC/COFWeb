using Hangfire.Dashboard;
using System.Web;

namespace COF.API.Filter.Hangfire
{
    public class HangfireAuthFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var user = HttpContext.Current.User;

            return user != null && user.IsInRole("PartnerAdmin") && user.Identity.IsAuthenticated;
        }
    }
}