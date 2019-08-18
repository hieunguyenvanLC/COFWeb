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
    public partial interface ICustomerRepository : IRepository<Customer>
    {
        Task<List<Customer>> GetAllCustomersAsync(int partnerId, string keyword);
        int GetTotalByPartnerId(int partnerId);
    }

    public partial class CustomerRepository : EFRepository<Customer>, ICustomerRepository
    {
        public async Task<List<Customer>> GetAllCustomersAsync(int partnerId, string keyword)
        {
            return await _dbSet.Where(x => x.PartnerId == partnerId && (string.IsNullOrEmpty(keyword) || x.FullName.Contains(keyword) || x.PhoneNumber.Contains(keyword)))
                         .ToListAsync();

        }

        public int GetTotalByPartnerId(int partnerId)
        {
            return  _dbSet.Where(x => x.PartnerId == partnerId)
                         .Count();
        }
    }
}
