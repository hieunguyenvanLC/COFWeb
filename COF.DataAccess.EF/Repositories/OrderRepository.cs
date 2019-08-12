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
    }

    public partial class OrderRepository : EFRepository<Order>, IOrderRepository
    {
        public List<Order> GetDailyOrders(int partnerId)
        {
            var today = DateTime.UtcNow.Date;
            var result = _dbSet
                .Include(x => x.Customer)
                .Include(x => x.User)
                .Include(x => x.Shop)
                .Where(x => x.PartnerId == partnerId && DbFunctions.TruncateTime(x.CreatedOnUtc) == today).ToList();
            return result;
        }
    }
}
