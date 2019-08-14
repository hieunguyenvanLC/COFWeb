using COF.API.Controllers.Core;
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
    [Authorize]
    public class DashboardController : MvcControllerBase
    {
        #region fields
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        #endregion

        #region ctor
        public DashboardController(
            IReportService reportService,
            IUserService userService)
        {
            _reportService = reportService;
            _userService = userService;
        }
        #endregion


        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetShopRevenue()
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
            var data = _reportService.GetPartnerRevenueMonthlyReport(user.PartnerId.GetValueOrDefault());
            return HttpPostSuccessResponse(data);
        }
    }
}