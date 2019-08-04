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
namespace COF.API.Controllers
{
    [Authorize]
    public class ProductController : MvcControllerBase
    {
        #region fields 
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IShopService _shopService;
        private readonly ISizeService _sizeService;
        #endregion


        #region ctor
        public ProductController(
            IProductService productService, 
            IUserService userService,
            IShopService shopService,
            ISizeService sizeService)
        {
            _productService = productService;
            _userService = userService;
            _shopService = shopService;
            _sizeService = sizeService;
        }

        #endregion

        [Route("san-pham")]
        public async Task<ActionResult> Index()
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
            var shops = await _shopService.GetAllShopAsync(user.PartnerId.GetValueOrDefault());
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
        public async Task<ActionResult> AddProduct(ProductCreateModel model)
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
                    PartnerId = user.PartnerId.GetValueOrDefault()
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
        public async Task<ActionResult> AddProductSize(ProductSizeCreateModel model)
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

        public async Task<ActionResult> UpdateProduct(ProductCreateModel model)
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
                    PartnerId = user.PartnerId.GetValueOrDefault()
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
    }
}