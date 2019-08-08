using COF.DataAccess.EF;
using COF.DataAccess.EF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COF.API.Filter
{
    public class PartnerActionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var partnerContext = (PartnerContext)DependencyResolver.Current.GetService<IPartnerContext>();
            var dataContext = DependencyResolver.Current.GetService<EFContext>();
            var partner = filterContext.HttpContext.Session["PartnerId"];
            if (partner != null)
            {
                partnerContext.PartnerId = 3;
               // dataContext.SetPartnerId(partnerContext.PartnerId);
            }
           
        }
    }
}