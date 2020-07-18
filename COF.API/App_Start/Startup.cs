using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Cors;
using System.Linq;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using COF.API.Ioc;
using Autofac;
using Autofac.Integration.WebApi;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using Hangfire;
using Hangfire.SqlServer;
using COF.API.Filter.Hangfire;
using COF.BusinessLogic.Services.Hangfire;

[assembly: OwinStartup(typeof(COF.API.App_Start.Startup))]

namespace COF.API.App_Start
{
    public partial class Startup
    {
        public static IContainer container { get; set; }
        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();
            var builder = AutofacWebapiConfig.Configuration(app);

            container = builder.Build();
            //var resolver = new AutofacWebApiDependencyResolver(container);
            //httpConfig.DependencyResolver = resolver;
            ConfigureAuth(app);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container); 

            httpConfig.SuppressDefaultHostAuthentication();
            httpConfig.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            string schemaName = "assessment_hf";
            string connectionString = "HangFireConnection";
            UseSqlServer(app, connectionString, schemaName);
            UseDashboard(app, connectionString, schemaName);
        }
        private static void UseSqlServer(IAppBuilder app, string connectionString, string schemaName)
        {
            var storageOptions = new SqlServerStorageOptions
            {
                SchemaName = schemaName
            };
            Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString, storageOptions);
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
    }
}
