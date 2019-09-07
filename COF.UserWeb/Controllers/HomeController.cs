using COF.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace COF.UserWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        public HomeController(IProductService productService)
        {
            _productService = productService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description pagm,e.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

   
        public ActionResult AllProducts()
        {
            var allProducts =   _productService.GetAllProducts("", 7);
            allProducts.ForEach(x => {
                x.Products = x.Products.Where(y => y.IsActive).ToList();
            });
            return View("_MenuPartial", allProducts);
        }
    }
}