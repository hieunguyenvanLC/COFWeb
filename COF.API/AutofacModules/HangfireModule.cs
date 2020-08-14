using Autofac;
using Hangfire;

namespace COF.API.AutofacModules
{
    public class HangfireModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BackgroundJobClient>().As<IBackgroundJobClient>();
        }
    }
}