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
    }
}
