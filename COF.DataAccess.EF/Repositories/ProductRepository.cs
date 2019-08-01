using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Repositories
{
    public partial interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetAllProductAsync(int shopId, string keyword);
    }

    public partial class ProductRepository : EFRepository<Product> ,IProductRepository
    {
        public Task<List<Product>> GetAllProductAsync(int shopId, string keyword)
        {
            return _dbSet.Where(x => x.ShopId == shopId && (string.IsNullOrEmpty(keyword) || x.ProductName.Contains(keyword)))
                         .Include(x => x.ProductSizes.Select(y => y.Size))
                         .ToListAsync();

        }
    }
}
