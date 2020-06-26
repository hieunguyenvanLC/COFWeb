using COF.API.Core;
using COF.BusinessLogic.Services;
using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace COF.API.Api
{
    [Authorize]
    [RoutePrefix("api/shop")]
    public class ShopController : ApiControllerBase
    {
        private readonly IShopService _shopService;
        private readonly IUserService _userService;
        public ShopController(
            IShopService shopService,
            IUserService userService)
        {
            _shopService = shopService;
            _userService = userService;
        }

        [Route("all")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllShops()
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
            var result = await _shopService.GetAllShopAsync(user.PartnerId.GetValueOrDefault());
            if (result is null) return ErrorResult("Kết quả rỗng.");
            return SuccessResult(result);
        }
    }
}