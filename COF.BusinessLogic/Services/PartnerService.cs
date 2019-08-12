using COF.BusinessLogic.Settings;
using COF.DataAccess.EF.Models;
using COF.DataAccess.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface IPartnerService
    {
        Task<BusinessLogicResult<Partner>> GetByIdAsync(int id);
        BusinessLogicResult<Partner> GetById(int id);
        Task<BusinessLogicResult<List<Partner>>> GetAllAsync();
        BusinessLogicResult<List<Partner>> GetAll();
    }
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _partnerRepository;

        public PartnerService(IPartnerRepository partnerRepository)
        {
            _partnerRepository = partnerRepository;
        }

        public async Task<BusinessLogicResult<Partner>> GetByIdAsync(int id)
        {
            var result = await _partnerRepository.GetByIdAsync(id);
            return new BusinessLogicResult<Partner>
            {
                Result = result,
                Success = true
            };
        }

        public async Task<BusinessLogicResult<List<Partner>>> GetAllAsync()
        {
            var result = await _partnerRepository.GetAllAsync();
            return new BusinessLogicResult<List<Partner>>
            {
                Result = result,
                Success = true
            };
        }

        public BusinessLogicResult<List<Partner>> GetAll()
        {
            var result = _partnerRepository.GetAll();
            return new BusinessLogicResult<List<Partner>>
            {
                Result = result,
                Success = true
            };
        }

        public BusinessLogicResult<Partner> GetById(int id)
        {
            var result =  _partnerRepository.GetById(id);
            return new BusinessLogicResult<Partner>
            {
                Result = result,
                Success = true
            };
        }
    }
}
