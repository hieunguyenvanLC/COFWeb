using COF.API.Controllers.Core;
using COF.API.Core;
using COF.API.Models.Shared;
using COF.BusinessLogic.Models.Order;
using COF.BusinessLogic.Services;
using COF.BusinessLogic.Settings;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ServiceModels = COF.BusinessLogic.Models;
namespace COF.API.Controllers
{
    [Authorize(Roles = "ShopManager,PartnerAdmin")]
    public class OrderController : MvcControllerBase
    {
        // GET: Order
        #region fields
        private readonly IWorkContext _workContext;
        private readonly IShopService _shopService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        #endregion

        #region ctor

        public OrderController(
            IWorkContext workContext,
            IShopService shopService,
            IOrderService orderService,
            IUserService userService)
        {
            _workContext = workContext;
            _shopService = shopService;
            _orderService = orderService;
            _userService = userService;
        }
        #endregion

        [Route("hoa-don")]
        public async Task<ActionResult> Index()
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
            var shops = await _shopService.GetAllShopAsync(user.PartnerId.GetValueOrDefault());

            var isAdmin = UserManager.IsInRole(user.Id, "PartnerAdmin");
            if (!isAdmin)
            {
                var shopIds = user.ShopHasUsers.Select(x => x.ShopId).ToList();
                shops = shops.Where(x => shopIds.Contains(x.Id)).ToList();
            }
            TempData["Shops"] = shops;
            return View();
        }

        public async Task<ActionResult> GetAllOrderWithPaging(
            int shopId,
            int pageIndex = 1,
            int pageSize = 10,
            string keyword = "",
            string filter = "")
        {
            try
            {
                var queryRes = await _orderService.GetAllOrderWithPaging(shopId, pageIndex, pageSize, keyword);
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

        #region private methods 
        
        #endregion
    }
}