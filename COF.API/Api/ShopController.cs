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
    [Authorize]
    [RoutePrefix("api/shop")]
    public class ShopController : ApiControllerBase
    {
        public readonly IShopService _shopService;
        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [Route("")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetTaskAsync()
        {
            var result = await _shopService.GetAllShopAsync();
            return SuccessResult(result);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetByIdAsync([FromUri] int id)
        {
            var result = await _shopService.GetByIdAsync(id);
            return SuccessResult(result);
        }
    }
}