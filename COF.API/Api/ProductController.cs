using COF.API.Core;
using COF.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace COF.API.Api
{
    [RoutePrefix("api/product")]
    //[Authorize]
    public class ProductController : ApiControllerBase
    {
        #region fields
        private readonly IProductService _productService;
        #endregion

        #region ctor
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region public methods
        [Route("{shopId}/all")]
        public async Task<HttpResponseMessage> GetAllProductByIdAsync([FromUri] int shopId)
        {
            var result = await _productService.GetAllProductsAsync(string.Empty,shopId);
            return SuccessResult(result);
        }
        #endregion
    }
}