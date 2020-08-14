using Autofac;
using Module = Autofac.Module;

namespace COF.API.AutofacModules
{
    public class COFApplicationsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ApplicationModule>();
            builder.RegisterModule<IdentityModule>();
            builder.RegisterModule<EfModule>();
            builder.RegisterModule<ServiceModule>();
        }
    }
}