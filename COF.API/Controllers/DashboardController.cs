using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COF.API.Controllers
{
    [RoutePrefix("bang-dieu-khien")]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}