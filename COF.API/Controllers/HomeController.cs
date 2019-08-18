using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace COF.API.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Partner") || User.IsInRole("PartnerAdmin"))
            {
                return Redirect("/bang-dieu-khien");
            }
            else
            {
                return Redirect("/hoa-don");
            }
           
        }
        
    }
}
