using COF.API.Controllers.Core;
using COF.BusinessLogic.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace COF.API.Controllers
{
    public class ShopController : MvcControllerBase
    {
        #region fields 
        private readonly IShopService _shopService;
        private readonly IUserService _userService;
        #endregion


        #region ctor
        public ShopController(
            IShopService shopService,
            IUserService userService)
        {
            _shopService = shopService;
            _userService = userService;
        }

        #endregion
       
        [HttpGet]
        public async Task<JsonResult> GetAllShops()
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
            var result = await _shopService.GetAllShopAsync(user.PartnerId.GetValueOrDefault());
            return HttpGetSuccessResponse(result);
        }


    }
}