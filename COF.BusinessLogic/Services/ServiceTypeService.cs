using CapstoneProjectServer.DataAccess.EF.Infrastructure;
using CapstoneProjectServer.DataAccess.EF.Models;
using CapstoneProjectServer.DataAccess.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface IServiceTypeService : ILogic
    {
        Task<IEnumerable<ServiceType>> GetAllServiceType();


    }
    public class ServiceTypeService : BaseService, IServiceTypeService
    {
        public ServiceTypeService(IRepositoryHelper repositoryHelper)
        {
            this.RepositoryHelper = repositoryHelper;
            this.UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepositoryHelper RepositoryHelper;

        public async Task<IEnumerable<ServiceType>> GetAllServiceType()
        {
            var serviceTypeRepository = RepositoryHelper.GetRepository<IServiceTypeRepository>(UnitOfWork);
            var result = await serviceTypeRepository.GetAllAsync();
            return result;
        }


    }
}
