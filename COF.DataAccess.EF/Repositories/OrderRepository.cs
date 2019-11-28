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
        Task<Order> GetByOrderCode(string orderCode);
        List<Order> GetAllOrdersInRangeByShop(int shopId, DateTime fromDate, DateTime toDate);
        List<Order> GetAllOrdersInRange(int partnerId, DateTime fromDate, DateTime toDate);
        Task<Order> GetByCodeAsync(string orderCode);

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
                .Where(x => x.PartnerId == partnerId && DbFunctions.TruncateTime(x.CheckInDate) == today).ToList();
            return result;
        }

        public List<Order> GetAllOrdersInCurrentMonth(int partnerId)
        {
            var today = DateTime.UtcNow.Date.AddHours(7);
            var result = _dbSet
                .Include(x => x.OrderDetails)
                .Include(x => x.Shop)
                .Where(x => x.PartnerId == partnerId && 
                x.CheckInDate.Month == today.Month && x.CheckInDate.Year == today.Year
                ).ToList();
            return result;
        }

        public List<Order> GetAllOrdersInCurrentMonthByShop(int shopId)
        {
            var today = DateTime.UtcNow.Date.AddHours(7);
            var result = _dbSet
                .Include(x => x.OrderDetails)
                .Where(x => x.ShopId == shopId &&
                x.CheckInDate.Month == today.Month && x.CheckInDate.Year == today.Year
                ).ToList();
            return result;
        }

        public List<Order> GetAllOrdersInCurrentYear(int partnerId)
        {
            var today = DateTime.UtcNow.Date.AddHours(7);
            var result = _dbSet
                .Include(x => x.Shop)
                .Include(x => x.OrderDetails)
                .Where(x => x.PartnerId == partnerId && x.CheckInDate.Year == today.Year
                ).ToList();
            return result;
        }

        public List<Order> GetAllOrdersInCurrentYearByShop(int shopId)
        {
            var today = DateTime.UtcNow.Date.AddHours(7);
            var result = _dbSet
                .Include(x => x.OrderDetails)
                .Where(x => x.ShopId == shopId && x.CheckInDate.Year == today.Year
                ).ToList();
            return result;
        }
        public int GetTotalOrder(int partnerId)
        {
            return _dbSet.Where(x => x.PartnerId == partnerId).Count();
        }

        public async Task<Order> GetByOrderCode(string orderCode)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.OrderCode == orderCode);
        }

        public List<Order> GetAllOrdersInRangeByShop(int shopId, DateTime fromDate, DateTime toDate)
        {
            var result = _dbSet
                .Include(x => x.OrderDetails)
                .Where(x => x.ShopId == shopId &&
                DbFunctions.TruncateTime(x.CheckInDate) >= fromDate.Date && DbFunctions.TruncateTime(x.CheckInDate) <= toDate
                ).ToList();
            return result;
        }

        public List<Order> GetAllOrdersInRange(int partnerId, DateTime fromDate, DateTime toDate)
        {
            var result = _dbSet
                .Include(x => x.OrderDetails)
                .Where(x => x.PartnerId == partnerId &&
                DbFunctions.TruncateTime(x.CheckInDate) >= fromDate.Date && DbFunctions.TruncateTime(x.CheckInDate) <= toDate.Date).ToList();
            return result;
        }

        public async Task<Order> GetByCodeAsync(string orderCode)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.OrderCode == orderCode);
        }
    }
}
