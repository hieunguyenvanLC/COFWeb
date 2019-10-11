﻿using COF.API.Controllers.Core;
using COF.API.Models.Dashboard;
using COF.BusinessLogic.Models.Report;
using COF.BusinessLogic.Services;
using COF.BusinessLogic.Services.Reports;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace COF.API.Controllers
{
    
    [Authorize(Roles = "Partner,PartnerAdmin")]
    public class DashboardController : MvcControllerBase
    {
        #region fields
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        private readonly IShopService _shopService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        #endregion

        #region ctor
        public DashboardController(
            IReportService reportService,
            IUserService userService,
            IShopService shopService,
            ICustomerService customerService,
            IOrderService orderService)
        {
            _reportService = reportService;
            _userService = userService;
            _shopService = shopService;
            _customerService = customerService;
            _orderService = orderService;
        }
        #endregion


        [Route("bang-dieu-khien")]
        public async Task<ActionResult> Index()
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
            var partnerId = user.PartnerId.GetValueOrDefault();
            var shops = await _shopService.GetAllShopAsync(user.PartnerId.GetValueOrDefault());
            TempData["Shops"] = shops;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetShopRevenue()
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
            var data = _reportService.GetPartnerRevenueMonthlyReport(user.PartnerId.GetValueOrDefault());
            return HttpPostSuccessResponse(data);
        }

        [HttpPost]
        public async Task<JsonResult> FilterRevenuneByPartner(FilterRevenueModel model)
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
            var result = new List<ShopRevenueReportModel>();
            switch (model.Type)
            {
                case FilterType.InMonth:
                    result = _reportService.GetShopRevenueReportImMonthModels(
                        user.PartnerId.GetValueOrDefault(), 
                        model.ShopId);
                    break;
                case FilterType.InYear:
                    result = _reportService.GetShopRevenueReportInYearModels(
                        user.PartnerId.GetValueOrDefault(), 
                        model.ShopId);
                    break;
                case FilterType.Customize:
                 {
                        if (model._fromDate is null || model._toDate is null)
                        {
                            return HttpPostErrorResponse("Phải nhập ngày bắt đầu và ngày kết thúc.");
                        }

                        if (model._fromDate > model._toDate)
                        {
                            return HttpPostErrorResponse("Ngày kết thúc phải trễ hơn ngày bắt đầu.");
                        }
                        if ((model._toDate.Value - model._fromDate.Value).Days > 30)
                        {
                            return HttpPostErrorResponse("Chỉ cho phép tìm kiếm trong 30 ngày.");
                        }

                        result = _reportService.GetShopRevenueReportInRange(
                        user.PartnerId.GetValueOrDefault(),
                        model.ShopId,
                        model._fromDate.GetValueOrDefault(),
                        model._toDate.GetValueOrDefault());
                        break;
                 }
                    
            }
            return HttpPostSuccessResponse(result);
        }


        [HttpPost]
        public async Task<JsonResult> GetGeneralInfo()
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
            var partnerId = user.PartnerId.GetValueOrDefault();
            var totalOrder = _orderService.GetTotalOrder(partnerId);
            var totalCustomer = _customerService.GetTotalCustomer(partnerId);
            return HttpPostSuccessResponse(new
            {
                TotalOrder = totalOrder,
                TotalCustomer = totalCustomer
            });
        }
    }
}