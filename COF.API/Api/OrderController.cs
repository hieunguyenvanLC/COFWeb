﻿using COF.API.Core;
using COF.API.Models.Announcement;
using COF.API.Models.Order;
using COF.API.SignalR;
using COF.BusinessLogic.Services;
using COF.BusinessLogic.Settings;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
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
        private readonly IUserService _userService;
        #endregion

        #region ctor
        public OrderController(
            IOrderService orderService,
            IWorkContext workContext,
            IUserService userService)
        {
            _orderService = orderService;
            _workContext = workContext;
            _userService = userService;
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

                if (model.OrderStatus == DataAccess.EF.Models.OrderStatus.CreateOrderMobile)
                {
                    var managers = await _userService.GetAllNotificationUsers(model.StoreId);

                    var message = JsonConvert.DeserializeObject<AnnouncementModel>(JsonConvert.SerializeObject(model));
                    message.OrderId = logicResult.Result.Id;
                    OrderNotificationHub.PushToUsers(managers.Select(x => x.UserId).ToArray(), message, null);
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

        [AllowAnonymous]
        [Route("calculaterms/{id}")]
        public async Task<HttpResponseMessage> CalculateRmsByOrderId([FromUri] int id)
        {
            try
            {
                
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [HttpGet]
        [Route("accept-online-order/{orderCode}")]
        public async  Task<HttpResponseMessage> AcceptOnlineOrder(string orderCode)
        {
            try
            {
                var order = await _orderService.GetByOrderCodeAsync(orderCode);
                if (order.Result is null)
                {
                    return ErrorResult("Order khong ton tai");
                }
                await _orderService.AcceptOnlineOrder(orderCode);
                return SuccessResult();
            }
            catch (Exception ex)
            {

                return ErrorResult(ex.Message);
            }
           
        }

        #region private methods
        #endregion
    }
}
