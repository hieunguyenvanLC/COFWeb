using COF.API.Controllers.Core;
using COF.API.Models.Category;
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
    public class CategoryController : MvcControllerBase
    {

        #region fields 
        private readonly IProductCategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly IShopService _shopService;
        private readonly ISizeService _sizeService;
        #endregion


        #region ctor
        public CategoryController(
            IProductCategoryService categoryService,
            IUserService userService,
            IShopService shopService,
            ISizeService sizeService)
        {
            _categoryService = categoryService;
            _userService = userService;
            _shopService = shopService;
            _sizeService = sizeService;
        }

        #endregion

        [Route("danh-muc-san-pham")]
        public async Task<ActionResult> Index()
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
            var isAdmin = UserManager.IsInRole(user.Id, "PartnerAdmin");
            var shops = await _shopService.GetAllShopAsync(user.PartnerId.GetValueOrDefault());
            if (!isAdmin)
            {
                var shopIds = user.ShopHasUsers.Select(x => x.ShopId).ToList();
                shops = shops.Where(x => shopIds.Contains(x.Id)).ToList();
            }
            TempData["IsPartnerAdmin"] = isAdmin;
            TempData["Shops"] = shops;
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCategories(string keyword = "", int shopId = 0)
        {
            var result = await _categoryService.GetAllAsync(keyword, shopId);

            return HttpGetSuccessResponse(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddCategory(CategoryCreateModel model)
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

                var createModel = new ServiceModels.Category.CategoryRequestModel
                {
                    Name = model.Name
                };
                var logicResult = await _categoryService.CreateCategoryAsync(model.ShopId, createModel);
                if (!logicResult.Success)
                {
                    return HttpPostErrorResponse(logicResult.Validations.Errors.First().ErrorMessage);
                }

                return HttpPostSuccessResponse(message:"Tạo mới thành công");
            }
            catch (Exception ex)
            {
                return HttpPostErrorResponse($"Xảy ra lỗi : " + ex.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult> UpdateCategory(CategoryUpdateModel model)
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

                var updateModel = new ServiceModels.Category.CategoryRequestModel
                {
                    Name = model.Name
                };
                var logicResult = await _categoryService.UpdateCategoryAsync(model.Id, updateModel);
                if (!logicResult.Success)
                {
                    return HttpPostErrorResponse(logicResult.Validations.Errors.First().ErrorMessage);
                }

                return HttpPostSuccessResponse(message: "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                return HttpPostErrorResponse($"Xảy ra lỗi : " + ex.Message);
            }

        }
    }
}