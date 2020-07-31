using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using COF.API.Api.Core;
using COF.BusinessLogic.Services;
using COF.BusinessLogic.Settings;
using COF.DataAccess.EF;
using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using COF.DataAccess.EF.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Hangfire;
namespace COF.API.Ioc
{
    public class AutofacWebapiConfig
    {
       

        public static ContainerBuilder Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<RoleStore<AppRole>>().As<IRoleStore<AppRole, string>>().InstancePerLifetimeScope();
            //Asp.net Identity
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<AppUser>>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerLifetimeScope();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerLifetimeScope();
            builder.RegisterType<EFTransaction>().As<ITransaction>().InstancePerLifetimeScope();
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<EFContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerLifetimeScope();


            // Repositories
            builder.RegisterAssemblyTypes(typeof(ProductRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().SingleInstance().InstancePerLifetimeScope();

            // Services

            builder.RegisterAssemblyTypes(typeof(ShopService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().SingleInstance().InstancePerLifetimeScope();


            return builder;

        }


    }

}