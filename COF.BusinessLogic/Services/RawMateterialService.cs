using COF.BusinessLogic.Models.RawMaterial;
using COF.BusinessLogic.Settings;
using COF.Common.Helper;
using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using COF.DataAccess.EF.Repositories;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        Task<BusinessLogicResult<RawMaterial>> GetByIdAsync(int id);
        Task<BusinessLogicResult<bool>> UpdateRmQty(int parnterId, int id, decimal qty, string updateBy, string note, InputType inputType);
        Task<BusinessLogicResult<List<RawMaterialModel>>> GetAllAsync(int shopId);
        Task<BusinessLogicResult<List<TodayRawMaterialReport>>> GetTodayReport(RmReportSearchModel model);
        Task<BusinessLogicResult<List<RawMaterialHistoryDetailModel>>> GetHistoriesWithPaging(int id, int pageIndex, int pageSize, DateTime? fromDate, DateTime? toDate, int? inputTypeId);
        Task<BusinessLogicResult<bool>> UpdateAsync(RawMaterialRequestModel model);
    }
    public class RawMateterialService : IRawMateterialService
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRawMaterialRepository _rawMaterialRepository;
        private readonly IRawMaterialUnitRepository _rawMaterialUnitRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IRawMaterialHistoryRepository _rawMaterialHistoryRepository;
        #endregion

        #region ctor
        public RawMateterialService(
            IUnitOfWork unitOfWork,
            IRawMaterialRepository rawMaterialRepository,
            IRawMaterialUnitRepository rawMaterialUnitRepository,
            IShopRepository shopRepository,
            IRawMaterialHistoryRepository rawMaterialHistoryRepository
            )
        {
            _unitOfWork = unitOfWork;
            _rawMaterialRepository = rawMaterialRepository;
            _rawMaterialUnitRepository = rawMaterialUnitRepository;
            _rawMaterialHistoryRepository = rawMaterialHistoryRepository;
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

        public async Task<BusinessLogicResult<RawMaterial>> GetByIdAsync(int id)
        {
            try
            {
                var res = await _rawMaterialRepository.GetByIdAsync(id);
                return new BusinessLogicResult<RawMaterial>
                {
                    Success = true,
                    Result = res
                };
            }
            catch (Exception ex)
            {

                return new BusinessLogicResult<RawMaterial>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", ex.Message) })
                };
            }
        }

       

        public async Task<BusinessLogicResult<bool>> UpdateRmQty(int parnterId, int id, decimal qty, string updateBy, string note, InputType inputType)
        {
            try
            {
                var rm = await _rawMaterialRepository.GetByIdAsync(id);
                if (rm is null)
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Nguyên liệu không tồn tại.") })
                    };
                }


                if (inputType == InputType.AddNew)
                {
                    if (qty < rm.AutoTotalQty)
                    {
                        return new BusinessLogicResult<bool>
                        {
                            Success = false,
                            Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Amount", "Giá trị nguyên liệu sau khi thêm vào không hợp lệ.") })
                        };
                    }
                }

                var diffrentQty = qty - rm.AutoTotalQty;

                rm.AutoTotalQty = qty;
                rm.UserInputTotalQty = qty;
                _rawMaterialRepository.Update(rm, updateBy);

                #region log history

                

                var rawMaterialHistory = new RawMaterialHistory
                {
                    TimeAccess = DateTimeHelper.CurentVnTime,
                    PartnerId = parnterId,
                    RawMaterialId = rm.Id,
                    Quantity = Math.Abs(diffrentQty),
                    TransactionTypeId = diffrentQty > 0 ? TransactionType.Increasement : TransactionType.Decreasement,
                    TotalQtyAtTimeAccess = qty,
                    CreatedBy = updateBy,
                    InputTypeId = inputType,
                    Description = note
                };

                _rawMaterialHistoryRepository.Add(rawMaterialHistory, updateBy);

                #endregion

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

        public async Task<BusinessLogicResult<List<RawMaterialHistoryDetailModel>>> GetHistoriesWithPaging(int id, int pageIndex, int pageSize, DateTime? fromDate, DateTime? toDate, int? inputTypeId)
        {
            try
            {
                
                var sql = "exec AllRawMaterialHistoriesWithPaging @p0, @p1, @p2, @p3, @p4, @p5";
                var result = await _unitOfWork.Context.Database.SqlQuery<RawMaterialHistoryDetailModel>(
                                sql, 
                                id, 
                                pageIndex, 
                                pageSize, 
                                fromDate, 
                                toDate, 
                                inputTypeId).ToListAsync();

                return new BusinessLogicResult<List<RawMaterialHistoryDetailModel>>
                {
                    Result = result,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<List<RawMaterialHistoryDetailModel>>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", ex.Message) })
                };
            }
        }

        public async Task<BusinessLogicResult<List<RawMaterialModel>>> GetAllAsync(int shopId)
        {
            try
            {
                var rms = await _rawMaterialRepository.GetByFilterAsync(filter: x => x.ShopId == shopId);
                var result = rms.Select(x => new RawMaterialModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();
                return new BusinessLogicResult<List<RawMaterialModel>>
                {
                    Success = true,
                    Result = result
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BusinessLogicResult<List<TodayRawMaterialReport>>> GetTodayReport(RmReportSearchModel model)
        {
            try
            {
                var shop = await _shopRepository.GetByIdAsync(model.ShopId);
                if (shop is null)
                {
                    return new BusinessLogicResult<List<TodayRawMaterialReport>>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("ShopId", "ShopId không tồn tại.") })
                    };
                }

                var rms = await _rawMaterialRepository.GetByFilterAsync(x => x.ShopId == model.ShopId);
                var rmIds = rms.Select(x => x.Id).ToList();
                var allRmsHistories = await _rawMaterialHistoryRepository.GetByFilterAsync(x => 
                (model._toDate ==  null || DbFunctions.TruncateTime(x.CreatedOnUtc) <= DbFunctions.TruncateTime(model._toDate))
                
                && (model._fromDate == null ||  DbFunctions.TruncateTime(x.CreatedOnUtc) >= DbFunctions.TruncateTime(model._fromDate))

                && rmIds.Contains(x.RawMaterialId));
                
                var result = new List<TodayRawMaterialReport>();
                foreach (var rm in rms)
                {
                    var allRmsHistory = allRmsHistories.Where(x => x.RawMaterialId == rm.Id).ToList();
                    var orders = allRmsHistory.Where(x => x.OrderId != null).Select(x => x.OrderId).Count();
                    var tmp = new TodayRawMaterialReport
                    {
                        RmId = rm.Id,
                        RmName = rm.Name,
                        TotalOrder = orders,
                        CurrentAmount = allRmsHistory.OrderByDescending(x => x.Id).FirstOrDefault()?.TotalQtyAtTimeAccess ?? 0,
                        StartDayAmount = allRmsHistory.OrderBy(x => x.Id).FirstOrDefault()?.OldQty ?? 0,
                        OrderUsedAmount = allRmsHistory.Where(x => x.OrderId != null).Select(x => x.Quantity).Sum(),
                        AddNewAmount = allRmsHistory.Where(x => x.InputTypeId == InputType.AddNew).Select(x => x.Quantity).Sum()
                    };
                    tmp.DiffrentAmount = allRmsHistory.Sum(x =>
                    {
                        if (x.InputTypeId != InputType.AddNew)
                        {
                            if (x.TransactionTypeId == TransactionType.Decreasement)
                            {
                                return -1.0m * x.Quantity;
                            }
                            else
                            {
                                return x.Quantity;
                            }
                        }
                        return 0;
                    });
                    result.Add(tmp);
                }

                return new BusinessLogicResult<List<TodayRawMaterialReport>>
                {
                    Result = result,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<List<TodayRawMaterialReport>>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", ex.Message) })
                };
            }
        }

        public async Task<BusinessLogicResult<bool>> UpdateAsync(RawMaterialRequestModel model)
        {
            try
            {
                var rm = await _rawMaterialRepository.GetByIdAsync(model.Id);
                if (rm is null)
                {
                    return new BusinessLogicResult<bool>
                    {
                        Result = false,
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id không tồn tại.") })
                    };
                }

                rm.RawMaterialUnitId = model.RawMaterialUnitId;
                rm.Name = model.Name;
                rm.Description = model.Description;

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
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", ex.Message) })
                };
            }
        }
    }
}
