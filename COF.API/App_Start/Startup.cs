using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Cors;
using System.Linq;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using COF.API.Ioc;
using Autofac;
using Autofac.Integration.WebApi;
using System.Web.Http;
using System.Web.Mvc;
using Autofac.Integration.Mvc;

[assembly: OwinStartup(typeof(COF.API.App_Start.Startup))]

namespace COF.API.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();
            ConfigureAuth(app);
            var container = AutofacWebapiConfig.RegisterServices(new ContainerBuilder());

            var resolver = new AutofacWebApiDependencyResolver(container);
            httpConfig.DependencyResolver = resolver;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(httpConfig);
        }

    }
}
