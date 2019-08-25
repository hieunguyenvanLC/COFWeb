using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Repositories
{
    public partial interface IOrderRepository : IRepository<Order>
    {
        List<Order> GetDailyOrders(int partnerId);
        List<Order> GetAllOrdersInCurrentMonth(int partnerId);
        List<Order> GetAllOrdersInCurrentMonthByShop(int shopId);

        List<Order> GetAllOrdersInCurrentYear(int partnerId);
        List<Order> GetAllOrdersInCurrentYearByShop(int shopId);

        int  GetTotalOrder(int partnerId);
       
    }

    public partial class OrderRepository : EFRepository<Order>, IOrderRepository
    {
        public List<Order> GetDailyOrders(int partnerId)
        {
            var today = DateTime.UtcNow.Date.AddHours(7);
            var result = _dbSet
                .Include(x => x.Customer)
                .Include(x => x.User)
                .Include(x => x.Shop)
                .Where(x => x.PartnerId == partnerId && DbFunctions.TruncateTime(x.CreatedOnUtc) == today).ToList();
            return result;
        }

        public List<Order> GetAllOrdersInCurrentMonth(int partnerId)
        {
            var today = DateTime.UtcNow.Date.AddHours(7);
            var result = _dbSet
                .Include(x => x.Shop)
                .Where(x => x.PartnerId == partnerId && 
                x.CreatedOnUtc.Month == today.Month && x.CreatedOnUtc.Year == today.Year
                ).ToList();
            return result;
        }

        public List<Order> GetAllOrdersInCurrentMonthByShop(int shopId)
        {
            var today = DateTime.UtcNow.Date.AddHours(7);
            var result = _dbSet
                .Where(x => x.ShopId == shopId &&
                x.CreatedOnUtc.Month == today.Month && x.CreatedOnUtc.Year == today.Year
                ).ToList();
            return result;
        }

        public List<Order> GetAllOrdersInCurrentYear(int partnerId)
        {
            var today = DateTime.UtcNow.Date.AddHours(7);
            var result = _dbSet
                .Include(x => x.Shop)
                .Where(x => x.PartnerId == partnerId && x.CreatedOnUtc.Year == today.Year
                ).ToList();
            return result;
        }

        public List<Order> GetAllOrdersInCurrentYearByShop(int shopId)
        {
            var today = DateTime.UtcNow.Date.AddHours(7);
            var result = _dbSet
                .Where(x => x.ShopId == shopId && x.CreatedOnUtc.Year == today.Year
                ).ToList();
            return result;
        }
        public int GetTotalOrder(int partnerId)
        {
            return _dbSet.Where(x => x.PartnerId == partnerId).Count();
        }
    }
}
