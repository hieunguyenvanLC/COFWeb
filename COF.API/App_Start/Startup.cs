using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using COF.API.Filter.Hangfire;
using COF.BusinessLogic.Services.Hangfire;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System.Web.Http;
using System.Web.Mvc;
using GlobalConfiguration = Hangfire.GlobalConfiguration;

[assembly: OwinStartup(typeof(COF.API.App_Start.Startup))]

namespace COF.API.App_Start
{
    public partial class Startup
    {
        public static IContainer container { get; set; }

        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();

            var containerBuilder = Bootstrapper.Bootstrap();

            container = containerBuilder.Build();

            ConfigureAuth(app);

            GlobalConfiguration.Configuration.UseAutofacActivator(container);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);

            httpConfig.SuppressDefaultHostAuthentication();
            httpConfig.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            string schemaName = "cof_hf";
            string connectionString = "HangFireConnection";

            UseSqlServer(app, connectionString, schemaName);

            UseDashboard(app, connectionString, schemaName);

            StartHangfireJob();
        }

        private static void UseSqlServer(IAppBuilder app, string connectionString, string schemaName)
        {
            var storageOptions = new SqlServerStorageOptions
            {
                SchemaName = schemaName
            };
            
            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString, storageOptions);

            JobStorage.Current = new SqlServerStorage(connectionString, storageOptions);
            app.UseHangfireServer();
        }

        private void UseDashboard(IAppBuilder app, string connectionString, string schemaName)
        {
            var dashboardOptions = CreateDashboardOptions();
            var storage = CreateStorage(connectionString, schemaName);
            app.UseHangfireDashboard("/hangfire", dashboardOptions, storage);
        }

        private static DashboardOptions CreateDashboardOptions()
        {
            DashboardOptions dashboardOptions = new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthFilter() }
            };
            return dashboardOptions;
        }

        private SqlServerStorage CreateStorage(string connectionString, string schemaName)
        {
            var storageOptions = new SqlServerStorageOptions
            {
                SchemaName = schemaName
            };
            return new SqlServerStorage(connectionString, storageOptions);
        }

        private void StartHangfireJob()
        {
            if (DependencyResolver.Current.GetService(typeof(IHangfireService)) is IHangfireService hangfireService)
            {
                hangfireService.Start();
            }
        }
    }
}
