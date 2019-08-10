using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COF.API.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order

        public OrderController()
        {

        }

        public ActionResult Index()
        {
            return View();
        }
    }
}