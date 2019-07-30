using COF.BusinessLogic.Models.Product;
using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using COF.DataAccess.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface IProductService
    {
        Task<List<ProductByCategoryModel>> GetAllProductsAsync(int shopId);
        Task<List<ProductByShop>> GetAllProductsByPartnerIdAsync(int partnerId);
        Task<List<ProductByCategoryModel>> GetAllCategoriesAsync(int shopId);
        Task<ProductModel> GetByIdAsync(int id);
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
        public async Task<List<ProductByCategoryModel>> GetAllProductsAsync(int shopId)
        {
            var categories = await _categoryRepository.GetByShopId(shopId);
            var result = categories.Select(x => new ProductByCategoryModel
            {
                CategoryId = x.Id,
                Name = x.Name,
                Products = x.Products.Select(y => new ProductModel
                {
                    Name = y.ProductName,
                    Sizes = y.ProductSizes.Select(z => new Models.Product.ProductSize
                    {
                        SizeId = z.Id,
                        Cost = z.Cost,
                        Size = z.Size.Name
                    }).ToList()
                }).ToList()
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
                    item.Categories = await GetAllProductsAsync(shop.Id);
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

        #endregion
    }
}
