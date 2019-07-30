using COF.API.Controllers.Core;
using COF.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace COF.API.Controllers
{
    [Authorize]
    public class ProductController : MvcControllerBase
    {
        #region fields 
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IShopService _shopService;
        #endregion


        #region ctor
        public ProductController(
            IProductService productService, 
            IUserService userService,
            IShopService shopService)
        {
            _productService = productService;
            _userService = userService;
            _shopService = shopService;
        }

        #endregion

        [Route("san-pham")]
        public async Task<ActionResult> Index()
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
            var result = await _productService.GetAllProductsAsync(7);
            var shops = await _shopService.GetAllShopAsync(user.PartnerId.GetValueOrDefault());
            TempData["Shops"] = shops;
            return View(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllProducts(int shopId)
        {
            var result = await _productService.GetAllProductsAsync(shopId);
           
            return HttpGetSuccessResponse(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetCategories(int shopId)
        {
            var result = await _productService.GetAllCategoriesAsync(shopId);

            return HttpGetSuccessResponse(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetProductDetail(int productId)
        {
            var result = await _productService.GetByIdAsync(productId);
            return HttpGetSuccessResponse(result);
        }

    }
}