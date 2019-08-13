using Autofac;
using COF.BusinessLogic.Services;
using COF.DataAccess.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.AzureFunctions.Ioc
{
    class FuncModule : AppModule
    {
        public override void Register(ContainerBuilder builder)
        {
            // Repositories
            builder.RegisterAssemblyTypes(typeof(ProductRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().SingleInstance();

            // Services

            builder.RegisterAssemblyTypes(typeof(ShopService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().SingleInstance();

        }
    }
}
