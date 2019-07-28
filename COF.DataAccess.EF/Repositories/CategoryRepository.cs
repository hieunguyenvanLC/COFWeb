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
    public partial interface ICategoryRepository : IRepository<Category> 
    {
        Task<List<Category>> GetByShopId(int shopId);
    }
    public partial class CategoryRepository : EFRepository<Category>, ICategoryRepository
    {
        public async Task<List<Models.Category>> GetByShopId(int shopId)
        {
            var result = await _dbSet
                                .Include(x => x.Products.Select(y => y.ProductSizes.Select(z => z.Size)))
                                .Where(x => x.ShopId == shopId).ToListAsync();
            return result;
        }
    }
}
