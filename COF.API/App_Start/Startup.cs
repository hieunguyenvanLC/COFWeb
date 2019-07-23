using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Cors;
using System.Linq;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;

[assembly: OwinStartup(typeof(COF.API.App_Start.Startup))]

namespace COF.API.App_Start
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions oAuthAuthorizationServerOptions { get; set; }
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
           
        }

    }
}
