using COF.BusinessLogic.Models.User;
using COF.BusinessLogic.Settings;
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
        Task<BusinessLogicResult<List<UserRoleModel>>> GetAppUsersByPartnerId(int partnerId);
    }
    public class UserService : IUserService
    {
        private readonly EFContext _context;
        private readonly DbSet<AppUser> _dbSet;
        private readonly DbSet<AppRole> _roleDbSet;
        public UserService(EFContext context)
        {
            _context = context;
            _dbSet = _context.Set<AppUser>();
            _roleDbSet = context.Set<AppRole>();
        }

        public async Task<BusinessLogicResult<List<UserRoleModel>>> GetAppUsersByPartnerId(int partnerId)
        {
            var usersWithRoles = await (from user in _dbSet
                                  where user.PartnerId == partnerId
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in _roleDbSet on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToListAsync();

            var result = usersWithRoles
            .Select(p => new UserRoleModel()

             {
                 UserId = p.UserId,
                 Username = p.Username,
                 Email = p.Email,
                 Roles = string.Join(",", p.RoleNames)
             }).ToList();

            return new BusinessLogicResult<List<UserRoleModel>>
            {
                Result = result,
                Success = true
            };
        }

        public async Task<AppUser> GetByIdAsync(string userId)
        {
            return await _dbSet.SingleOrDefaultAsync(x => x.Id == userId);
        }
    }
}
