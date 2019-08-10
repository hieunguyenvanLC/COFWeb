using COF.API.Core;
using COF.API.Models.Order;
using COF.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ServiceModels = COF.BusinessLogic.Models;

namespace COF.API.Api
{
    [RoutePrefix("api/order")]
    public class OrderController : ApiControllerBase
    {
        #region fields
        private readonly IOrderService _orderService;
        #endregion

        #region ctor
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        #endregion

        #region public methods
        public async Task<HttpResponseMessage> CreateOrderAsync([FromBody] OrderCreateModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ErrorResult(ModelStateErrorMessage());
                }
                var orderCreateModel = new ServiceModels.Order.OrderCreateModel
                {
                    CustomerId = model.CustomerId,
                    ShopId = model.ShopId,
                    OrderDetails = model.OrderDetails.Select(x => new ServiceModels.Order.OrderDetailModel
                    {
                        ProductSizeId = x.ProductSizeId,
                        Quantity = x.Quantity
                    }).ToList()
                };
               var logicResult = await _orderService.CreateOrderAsync(model.ShopId, orderCreateModel);
                return SuccessResult(null);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region private methods
        #endregion
    }
}
