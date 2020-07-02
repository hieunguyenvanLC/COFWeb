using COF.BusinessLogic.Models.Shop;
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
        Task<List<ShopModel>> GetAllShopAsync(int partnerId);
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
        public async Task<List<ShopModel>> GetAllShopAsync(int partnerId)
        {
            var shops = await _shopRepository.GetAllAsync();
            var result = shops.Select(x => new ShopModel
            {
                Id = x.Id, 
                Name = x.ShopName,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Description = x.Description
            }).ToList();
            return result;
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

        #endregion


    }
}
