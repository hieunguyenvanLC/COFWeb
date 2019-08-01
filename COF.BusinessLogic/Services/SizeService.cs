using COF.BusinessLogic.Models.Common;
using COF.DataAccess.EF.Models;
using COF.DataAccess.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface ISizeService
    {
        Task<List<SizeModel>> GetAllSizesAsync();
        Task<Size> GetByIdAsync(int id);
    }
    public class SizeService : ISizeService
    {
        #region fields
        private readonly ISizeRepository _sizeRepository;
        #endregion

        #region ctor
        public SizeService(ISizeRepository sizeRepository)
        {
            _sizeRepository = sizeRepository;
        }
        #endregion

        #region public methods
        public async Task<List<SizeModel>> GetAllSizesAsync()
        {
           var allSizes = await _sizeRepository.GetAllAsync();
           var res = allSizes.Select(x => new SizeModel
           {
               Id = x.Id,
               Name = x.Name

           }).ToList();
           return res;
        }

        public async Task<Size> GetByIdAsync(int id)
        {
            return await _sizeRepository.GetByIdAsync(id);
        }

        #endregion
    }
}
