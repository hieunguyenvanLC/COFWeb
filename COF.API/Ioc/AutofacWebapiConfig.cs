﻿using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using COF.DataAccess.EF;
using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Repositories;
using CustomOAuthTutorial.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace COF.API.Ioc
{
    public class AutofacWebapiConfig
    {
        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }


        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }


        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<EFTransaction>().As<ITransaction>();
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<EFContext>().AsSelf().InstancePerRequest();


            // Repositories
            builder.RegisterAssemblyTypes(typeof(ProductRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Services
            //builder.RegisterAssemblyTypes(typeof(ProductService).Assembly)
            //   .Where(t => t.Name.EndsWith("Service"))
            //   .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterModule(new AutofacWebTypesModule());
            Container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));

            return Container;

        }


    }

}