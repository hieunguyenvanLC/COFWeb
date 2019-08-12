using COF.API.Core;
using COF.API.Models.Order;
using COF.BusinessLogic.Services;
using Microsoft.AspNet.Identity;
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
    [Authorize]
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
        [HttpPost]
        [Route("create")]
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
                    }).ToList(),
                    FinalAmount = model.FinalAmount,
                    OrderCode = model.OrderCode,
                    OrderType = model.OrderType,
                    TotalAmount = model.TotalAmount
                };
                var logicResult = await _orderService.CreateOrderAsync(model.ShopId, orderCreateModel);
                if (logicResult.Validations != null)
                {
                    return ErrorResult(logicResult.Validations.Errors[0].ErrorMessage);
                }
                return SuccessResult();
            }
            catch (Exception ex)
            {
                return ErrorResult(ex.Message);
            }
        }
        #endregion

        #region private methods
        #endregion
    }
}
