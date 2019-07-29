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
    public partial interface IShopRepository : IRepository<Shop>
    {
        Task<List<Shop>> GetAllShopByPartnerIdAsync(int parnterId);
    }
    public partial class ShopRepository : EFRepository<Shop>, IShopRepository
    {
        public async Task<List<Shop>> GetAllShopByPartnerIdAsync(int parnterId)
        {
            var result = await _dbSet
                                .Where(x => x.PartnerId == parnterId).ToListAsync();
            return result;
        }
    }
}
