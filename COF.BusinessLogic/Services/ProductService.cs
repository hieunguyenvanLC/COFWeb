using COF.BusinessLogic.Models.Product;
using COF.BusinessLogic.Models.RawMaterial;
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
using EFModels = COF.DataAccess.EF.Models;

namespace COF.BusinessLogic.Services
{
    public interface IProductService
    {
        Task<List<ProductByCategoryModel>> GetAllProductsAsync(string keyword,int shopId);
        Task<List<ProductByShop>> GetAllProductsByPartnerIdAsync(int partnerId);
        Task<List<ProductByCategoryModel>> GetAllCategoriesAsync(int shopId);
        Task<ProductModel> GetByIdAsync(int id);
        Task<BusinessLogicResult<Product>> AddProductAsync(ProductCreateModel model);
        Task<BusinessLogicResult<bool>> AddProductSizeAsync(ProductSizeRequestModel model);
        Task<BusinessLogicResult<Product>> UpdatProductAsync(int productId, ProductCreateModel model);
        Task<BusinessLogicResult<bool>> RemoveProductSize(int id);
        Task<BusinessLogicResult<bool>> UpdateProductSizeAsync(int id, ProductSizeRequestModel model);

        List<ProductByCategoryModel> GetAllProducts(string keyword, int shopId);

        Task<BusinessLogicResult<List<ProductFormularForAllSize>>> GetFormularByProductId(int productId);
        Task<BusinessLogicResult<List<RawMaterialModel>>> GetRms(int productId);

        Task<BusinessLogicResult<bool>> UpdateFormularByProductId(List<ProductSizeRawMaterial> model);
    }
    public class ProductService : IProductService
    {
        #region fields

        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IShopRepository _shopRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly IProductHasRawMaterialRepository _productHasRawMaterialRepository;
        private readonly IProductSizeRawMaterialRepository _productSizeRawMaterialRepository;
        #endregion

        #region ctor
        public ProductService
        (
            IUnitOfWork unitOfWork,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IShopRepository shopRepository,
            ISizeRepository sizeRepository,
            IProductSizeRepository productSizeRepository,
            IProductHasRawMaterialRepository productHasRawMaterialRepository,
            IProductSizeRawMaterialRepository productSizeRawMaterialRepository
        )
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _shopRepository = shopRepository;
            _sizeRepository = sizeRepository;
            _productSizeRepository = productSizeRepository;
            _productHasRawMaterialRepository = productHasRawMaterialRepository;
            _productSizeRawMaterialRepository = productSizeRawMaterialRepository;

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
                Image = x.CategoryImage,
                Products = products.Where(p => p.CategoryId == x.Id).Select(y => new ProductModel
                {
                    Id = y.Id,
                    Name = y.ProductName,
                    Description = y.Description,
                    CategoryId = y.CategoryId,
                    Image = y.ProductImage,
                    
                    Sizes = y.ProductSizes.Select(z => new Models.Product.ProductSize
                    {
                        Id = z.Id,
                        SizeId = z.SizeId,
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
                    CategoryId = product.CategoryId,
                    IsActive = product.IsActive,
                    Image = product.ProductImage,
                    Sizes = product.ProductSizes.Select(z => new Models.Product.ProductSize
                    {
                        Id = z.Id,
                        SizeId = z.SizeId,
                        Cost = z.Cost,
                        Size = z.Size.Name
                    }).ToList(),
                    Rms = product.ProductHasRawMaterials.Select(x => new ProductRmUpdateModel
                    {
                        Id = x.RawMaterialId,
                        Name = x.RawMaterial.Name
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
                    PartnerId = model.PartnerId,
                    ProductImage = model.Image,
                    ProductHasRawMaterials = model.Rms.Select(x => new ProductHasRawMaterial
                    {
                        RawMaterialId = x.Id,
                        PartnerId = model.PartnerId
                    }).ToList()
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

        public async Task<BusinessLogicResult<bool>> AddProductSizeAsync(ProductSizeRequestModel model)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(model.ProductId);
                if (product is null)
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Sản phẩm", "Sản phẩm không tồn tại." )})
                    };
                }

                var size = await _sizeRepository.GetByIdAsync(model.SizeId);

                if (size is null)
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Size", "Size không tồn tại." )})
                    };
                }

                if (product.ProductSizes.Any(x  => x.SizeId == size.Id ))
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Size", "Size đã tồn tại.") })
                    };
                }

                var entity = new EFModels.ProductSize
                {
                    ProductId = model.ProductId,
                    SizeId = model.SizeId,
                    Cost = model.Price
                };

                _productSizeRepository.Add(entity);
                await _unitOfWork.SaveChangesAsync();
                return new BusinessLogicResult<bool>
                {
                    Success = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra : ", ex.Message) })
                };
            }
        }

        public async Task<BusinessLogicResult<Product>> UpdatProductAsync(int productId, ProductCreateModel model)
        {
            try
            {
                var product = _productRepository.GetById(productId);

                if (product is null)
                {
                    return new BusinessLogicResult<Product>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Sản phẩm", "Sản phẩm không tồn tại.") })
                    };

                }

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
                if (duplicatedProduct != null && duplicatedProduct.Id != product.Id)
                {
                    return new BusinessLogicResult<Product>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Tên sản phẩm", "Tên sản phẩm đã tồn tại.") })
                    };
                }

                if (model.IsActive && !product.ProductSizes.Any())
                {
                    return new BusinessLogicResult<Product>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Giá", "Bạn phải thêm giá mới có thể Active được sản phẩm.") })
                    };
                }

                product.ProductName = model.Name;
                product.Description = model.Description;
                product.IsActive = model.IsActive;
                product.ProductImage = model.Image;

                _productHasRawMaterialRepository.RemoveMultiple(product.ProductHasRawMaterials.ToList());

                var newRms = model.Rms.Select(x => new ProductHasRawMaterial
                {
                    ProductId = product.Id,
                    PartnerId = product.PartnerId,
                    RawMaterialId = x.Id
                }).ToList();

                _productHasRawMaterialRepository.AddMultiple(newRms);

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

        public async Task<BusinessLogicResult<bool>> RemoveProductSize(int id)
        {
            try
            {
                var productSize = await _productSizeRepository.GetByIdAsync(id);
                if (productSize is null)
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Size", "Size không tồn tại.") })
                    };
                }
                _productSizeRepository.MarkAsRemove(productSize);
                await _unitOfWork.SaveChangesAsync();
                return new BusinessLogicResult<bool>
                {
                    Success = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra : ", ex.Message) })
                };
            }
        }

        public async Task<BusinessLogicResult<bool>> UpdateProductSizeAsync(int id, ProductSizeRequestModel model)
        {
            try
            {
                var productSize = await _productSizeRepository.GetByIdAsync(id);
                if (productSize is null)
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Size", "Size không tồn tại.") })
                    };
                }

                var size = await _sizeRepository.GetByIdAsync(model.SizeId);

                if (size is null)
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Size", "Size không tồn tại.") })
                    };
                }

                if (productSize.SizeId != model.SizeId && productSize.Product.ProductSizes.Any(x => x.SizeId == size.Id))
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Size", "Size đã tồn tại.") })
                    };
                }
                productSize.SizeId = model.SizeId;
                productSize.Cost = model.Price;
                _productSizeRepository.Update(productSize);
                await _unitOfWork.SaveChangesAsync();
                return new BusinessLogicResult<bool>
                {
                    Success = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra : ", ex.Message) })
                };
            }
        }

        public List<ProductByCategoryModel> GetAllProducts(string keyword, int shopId)
        {
            var products =  _productRepository.GetAllProduct(shopId, keyword);

            var categories = products.Select(x => x.Category).Distinct().OrderBy(x => x.SeqNo).ToList();
            var result = categories.Select(x => new ProductByCategoryModel
            {
                CategoryId = x.Id,
                Name = x.Name,
                Image = x.CategoryImage,
                Products = products.Where(p => p.CategoryId == x.Id).Select(y => new ProductModel
                {
                    Id = y.Id,
                    Name = y.ProductName,
                    Description = y.Description,
                    CategoryId = y.CategoryId,
                    Image = y.ProductImage,
                    Sizes = y.ProductSizes.Select(z => new Models.Product.ProductSize
                    {
                        Id = z.Id,
                        SizeId = z.SizeId,
                        Cost = z.Cost,
                        Size = z.Size.Name
                    }).ToList(),
                    IsActive = y.IsActive
                }).ToList(),

            }).ToList();
            return result;
        }

        public async Task<BusinessLogicResult<List<ProductFormularForAllSize>>> GetFormularByProductId(int productId)
        {
            try
            {
                var productSizes = await _productSizeRepository.GetByFilterAsync(x => x.ProductId == productId);
                var result = productSizes.Select(x => new ProductFormularForAllSize
                {
                    ProductSizeId = x.Id,
                    Size = x.Size.Name,
                    SizeId = x.SizeId,
                    Formulars = x.ProductSizeRawMaterials.Select(y => new ProductRmFormularDetailModel
                    {
                        Id = y.Id,
                        RawMaterialId = y.RawMaterialId,
                        Amount = y.Amount
                    }).ToList()
                }).ToList();

                return new BusinessLogicResult<List<ProductFormularForAllSize>>
                {
                    Success = true,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<List<ProductFormularForAllSize>>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra : ", ex.Message) })
                };
            }
        }

        public async Task<BusinessLogicResult<List<RawMaterialModel>>> GetRms(int productId)
        {
            try
            {
                var rms =  await _productHasRawMaterialRepository.GetByFilterAsync(filter: x => x.ProductId == productId);
                var result = rms.Select(x => new RawMaterialModel
                {
                    Id = x.RawMaterialId,
                    Name = x.RawMaterial.Name,
                    RawMaterialUnitName = x.RawMaterial.RawMaterialUnit.Name
                }).ToList();
                return new BusinessLogicResult<List<RawMaterialModel>>
                {
                    Result = result,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<List<RawMaterialModel>>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra : ", ex.Message) })
                };
            }
        }

        public async Task<BusinessLogicResult<bool>> UpdateFormularByProductId( List<ProductSizeRawMaterial> model)
        {
            try
            {
                var productSizeIds = model.Select(x => x.ProductSizeId).Distinct().ToList();
                var productSizeRms = await _productSizeRawMaterialRepository.GetByFilterAsync(x => productSizeIds.Contains(x.ProductSizeId));
                _productSizeRawMaterialRepository.RemoveMultiple(productSizeRms);
                _productSizeRawMaterialRepository.AddMultiple(model);
                await  _unitOfWork.SaveChangesAsync();
                return new BusinessLogicResult<bool>
                {
                    Result = true,
                    Success = true
                };
            }
            catch (Exception ex)
            {

                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra : ", ex.Message) })
                };
            }
        }

        #endregion
    }
}
