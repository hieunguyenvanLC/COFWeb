using Autofac;
using COF.BusinessLogic.Services;
using COF.BusinessLogic.Services.Hangfire;
using COF.BusinessLogic.Services.Reports;

namespace COF.API.AutofacModules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterDataServices(builder);

            RegisterBackgroundService(builder);
        }

        private static void RegisterDataServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ShopService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
        }

        private static void RegisterBackgroundService(ContainerBuilder builder)
        {
            builder.RegisterType<TaskService>();
            builder.RegisterType<ReportService>();
        }
    }
}