using COF.BusinessLogic.Models.Common;
using COF.DataAccess.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface ICommonService
    {
        Task<List<SizeModel>> GetAllSizesAsync();
    }
    public class CommonService : ICommonService
    {
        #region fields
        private readonly ISizeRepository _sizeRepository;
        #endregion

        #region ctor
        public CommonService(ISizeRepository sizeRepository)
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

        #endregion
    }
}
