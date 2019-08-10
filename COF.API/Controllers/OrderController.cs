using COF.API.Controllers.Core;
using COF.API.Core;
using COF.API.Models.Shared;
using COF.BusinessLogic.Services;
using COF.BusinessLogic.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ServiceModels = COF.BusinessLogic.Models;
namespace COF.API.Controllers
{
    public class OrderController : MvcControllerBase
    {
        // GET: Order
        #region fields
        private readonly IWorkContext _workContext;
        private readonly IShopService _shopService;
        private readonly IOrderService _orderService;

        #endregion

        #region ctor

        public OrderController(
            IWorkContext workContext,
            IShopService shopService,
            IOrderService orderService)
        {
            _workContext = workContext;
            _shopService = shopService;
            _orderService = orderService;
        }
        #endregion

        [Route("hoa-don")]
        public async Task<ActionResult> Index()
        {
            var shops = await _shopService.GetAllShopAsync(_workContext.CurrentUser.PartnerId.GetValueOrDefault());
            TempData["Shops"] = shops;
            return View();
        }

        public async Task<ActionResult> GetAllOrderWithPaging(
            int shopId,
            int pageIndex = 1,
            int pageSize = 10,
            string filter = "")
        {
            try
            {
                var queryRes = await _orderService.GetAllOrderWithPaging(shopId, pageIndex, pageSize, filter);
                if (!queryRes.Success)
                {
                    return HttpGetErrorResponse(queryRes.Validations.Errors[0].ToString());
                }
                var totalData = queryRes.Result;
                var record = totalData.FirstOrDefault();
                var totalRecord = record.RowCounts;

                totalData.Remove(record);
                var res = new PaginationSet<ServiceModels.Order.OrderModel>
                {
                    Items = totalData,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalRows = totalRecord.GetValueOrDefault()
                };
                return HttpGetSuccessResponse(res);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}