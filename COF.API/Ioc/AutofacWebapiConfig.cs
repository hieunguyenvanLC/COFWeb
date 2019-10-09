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
using COF.API.Hubs;

namespace COF.API.Ioc
{
    public class AutofacWebapiConfig
    {
       

        public static ContainerBuilder Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<RoleStore<AppRole>>().As<IRoleStore<AppRole, string>>().InstancePerRequest();
            //Asp.net Identity
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<AppUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.RegisterType<EFTransaction>().As<ITransaction>().InstancePerRequest();
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<EFContext>().AsSelf().InstancePerRequest();
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerRequest();
            builder.RegisterType<MessageHub>().AsSelf().InstancePerLifetimeScope();


            // Repositories
            builder.RegisterAssemblyTypes(typeof(ProductRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Services

            builder.RegisterAssemblyTypes(typeof(ShopService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();


            return builder;

        }


    }

}