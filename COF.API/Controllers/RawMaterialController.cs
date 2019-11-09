using COF.API.Controllers.Core;
using COF.API.Models.Shared;
using COF.BusinessLogic.Models.RawMaterial;
using COF.BusinessLogic.Services;
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
    [Authorize]
    [Authorize(Roles = "PartnerAdmin")]
    public class RawMaterialController : MvcControllerBase
    {
        #region fields
        private readonly IRawMateterialService _rawMateterialService;
        private readonly IUserService _userService;
        private readonly IShopService _shopService;
        #endregion

        #region ctor
        public RawMaterialController(
            IRawMateterialService rawMateterialService,
            IUserService userService,
            IShopService shopService
            )
        {
            _rawMateterialService = rawMateterialService;
            _userService = userService;
            _shopService = shopService;

        }
        #endregion


        // GET: RawMaterial
        [Route("nguyen-lieu")]
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

        [HttpGet]
        public async Task<ActionResult> GetAllAsync(int pageSize = 10, int pageIndex = 1, string keyword = "", int shopId = 0)
        {
            var queryRes = await _rawMateterialService.GetAllWithPaging(shopId,pageIndex,pageSize,keyword);

            if (!queryRes.Success)
            {
                return HttpGetErrorResponse(queryRes.Validations.Errors[0].ToString());
            }
            var totalData = queryRes.Result;
            var record = totalData.FirstOrDefault();
            var totalRecord = record.RowCounts;

            totalData.Remove(record);
            var res = new PaginationSet<ServiceModels.RawMaterial.RawMaterialModel>
            {
                Items = totalData,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRows = totalRecord.GetValueOrDefault()
            };
            return HttpGetSuccessResponse(res);
        }

        [HttpPost]
        public  async Task<ActionResult> CreateAsync(RawMaterialRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return HttpPostErrorResponse(ModelStateErrorMessage());
            }

            try
            {
                var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
                var shops = await _shopService.GetAllShopAsync(user.PartnerId.GetValueOrDefault());

                if (!shops.Any(x => x.Id == model.ShopId))
                {
                    return HttpPostErrorResponse($"Chi nhánh với # {model.ShopId} không thuộc hệ thống");
                }
                var logicRes = await _rawMateterialService.CreateAsync(model.ShopId, model);
                if (logicRes.Validations != null)
                {
                    return HttpPostErrorResponse(logicRes.Validations.Errors[0].ErrorMessage);
                }

                return HttpPostSuccessResponse();

            }
            catch (Exception ex)
            {
                return HttpPostErrorResponse(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRmUnitsAsync()
        {
            try
            {
                var queryRes = await _rawMateterialService.GetAllRmUnitsAsync();
                if (queryRes.Validations != null)
                {
                    return HttpGetSuccessResponse(queryRes.Validations.Errors[0].ErrorMessage);
                }

                return HttpGetSuccessResponse(queryRes.Result);
            }
            catch (Exception ex)
            {

                return HttpPostErrorResponse(ex.Message);
            }
        }
    }
}