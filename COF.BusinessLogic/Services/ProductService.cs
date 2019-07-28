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
    }
    public class ProductService : IProductService
    {
        #region fields

        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        #endregion

        #region ctor
        public ProductService
        (
            IUnitOfWork unitOfWork,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository
        )
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
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
        #endregion
    }
}
