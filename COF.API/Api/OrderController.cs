using COF.API.Core;
using COF.API.Models.Order;
using COF.BusinessLogic.Services;
using COF.BusinessLogic.Settings;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
        private readonly IWorkContext _workContext;
        #endregion

        #region ctor
        public OrderController(
            IOrderService orderService,
            IWorkContext workContext)
        {
            _orderService = orderService;
            _workContext = workContext;
        }
        #endregion

        #region public methods
        [HttpPost]
        [Route("create")]
        public async Task<HttpResponseMessage> CreateOrderAsync([FromBody] ServiceModels.Order.OrderCreateModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ErrorResult(ModelStateErrorMessage());
                }
               
                var logicResult = await _orderService.CreateOrderAsync(model.StoreId, model);
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


        [HttpPost]
        [Route("create-unpublished-order")]
        public async Task<HttpResponseMessage> CreateUnpublishedOrderAsync([FromBody] ServiceModels.Order.OrderCreateModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ErrorResult(ModelStateErrorMessage());
                }

                var order = await _orderService.GetByOrderCodeAsync(model.OrderCode);

                if (order.Result != null)
                {
                    return ErrorResult("OrderCode da ton tai.");
                }

                var logicResult = await _orderService.CreateOrderAsync(model.StoreId, model);
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

        [HttpPost]
        [Route("cancel")]
        public async Task<HttpResponseMessage> CancelOrder(OrderCancelModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ErrorResult(ModelStateErrorMessage());
                }

                var user = await AppUserManager.FindAsync(model.Username, model.Password);

                if (user is null)
                {
                    return ErrorResult("Tên đăng nhập hoặc mật khẩu không đúng.");
                }

                var roles = AppUserManager.GetRoles(user.Id);
                if (!roles.Contains("ShopManager"))
                {
                    return ErrorResult("Bạn không có quyền hủy order");
                }


                var logicResult = await _orderService.CancelOrder(_workContext.CurrentUser.PartnerId.GetValueOrDefault(), model.OrderCode, user.FullName, model.Reason);
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
