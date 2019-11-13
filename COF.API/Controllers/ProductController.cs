using COF.API.Controllers.Core;
using COF.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using COF.API.Models.Product;
using ServiceModels = COF.BusinessLogic.Models;
using COF.BusinessLogic.Models.Product;
using COF.DataAccess.EF.Models;

namespace COF.API.Controllers
{
    [Authorize]
    [Authorize(Roles = "ShopManager,PartnerAdmin")]
    public class ProductController : MvcControllerBase
    {
        #region fields 
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IShopService _shopService;
        private readonly ISizeService _sizeService;
        private readonly IRawMateterialService _rawMateterialService;
        #endregion


        #region ctor
        public ProductController(
            IProductService productService, 
            IUserService userService,
            IShopService shopService,
            ISizeService sizeService,
            IRawMateterialService rawMateterialService)
        {
            _productService = productService;
            _userService = userService;
            _shopService = shopService;
            _sizeService = sizeService;
            _rawMateterialService = rawMateterialService;
        }

        #endregion

        [Route("san-pham")]
        public async Task<ActionResult> Index()
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
            var isAdmin =  UserManager.IsInRole(user.Id, "PartnerAdmin");
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
        public async Task<ActionResult> GetAllProducts(string keyword= "",int shopId = 0)
        {
            var result = await _productService.GetAllProductsAsync(keyword,shopId);
           
            return HttpGetSuccessResponse(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetCategories(int shopId)
        {
            var result = await _productService.GetAllCategoriesAsync(shopId);

            return HttpGetSuccessResponse(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetProductDetail(int productId)
        {
            var result = await _productService.GetByIdAsync(productId);
            return HttpGetSuccessResponse(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(COF.API.Models.Product.ProductCreateModel model)
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

                var productModel = new ServiceModels.Product.ProductCreateModel
                {
                    Name = model.Name,
                    CategoryId = model.CategoryId,
                    Description = model.Description,
                    ShopId = model.ShopId,
                    PartnerId = user.PartnerId.GetValueOrDefault(),
                    Image = model.Image,
                    Rms = model.Rms
                };

                var logicResult = await _productService.AddProductAsync(productModel);
                if (!logicResult.Success)
                {
                    return HttpPostErrorResponse(logicResult.Validations.Errors.First().ErrorMessage);
                }

                var result = new ServiceModels.Product.ProductModel
                {
                    Id = logicResult.Result.Id,
                    Description = logicResult.Result.Description,
                    Name = logicResult.Result.ProductName,
                    IsActive = false
                };
                return HttpPostSuccessResponse(result, "Tạo mới thành công");
            }
            catch (Exception ex)
            {
                return HttpPostErrorResponse($"Xảy ra lỗi : " + ex.Message);
            }
          
        }

        [HttpPost]
        public async Task<ActionResult> AddProductSize(COF.API.Models.Product.ProductSizeCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return HttpPostErrorResponse(ModelStateErrorMessage());
            }
            try
            {
                var productSizeModel = new ServiceModels.Product.ProductSizeRequestModel
                {
                   Price = model.Price, 
                   ProductId = model.ProductId,
                   SizeId = model.SizeId
                };

                var logicResult = await _productService.AddProductSizeAsync(productSizeModel);
                if (!logicResult.Success)
                {
                    return HttpPostErrorResponse(logicResult.Validations.Errors.First().ErrorMessage);
                }

            
                return HttpPostSuccessResponse(message: "Tạo mới thành công");
            }
            catch (Exception ex)
            {
                return HttpPostErrorResponse($"Xảy ra lỗi : " + ex.Message);
            }

        }

        public async Task<ActionResult> UpdateProduct(COF.API.Models.Product.ProductCreateModel model)
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

                var productModel = new ServiceModels.Product.ProductCreateModel
                {
                    Name = model.Name,
                    CategoryId = model.CategoryId,
                    Description = model.Description,
                    ShopId = model.ShopId,
                    IsActive = model.IsActive,
                    PartnerId = user.PartnerId.GetValueOrDefault(),
                    Image = model.Image,
                    Rms = model.Rms
                };

                var logicResult = await _productService.UpdatProductAsync(model.Id.GetValueOrDefault(), productModel);
                if (!logicResult.Success)
                {
                    return HttpPostErrorResponse(logicResult.Validations.Errors.First().ErrorMessage);
                }
                return HttpPostSuccessResponse(null, "Cập nhập  thành công");
            }
            catch (Exception ex)
            {
                return HttpPostErrorResponse($"Xảy ra lỗi : " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateProductSize(ProductSizeUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return HttpPostErrorResponse(ModelStateErrorMessage());
            }
            try
            {
                var productSizeModel = new ServiceModels.Product.ProductSizeRequestModel
                {
                    Price = model.Price,
                    ProductId = model.ProductId,
                    SizeId = model.SizeId
                };

                var logicResult = await _productService.UpdateProductSizeAsync(model.Id , productSizeModel);
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

        [HttpPost]
        public async Task<ActionResult> RemoveProductSize(int productId,int id)
        {
            try
            { 
                var logicResult = await _productService.RemoveProductSize(id);
                if (!logicResult.Success)
                {
                    return HttpPostErrorResponse(logicResult.Validations.Errors.First().ErrorMessage);
                }

                return HttpPostSuccessResponse(null, "Xóa thành công");
            }
            catch (Exception ex) 
            {
                return HttpPostErrorResponse($"Xảy ra lỗi : " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetProductFormularTable(int productId)
        {
            try
            {
                var allRmsQuery = await _productService.GetRms(productId);
                var productRmsQuery = await _productService.GetFormularByProductId(productId);

                var allRms = allRmsQuery.Result;
                var productRms = productRmsQuery.Result;
                var model = new ProductSizeFormularModel
                {
                    Rms = allRms,
                    Details = productRms
                };

                var data = RenderPartialViewToString("_PartialUpdateProductRm", model);
                return HttpGetSuccessResponse(data);
            }
            catch (Exception ex)
            {
                return HttpGetErrorResponse(ex.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult> UpdateProductFormularTable(List<ProductSizeRawMaterialUpdateFormularModel> model)
        {
            try
            {
                var productSizeRms = model.Select(x => new ProductSizeRawMaterial
                {
                    ProductSizeId = x.ProductSizeId,
                    RawMaterialId = x.RawMaterialId,
                    Amount = x.Amount
                }).ToList();

                var res = await _productService.UpdateFormularByProductId(productSizeRms);

                if (res.Validations != null)
                {
                    return HttpPostErrorResponse(res.Validations.Errors[0].ErrorMessage);
                }
                return HttpPostSuccessResponse();
            }
            catch (Exception ex)
            {
                return HttpPostErrorResponse(ex.Message);
            }
        }
    }
}