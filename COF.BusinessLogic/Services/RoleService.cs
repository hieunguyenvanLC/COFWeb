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
    public interface IRoleService
    {
        Task<List<AppRole>> GetAllRoles();
        Task<AppRole> GetByIdAsync(string id);
        Task<AppRole> GetByNameAsync(string name);

    }
    public class RoleService : IRoleService
    {
        #region fields
        private readonly EFContext _context;
        private readonly DbSet<AppRole> _appRoles;
        #endregion
        public RoleService(EFContext context)
        {
            _context = context;
            _appRoles = _context.Set<AppRole>();
        }
        #region ctor
        #endregion
        #region public methods
        public async Task<List<AppRole>> GetAllRoles()
        {
            return await _appRoles.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<AppRole> GetByIdAsync(string id)
        {
            return await _appRoles.FindAsync(id);
        }

        public async Task<AppRole> GetByNameAsync(string name)
        {
            return await _appRoles.FirstOrDefaultAsync(x => x.Name == name);
        }
        #endregion
    }
}
