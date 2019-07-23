using COF.BusinessLogic.Settings;
using CapstoneProjectServer.DataAccess.EF.Infrastructure;
using CapstoneProjectServer.DataAccess.EF.Models;
using CapstoneProjectServer.DataAccess.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface IServiceService : ILogic
    {
        Task<BusinessLogicResult<bool>> AddServiceAsync(Service service);
        Task<IEnumerable<ServiceType>> GetAlllServiceType();
        Task<IEnumerable<ServiceStatus>> GetAllServiceStatus();
        Task<IEnumerable<Service>> GetAllServiceBySupplierIdAsync(int supplierId, String searchKey);
        Task<BusinessLogicResult<bool>> UpdateServiceAsync(Service serviceDto);
        Task<Service> GetServiceById(int serviceId);
        Task<IEnumerable<Service>> GetAllServices();
        Task<BusinessLogicResult<bool>> DeleteServiceByIdAsync(int id);
        Task<IEnumerable<Service>> GetAllServiceBySupplierId(int supplierId);
        Task<IEnumerable<ServiceType>> GetAllServiceType();
        Task<IEnumerable<Service>> GetAllServiceByServiceTypeIdAsync(int serviceTypeId);
    }
    public class ServiceService : BaseService, IServiceService
    {
        public ServiceService(IRepositoryHelper repositoryHelper)
        {
            this.RepositoryHelper = repositoryHelper;
            this.UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepositoryHelper RepositoryHelper;

        public async Task<BusinessLogicResult<bool>> AddServiceAsync(Service service)
        {

            var repo = RepositoryHelper.GetRepository<IServiceRepository>(UnitOfWork);
            service.IsDeleted = false;
            repo.Create(service, AccountId.ToString());
            var saveResult = await UnitOfWork.SaveChangesAsync();
            if (saveResult.Any())
            {
                return new BusinessLogicResult<bool>()
                {
                    Success = false,
                    Validations = saveResult.AsFluentValidationResult()
                };
            }
            return new BusinessLogicResult<bool>()
            {
                Success = true,
                Result = true
            };
        }

        public async Task<IEnumerable<ServiceType>> GetAlllServiceType()
        {
            var serviceTypeRepository = RepositoryHelper.GetRepository<IServiceTypeRepository>(UnitOfWork);
            var result = (await serviceTypeRepository.GetAllAsync()).ToList();
            var otherType = result.SingleOrDefault(x => x.Name.Contains("Loại khác"));
            if (otherType != null)
            {
                result.Remove(otherType);
            }
            return result;
        }

        public async Task<IEnumerable<ServiceStatus>> GetAllServiceStatus()
        {
            var serviceStatusRepository = RepositoryHelper.GetRepository<IServiceStatusRepository>(UnitOfWork);
            var result = await serviceStatusRepository.GetAllAsync();
            return result;
        }

        public async Task<IEnumerable<Service>> GetAllServiceBySupplierIdAsync(int supplierId, String searchKey)
        {
            var repo = RepositoryHelper.GetRepository<IServiceRepository>(UnitOfWork);
            var result = (await repo.GetSimpleBySupplierId(supplierId)).Where(x => x.Name.Contains(searchKey));
            return result;
        }

        public async Task<BusinessLogicResult<bool>> UpdateServiceAsync(Service serviceDto)
        {
            var repo = RepositoryHelper.GetRepository<IServiceRepository>(UnitOfWork);
            var service = await repo.GetByIdAsync(serviceDto.ServiceId);
            // Update field:
            service.ServiceStatusId = serviceDto.ServiceStatusId;
            service.ServiceStatu = null;

            service.ServiceTypeId = serviceDto.ServiceTypeId;
            service.ServiceType = null;

            service.Price = serviceDto.Price;
            service.Name = serviceDto.Name;
            var updateResult = await UnitOfWork.SaveChangesAsync();
            if (updateResult.Any())
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = updateResult.AsFluentValidationResult()
                };
            }
            return new BusinessLogicResult<bool>
            {
                Success = true,
                Result = true
            };


        }

        public async Task<Service> GetServiceById(int serviceId)
        {
            var repo = RepositoryHelper.GetRepository<IServiceRepository>(UnitOfWork);
            var result = await repo.GetByIdAsync(serviceId);
            return result;
        }

        public async Task<IEnumerable<Service>> GetAllServices()
        {
            var repo = RepositoryHelper.GetRepository<IServiceRepository>(UnitOfWork);
            var service = (await repo.GetAllAsync()).Select(x => new Service { ServiceId = x.ServiceId, Name = x.Name, Price = x.Price });
            return service;
        }

        public async Task<BusinessLogicResult<bool>> DeleteServiceByIdAsync(int id)
        {
            var repo = RepositoryHelper.GetRepository<IServiceRepository>(UnitOfWork);
            var service = await repo.GetSimpleByIdAsync(id);
            if (service != null)
            {
                var warningInfo = new StringBuilder();
                var promotionDetails = service.PromotionDetails.Where(x => x.Promotion?.PromotionStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.PromotionStatus.Active).ToList();
                var activePromotions = promotionDetails.Select(x => x.Promotion).ToList();
                if (activePromotions != null && activePromotions.Count() > 0)
                {
                    warningInfo.AppendLine("<h3>Không thể xóa được dịch vụ khi nó đang thêm và trong một hoặc nhiều giảm giá </h3>");
                    warningInfo.AppendLine("<br> <br/> <b> Cụ thể: </b> <br>");
                    foreach (var item in activePromotions)
                    {
                        warningInfo.AppendLine("<h4>" + item.Title + $" từ ngày {item.EffectiveStartDate.Value.ToString("dd-MM-yyyy")} đến ngày {item.EffectiveEndDate.Value.ToString("dd-MM-yyyy")} <br> </h4>");
                    }
                    var warningError = new FluentValidation.Results.ValidationFailure("Xóa dịch vụ", warningInfo.ToString());
                    warningError.Severity = FluentValidation.Severity.Info;
                    return new BusinessLogicResult<bool>
                    {
                        Result = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { warningError })
                    };
                }
                service.IsDeleted = true;
                repo.Update(service, AccountId.ToString());
                var dbValresults = await UnitOfWork.SaveChangesAsync();
                if (dbValresults.Any())
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = dbValresults.AsFluentValidationResult(),
                    };
                }

            }
            return new BusinessLogicResult<bool>
            {
                Success = true,
                Result = true
            };
        }

        public async Task<IEnumerable<Service>> GetAllServiceBySupplierId(int supplierId)
        {
            var repo = RepositoryHelper.GetRepository<IServiceRepository>(UnitOfWork);
            var result = await repo.GetSimpleBySupplierId(supplierId);
            result = result.Where(x => x.ServiceStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.ServiceStatus.Active).ToList();
            foreach (var service in result)
            {
                service.PromotionDetails = service.PromotionDetails.Where(x => x.Promotion.EffectiveStartDate.Value.Date <= DateTime.Now.Date && DateTime.Now.Date <= x.Promotion.EffectiveEndDate.Value.Date).ToList();
            }
            return result;
        }

        public async Task<IEnumerable<ServiceType>> GetAllServiceType()
        {
            var result = await UnitOfWork.Context.Set<ServiceType>().ToListAsync();
            var chuchimnho = result.Select(x => new ServiceType { Name = x.Name, ServiceTypeId = x.ServiceTypeId }).ToList();
            return result;
        }

        public async Task<IEnumerable<Service>> GetAllServiceByServiceTypeIdAsync(int serviceTypeId)
        {
            var result = await UnitOfWork.Context.Set<Service>()
                .Include(x => x.Supplier)
                .Include(x => x.PromotionDetails.Select(y => y.Promotion))
                .Where(x => x.ServiceTypeId == serviceTypeId && x.ServiceStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.ServiceStatus.Active &&
                    x.Supplier.SupplierStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.SupplierStatusEnum.Active
                )
                .OrderBy(x => x.Price)
                .ToListAsync();
            foreach (var item in result)
            {
                item.PromotionDetails = item.PromotionDetails.Where(x => x.Promotion.EffectiveStartDate.Value.Date <= DateTime.Now.Date && DateTime.Now.Date <= x.Promotion.EffectiveEndDate.Value.Date).ToList();
            }
            return result;
        }
    }
}
