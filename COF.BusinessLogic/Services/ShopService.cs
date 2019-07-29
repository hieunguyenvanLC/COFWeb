using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using COF.DataAccess.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface IShopService
    {
        Task<List<Shop>> GetAllShopAsync(int partnerId);
        Task<Shop> GetByIdAsync(int id);
        void AddShopAsync(List<Shop> shops);
    }
    public class ShopService : IShopService
    {
        #region fields

        private readonly IShopRepository _shopRepository;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region ctor
        public ShopService
        (
            IUnitOfWork unitOfWork,
            IShopRepository shopRepository
        )
        {
            _shopRepository = shopRepository;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region public methods
        public async Task<List<Shop>> GetAllShopAsync(int partnerId)
        {
            var shops = await _shopRepository.GetAllAsync();
            return shops;
        }


        public void AddShopAsync(List<Shop> shops)
        {
            try
            {
                _shopRepository.AddMultiple(shops);


                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
            }
           
        }

        public async Task<Shop> GetByIdAsync(int id)
        {
            return await _shopRepository.GetByIdAsync(id);
        }
        #endregion


    }
}
