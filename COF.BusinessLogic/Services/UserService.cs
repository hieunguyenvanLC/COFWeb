using COF.DataAccess.EF;
using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface IUserService
    {
        Task<AppUser> GetByIdAsync(string userId);
    }
    public class UserService : IUserService
    {
        private readonly EFContext _context;
        private readonly DbSet<AppUser> _dbSet;
        public UserService(EFContext context)
        {
            _context = context;
            _dbSet = _context.Set<AppUser>();
        }
        public async Task<AppUser> GetByIdAsync(string userId)
        {
            return await _dbSet.SingleOrDefaultAsync(x => x.Id == userId);
        }
    }
}
