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
        public async Task<ActionResult> CreateAsync(RawMaterialRequestModel model)
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

        [HttpGet]
        [Route("nguyen-lieu/{id}")]
        public async Task<ActionResult> Detail(int id)
        {
            try
            {
                var queryRes = await _rawMateterialService.GetByIdAsync(id);
                var rm = queryRes.Result;
                var res = new RawMaterialDetailModel
                {
                    Id = rm.Id,
                    Name = rm.Name,
                    Description = rm.Description,
                    RawMaterialUnitId = rm.RawMaterialUnitId,
                    RawMaterialUnitName = rm.RawMaterialUnit.Name,
                    AutoTotalQty = rm.AutoTotalQty,
                    UserInputTotalQty = rm.UserInputTotalQty,
                    ShopId = rm.ShopId,
                    Shop = rm.Shop.ShopName
                };
                return View(res);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateQty(RmUpdateQtyModel model)
        {
            try
            {
                var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
                var logicRes = await _rawMateterialService.UpdateRmQty( user.PartnerId.GetValueOrDefault() , model.Id, model.Qty, user.FullName, model.Note);
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


        [HttpPost]
        public async Task<ActionResult> RmHistoriesWithPaging(RmHistorySearchModel model)
        {
            try
            {
                var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
                var logicRes = await _rawMateterialService.GetHistoriesWithPaging(model.Id,model.PageIndex,model.PageSize,model._fromDate,model._toDate,model.InputTypeId);
                if (!logicRes.Success)
                {
                    return HttpGetErrorResponse(logicRes.Validations.Errors[0].ToString());
                }
                var totalData = logicRes.Result;
                var record = totalData.FirstOrDefault();
                var totalRecord = record.RowCounts;

                totalData.Remove(record);
                var res = new PaginationSet<ServiceModels.RawMaterial.RawMaterialHistoryDetailModel>
                {
                    Items = totalData,
                    PageIndex = model.PageIndex,
                    PageSize = model.PageSize,
                    TotalRows = totalRecord.GetValueOrDefault()
                };
                return HttpGetSuccessResponse(res);
            }
            catch (Exception ex)
            {
                return HttpPostErrorResponse(ex.Message);
            }
        }


        [HttpGet]
        public async Task<ActionResult> GetAllRmsAsync(int shopId)
        {
            try
            {
                var queryRes = await _rawMateterialService.GetAllAsync(shopId);
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

        [HttpGet]
        public async Task<ActionResult> GetTodayReport(int shopId)
        {
            try
            {
                var queryRes = await _rawMateterialService.GetTodayReport(shopId);
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