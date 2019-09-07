using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(COF.UserWeb.App_Start.Startup))]

namespace COF.UserWeb.App_Start
{
    public class Startup
    {
       
        public void Configuration(IAppBuilder app)
        {
           
        }
    }
}
