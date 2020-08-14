using Autofac;
using Autofac.Integration.Mvc;
using COF.API.Api.Core;
using COF.BusinessLogic.Settings;
using COF.DataAccess.EF;
using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Repositories;

namespace COF.API.AutofacModules
{
    public class EfModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EFTransaction>().As<ITransaction>();
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<EFContext>();
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterType<WorkContext>().As<IWorkContext>();

            
            builder.RegisterAssemblyTypes(typeof(ProductRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
        }
    }
}