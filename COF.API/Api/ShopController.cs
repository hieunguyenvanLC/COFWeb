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

namespace COF.API.Api
{
    //[Authorize]
    [RoutePrefix("api/shop")]
    public class ShopController : ApiControllerBase
    {
        public readonly IShopService _shopService;
        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [Route("all")]
        public async Task<HttpResponseMessage> GetAllProductByIdAsync()
        {
            var result = await _shopService.GetAllShopAsync(0);
            return SuccessResult(result);
        }
    }
}