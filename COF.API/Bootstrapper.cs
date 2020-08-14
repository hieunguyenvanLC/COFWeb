using Autofac;
using COF.API.AutofacModules;

namespace COF.API
{
    public static class Bootstrapper
    {
        public static ContainerBuilder Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new COFApplicationsModule());
            builder.RegisterModule<HangfireModule>();

            return builder;
        }
    }
}