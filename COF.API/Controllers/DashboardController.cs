using COF.API.Controllers.Core;
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
    [RoutePrefix("bang-dieu-khien")]
    //[Authorize]
    public class DashboardController : MvcControllerBase
    {
        #region fields
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        private readonly IShopService _shopService;
        #endregion

        #region ctor
        public DashboardController(
            IReportService reportService,
            IUserService userService,
            IShopService shopService)
        {
            _reportService = reportService;
            _userService = userService;
            _shopService = shopService;
        }
        #endregion


        [Route("")]
        public async Task<ActionResult> Index()
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId()); 
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
                    result = _reportService.GetShopRevenueReportImMonthModels(user.PartnerId.GetValueOrDefault(), model.ShopId);
                    break;
                case FilterType.InYear:
                    result = _reportService.GetShopRevenueReportInYearModels(user.PartnerId.GetValueOrDefault(), model.ShopId);
                    break;
            }
            return HttpPostSuccessResponse(result);
        }
    }
}