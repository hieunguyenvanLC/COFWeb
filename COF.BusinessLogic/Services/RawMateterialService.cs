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

namespace COF.BusinessLogic.Services
{
    public interface IRawMateterialService
    {
        Task<BusinessLogicResult<List<RawMaterialModel>>> GetAllWithPaging(int shopId, int pageIndex, int pageSize, string keyword);
        Task<BusinessLogicResult<bool>> CreateAsync(int shopId, RawMaterialRequestModel model);
        Task<BusinessLogicResult<List<RawMaterialUnitModel>>> GetAllRmUnitsAsync();
    }
    public class RawMateterialService : IRawMateterialService
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRawMaterialRepository _rawMaterialRepository;
        private readonly IRawMaterialUnitRepository _rawMaterialUnitRepository;
        private readonly IShopRepository _shopRepository;
        #endregion

        #region ctor
        public RawMateterialService(
            IUnitOfWork unitOfWork,
            IRawMaterialRepository rawMaterialRepository,
            IRawMaterialUnitRepository rawMaterialUnitRepository,
            IShopRepository shopRepository
            )
        {
            _unitOfWork = unitOfWork;
            _rawMaterialRepository = rawMaterialRepository;
            _rawMaterialUnitRepository = rawMaterialUnitRepository;
            _shopRepository = shopRepository;
        }
        #endregion

        public async Task<BusinessLogicResult<List<RawMaterialModel>>> GetAllWithPaging(int shopId, int pageIndex, int pageSize, string keyword)
        {
            try
            {
                var sql = "exec [dbo].[AllRawMaterialByShopWithPaging] @p0, @p1, @p2, @p3";
                var queryRes = await _unitOfWork.Context.Database.SqlQuery<RawMaterialModel>(sql, shopId, pageIndex, pageSize, keyword).ToListAsync();

                return new BusinessLogicResult<List<RawMaterialModel>>
                {
                    Result = queryRes,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<List<RawMaterialModel>>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", ex.Message) })
                };
            }

        }

        public async Task<BusinessLogicResult<bool>> CreateAsync(int shopId, RawMaterialRequestModel model)
        {
            try
            {
                var shop = await _shopRepository.GetByIdAsync(shopId);

                

                var rawMaterialUnit = await _rawMaterialUnitRepository.GetByIdAsync(model.RawMaterialUnitId);
                if (rawMaterialUnit is null)
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Đơn vị", "Đơn vị không tồn tại.") })
                    };
                }
                var rawMaterial = new RawMaterial
                {
                    Name = model.Name,
                    Description = model.Description,
                    AutoTotalQty = 0,
                    UserInputTotalQty = 0,
                    RawMaterialUnitId = rawMaterialUnit.Id,
                    ShopId = shopId,
                    PartnerId = shop.PartnerId
                };

                _rawMaterialRepository.Add(rawMaterial);
                await _unitOfWork.SaveChangesAsync();

                return new BusinessLogicResult<bool>
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", ex.Message) })
                };
            }

        }

        public async Task<BusinessLogicResult<List<RawMaterialUnitModel>>> GetAllRmUnitsAsync()
        {
            try
            {
                var queryRes = await _rawMaterialUnitRepository.GetAllAsync();
                var res = queryRes.Select(x => new RawMaterialUnitModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

                return new BusinessLogicResult<List<RawMaterialUnitModel>>
                {
                    Success = true,
                    Result = res
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<List<RawMaterialUnitModel>>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", ex.Message) })
                };
            }
        }
    }
}
