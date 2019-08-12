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

[assembly: OwinStartup(typeof(COF.API.App_Start.Startup))]

namespace COF.API.App_Start
{
    public partial class Startup
    {
        public static IContainer container { get; set; }
        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();
            //register DI here
            var builder = AutofacWebapiConfig.Configuration(app);

            container = builder.Build();
            // System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            var resolver = new AutofacWebApiDependencyResolver(container);
            httpConfig.DependencyResolver = resolver;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            ConfigureAuth(app);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container); //Set the WebApi DependencyResolver

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            httpConfig.SuppressDefaultHostAuthentication();
            httpConfig.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage("HangFireContext")
               .UseAutofacActivator(container, false);
            var options = new BackgroundJobServerOptions { WorkerCount = Environment.ProcessorCount * 5 };
            app.UseHangfireServer(options);
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthFilter() }
            });

            



        }

    }
}
