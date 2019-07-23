using COF.BusinessLogic.Settings;
using CapstoneProjectServer.DataAccess.EF.Infrastructure;
using CapstoneProjectServer.DataAccess.EF.Models;
using CapstoneProjectServer.DataAccess.EF.Repositories;
using CapstoneProjectServer.Models.dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface IPromotionService : ILogic
    {
        Task<IEnumerable<Promotion>> GetAllPromotionAsync(PromotionSearchDto dto);
        Task<BusinessLogicResult<bool>> AddPromotionAsync(PromotionCreateDto promotionCreateDto);
        Task<PromotionCreateDto> GetPromotionByIdAsync(int Id);
        Task<BusinessLogicResult<bool>> UpdatePromotionAsync(PromotionCreateDto promotionCreateDto);
        Task<BusinessLogicResult<bool>> DeletePromotionByPromotionIdAsync(int promotionId);
    }
    public class PromotionService : BaseService, IPromotionService
    {
        public PromotionService(IRepositoryHelper repositoryHelper)
        {
            this.RepositoryHelper = repositoryHelper;
            this.UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepositoryHelper RepositoryHelper;
        public async Task<IEnumerable<Promotion>> GetAllPromotionAsync(PromotionSearchDto dto)
        {
            var promotionRepo = this.RepositoryHelper.GetRepository<IPromotionRepository>(UnitOfWork);
            var result = await promotionRepo.GetPromotionsBySearchKey(dto);
            return result;
        }

        public async Task<BusinessLogicResult<bool>> AddPromotionAsync(PromotionCreateDto promotionCreateDto)
        {
            var unitOfWork = this.RepositoryHelper.GetUnitOfWork();
            var promotionRepo = this.RepositoryHelper.GetRepository<IPromotionRepository>(unitOfWork);
            var promtionDetailRepo = this.RepositoryHelper.GetRepository<IPromotionDetailRepository>(unitOfWork);


            using (var transaction = unitOfWork.BeginTransaction())
            {

                try
                {
                    var promtion = new Promotion()
                    {
                        Title = promotionCreateDto.PromotionTile,
                        SupplierId = promotionCreateDto.SupplierId,
                        EffectiveStartDate = DateTime.Parse(promotionCreateDto.EffectiveStartDate, new CultureInfo("en-US", true)),
                        EffectiveEndDate = DateTime.Parse(promotionCreateDto.EffectiveEndDate, new CultureInfo("en-US", true)),
                        Description = promotionCreateDto.Description,
                        IsDeleted = false
                    };
                    promotionRepo.Create(promtion, AccountId.ToString());
                    var dbValresults = await unitOfWork.SaveChangesAsync();
                    if (dbValresults.Any())
                    {
                        transaction.Rollback();
                        return new BusinessLogicResult<bool>
                        {
                            Success = false,
                            Validations = dbValresults.AsFluentValidationResult(),
                        };
                    }
                    var promotionDetails = new List<PromotionDetail>();
                    foreach (var item in promotionCreateDto.PromotionDetailDto)
                    {
                        var detail = new PromotionDetail()
                        {
                            PromotionId = promtion.PromotionId,
                            PromotionPercent = item.PromotionPercent,
                            OriginalPrice = item.OriginalPrice,
                            ServiceId = item.ServiceId
                        };
                        promotionDetails.Add(detail);
                        //promtionDetailRepo.Create(detail, AccountId.ToString());
                    }
                    await promtionDetailRepo.InsertListPromotionDetail(promotionDetails, AccountId.ToString());
                    //dbValresults = await unitOfWork.SaveChangesAsync();
                    //if (dbValresults.Any())
                    //{
                    //    transaction.Rollback();
                    //    return new BusinessLogicResult<bool>
                    //    {
                    //        Success = false,
                    //        Validations = dbValresults.AsFluentValidationResult()
                    //    };
                    //}

                    transaction.Commit();
                    return new BusinessLogicResult<bool>
                    {
                        Success = true,
                        Result = true
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", ex.Message) })
                    };
                }
            }
        }

        public async Task<PromotionCreateDto> GetPromotionByIdAsync(int Id)
        {
            var promotionRepo = this.RepositoryHelper.GetRepository<IPromotionRepository>(UnitOfWork);
            var promotion = await promotionRepo.GetSimpleById(Id);

            var result = new PromotionCreateDto();
            result.PromotionId = promotion.PromotionId;
            result.SupplierId = promotion.SupplierId.Value;
            result.PromotionTile = promotion.Title;
            result.EffectiveEndDate = promotion.EffectiveEndDate.Value.ToString("yyyy-MM-dd");
            result.EffectiveStartDate = promotion.EffectiveStartDate.Value.ToString("yyyy-MM-dd"); ;
            result.Description = promotion.Description;
            result.PromotionDetailDto = new List<PromotionDetailDto>();
            foreach (var item in promotion.PromotionDetails)
            {
                var tmpData = new PromotionDetailDto()
                {
                    PromotionDetailId = item.PromotionDetailId,
                    OriginalPrice = item.OriginalPrice,
                    PromotionPercent = item.PromotionPercent,
                    ServiceId = item.ServiceId,
                    ServiceName = item.Service.Name,
                    PromotionPriceDisplay = String.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:c0}", item.OriginalPrice.Value * (100 - item.PromotionPercent.Value) / 100),
                    OriginalPricePriceDisplay = String.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:c0}", item.OriginalPrice)

                };
                result.PromotionDetailDto.Add(tmpData);
            }

            return result;
        }

        public async Task<BusinessLogicResult<bool>> UpdatePromotionAsync(PromotionCreateDto promotionCreateDto)
        {
            var unitOfWork = this.RepositoryHelper.GetUnitOfWork();
            var promotionRepo = this.RepositoryHelper.GetRepository<IPromotionRepository>(unitOfWork);
            var promtionDetailRepo = this.RepositoryHelper.GetRepository<IPromotionDetailRepository>(unitOfWork);


            using (var transaction = unitOfWork.BeginTransaction())
            {

                try
                {
                    var promotion = await promotionRepo.GetSimpleById(promotionCreateDto.PromotionId);
                    promotion.Title = promotionCreateDto.PromotionTile;
                    promotion.EffectiveStartDate = DateTime.Parse(promotionCreateDto.EffectiveStartDate, new CultureInfo("en-US", true));
                    promotion.EffectiveEndDate = DateTime.Parse(promotionCreateDto.EffectiveEndDate, new CultureInfo("en-US", true));
                    promotion.Description = promotionCreateDto.Description;
                    promotionRepo.Update(promotion, AccountId.ToString());
                    var dbValresults = await unitOfWork.SaveChangesAsync();
                    if (dbValresults.Any())
                    {
                        transaction.Rollback();
                        return new BusinessLogicResult<bool>
                        {
                            Success = false,
                            Validations = dbValresults.AsFluentValidationResult(),
                        };
                    }
                    //foreach (var item in promotion.PromotionDetails)
                    //{
                    //    item.IsDeleted = true;
                    //    //promtionDetailRepo.Update(item, AccountId.ToString());
                    //}
                    await promtionDetailRepo.DeleteListPromotionDetail(promotion.PromotionDetails.ToList());
                    var promotionDetails = new List<PromotionDetail>();
                    foreach (var item in promotionCreateDto.PromotionDetailDto)
                    {
                        var detail = new PromotionDetail()
                        {
                            PromotionId = promotion.PromotionId,
                            PromotionPercent = item.PromotionPercent,
                            OriginalPrice = item.OriginalPrice,
                            ServiceId = item.ServiceId
                        };
                        promotionDetails.Add(detail);
                        //promtionDetailRepo.Create(detail, AccountId.ToString());
                    }
                    await promtionDetailRepo.InsertListPromotionDetail(promotionDetails, AccountId.ToString());
                    //dbValresults = await unitOfWork.SaveChangesAsync();
                    //if (dbValresults.Any())
                    //{
                    //    transaction.Rollback();
                    //    return new BusinessLogicResult<bool>
                    //    {
                    //        Success = false,
                    //        Validations = dbValresults.AsFluentValidationResult()
                    //    };
                    //}

                    transaction.Commit();
                    return new BusinessLogicResult<bool>
                    {
                        Success = true,
                        Result = true
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", ex.Message) })
                    };
                }
            }
        }

        public async Task<BusinessLogicResult<bool>> DeletePromotionByPromotionIdAsync(int promotionId)
        {
            var unitOfWork = this.RepositoryHelper.GetUnitOfWork();
            var promotionRepo = this.RepositoryHelper.GetRepository<IPromotionRepository>(unitOfWork);
            var promtionDetailRepo = this.RepositoryHelper.GetRepository<IPromotionDetailRepository>(unitOfWork);
            try
            {
                var promotion = await promotionRepo.GetSimpleById(promotionId);
                foreach (var item in promotion.PromotionDetails)
                {
                    item.IsDeleted = true;
                    //promtionDetailRepo.Update(item);
                }
                promotion.IsDeleted = true;
                promotionRepo.Update(promotion, AccountId.ToString());
                var dbValresults = await unitOfWork.SaveChangesAsync();
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
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", ex.Message) })
                };
            }
        }
    }
}
