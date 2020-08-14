using Autofac;
using Autofac.Integration.Mvc;
using COF.API.Api.Core;
using COF.BusinessLogic.Settings;
using COF.DataAccess.EF;
using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;

namespace COF.API.AutofacModules
{
    public class IdentityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RoleStore<AppRole>>().As<IRoleStore<AppRole, string>>();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<AppUser>>();
            builder.RegisterType<ApplicationUserManager>();
            builder.RegisterType<ApplicationSignInManager>();
            builder.RegisterType<ApplicationRoleManager>();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication);
        }
    }
}