using COF.BusinessLogic.Models.Category;
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
    public interface IProductCategoryService
    {
        Task<List<CategoryModel>> GetAllAsync(string keyword, int shopId);
        Task<BusinessLogicResult<bool>> CreateCategoryAsync(int shopId, CategoryRequestModel createModel);
        Task<BusinessLogicResult<bool>> UpdateCategoryAsync(int id, CategoryRequestModel model);
        List<Category> GetAll();

    }
    public class ProductCategoryService : IProductCategoryService
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IShopRepository _shopRepository;
        #endregion

        #region ctor
        public ProductCategoryService
        (
            IUnitOfWork unitOfWork,
            ICategoryRepository categoryRepository,
            IShopRepository shopRepository
        )
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _shopRepository = shopRepository;

        }

       
        #endregion

        #region public methods
        public async Task<List<CategoryModel>> GetAllAsync(string keyword, int shopId)
        {
            var categories = await _categoryRepository.GetByShopId(shopId, keyword);
            var result = categories.Select(x => new CategoryModel { Id =  x.Id, Name = x.Name }).ToList();
            return result;
        }

        public async Task<BusinessLogicResult<bool>> CreateCategoryAsync(int shopId, CategoryRequestModel createModel)
        {
            try
            {
                var shop = await _shopRepository.GetByIdAsync(shopId);
                if (shop is null)
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Chi nhánh", "Chi nhánh không tồn tại.") })
                    };
                }


                if (shop.Categories.Any(x => x.Name == createModel.Name))
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Tên danh mục", "Tên danh mục đã tồn tại.") })
                    };
                }

                var category = new Category
                {
                    Name = createModel.Name,
                    ShopId = shop.Id,
                    PartnerId = shop.PartnerId
                };
                _categoryRepository.Add(category);
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

        public async Task<BusinessLogicResult<bool>> UpdateCategoryAsync(int id, CategoryRequestModel model)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category is null)
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Danh mục", "Danh mục không tồn tại.") })
                    };
                }

                if (category.Name != model.Name && category.Shop.Categories.Any(x => x.Name == model.Name))
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Danh mục", "Tên danh mục đã tồn tại.") })
                    };
                }

                category.Name = model.Name;
                _categoryRepository.Update(category);
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

        public  List<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }




        #endregion
    }
}
