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
        #endregion


        #region ctor
        public ProductController(
            IProductService productService, 
            IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());

            var allMenu = await _productService.GetAllProductsByPartnerIdAsync(user.PartnerId.GetValueOrDefault());
            return View(allMenu);
        }

        
    }
}