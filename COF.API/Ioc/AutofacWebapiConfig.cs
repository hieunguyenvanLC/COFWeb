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
            builder.RegisterType<RoleStore<AppRole>>().As<IRoleStore<AppRole, string>>().InstancePerBackgroundJob().SingleInstance();
            //Asp.net Identity
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<AppUser>>().InstancePerBackgroundJob().SingleInstance();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerBackgroundJob().SingleInstance();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerBackgroundJob().SingleInstance();
            builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerBackgroundJob().SingleInstance();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerBackgroundJob().SingleInstance();
            builder.RegisterType<EFTransaction>().As<ITransaction>().InstancePerBackgroundJob().SingleInstance();
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>().InstancePerBackgroundJob().SingleInstance();
            builder.RegisterType<EFContext>().AsSelf().InstancePerBackgroundJob().SingleInstance();
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerBackgroundJob().SingleInstance();


            // Repositories
            builder.RegisterAssemblyTypes(typeof(ProductRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerBackgroundJob().SingleInstance();

            // Services

            builder.RegisterAssemblyTypes(typeof(ShopService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerBackgroundJob().SingleInstance();


            return builder;

        }


    }

}