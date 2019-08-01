using COF.BusinessLogic.Models.Product;
using COF.BusinessLogic.Settings;
using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using COF.DataAccess.EF.Repositories;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface IProductService
    {
        Task<List<ProductByCategoryModel>> GetAllProductsAsync(string keyword,int shopId);
        Task<List<ProductByShop>> GetAllProductsByPartnerIdAsync(int partnerId);
        Task<List<ProductByCategoryModel>> GetAllCategoriesAsync(int shopId);
        Task<ProductModel> GetByIdAsync(int id);
        Task<BusinessLogicResult<Product>> AddProductAsync(ProductCreateModel model);
    }
    public class ProductService : IProductService
    {
        #region fields

        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IShopRepository _shopRepository;
        #endregion

        #region ctor
        public ProductService
        (
            IUnitOfWork unitOfWork,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IShopRepository shopRepository
        )
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _shopRepository = shopRepository;

        }
        #endregion

        #region public methods
        public async Task<List<ProductByCategoryModel>> GetAllProductsAsync(string keyword ,int shopId)
        {
            var products = await _productRepository.GetAllProductAsync(shopId, keyword);
            var categories =  products.Select(x => x.Category).Distinct().OrderBy(x => x.SeqNo).ToList();
            var result = categories.Select(x => new ProductByCategoryModel
            {
                CategoryId = x.Id,
                Name = x.Name,
                Products = products.Where(p => p.CategoryId == x.Id).Select(y => new ProductModel
                {
                    Id = y.Id,
                    Name = y.ProductName,
                    Sizes = y.ProductSizes.Select(z => new Models.Product.ProductSize
                    {
                        SizeId = z.Id,
                        Cost = z.Cost,
                        Size = z.Size.Name               
                    }).ToList(),
                    IsActive = y.IsActive
                }).ToList(),
                
            }).ToList();
            return result;

        }

        public async Task<List<ProductByShop>> GetAllProductsByPartnerIdAsync(int partnerId)
        {
            var listShops = await _shopRepository.GetAllShopByPartnerIdAsync(partnerId);
            var result = new List<ProductByShop>();
            if (listShops.Any())
            {
                foreach (var shop in listShops)
                {
                    var item = new ProductByShop
                    {
                        ShopId = shop.Id,
                        Name = shop.ShopName
                    };
                    item.Categories = await GetAllProductsAsync(string.Empty,shop.Id);
                    result.Add(item);
                }
            }
            return result;
        }

        public async Task<List<ProductByCategoryModel>> GetAllCategoriesAsync(int shopId)
        {
            var categories = await _categoryRepository.GetByShopId(shopId);
            var result = categories.Select(x => new ProductByCategoryModel
            {
                CategoryId = x.Id,
                Name = x.Name
            }).ToList();
            return result;
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            ProductModel result = null;
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                result = new ProductModel
                {
                    Id = product.Id,
                    Name = product.ProductName,
                    Description = product.Description,
                    Sizes = product.ProductSizes.Select(z => new Models.Product.ProductSize
                    {
                        SizeId = z.Id,
                        Cost = z.Cost,
                        Size = z.Size.Name
                    }).ToList()
                };
            }
            return result;
        }

        public async Task<BusinessLogicResult<Product>> AddProductAsync(ProductCreateModel model)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(model.CategoryId);
                if (category is null)
                {
                    return new BusinessLogicResult<Product>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Danh mục", "Danh mục không tồn tại.") })
                    };
                }

                var duplicatedProduct = await _productRepository.GetSingleAsync((x) => x.ShopId == model.ShopId && x.ProductName == model.Name);
                if (duplicatedProduct != null)
                {
                    return new BusinessLogicResult<Product>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Tên sản phẩm", "Tên sản phẩm đã tồn tại.") })
                    };
                }

                var product = new Product
                {
                    CategoryId = category.Id,
                    ProductName = model.Name,
                    Description = model.Description,
                    ShopId = model.ShopId,
                    PartnerId = model.PartnerId
                };

                _productRepository.Add(product);
                 await _unitOfWork.SaveChangesAsync();
                return new BusinessLogicResult<Product>
                {
                    Success = true,
                    Result = product
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<Product>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra : ", ex.Message) })
                };
            }
            


        }

        #endregion
    }
}
