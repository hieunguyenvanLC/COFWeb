﻿using COF.BusinessLogic.Models.User;
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
            _roleDbSet = _context.Set<AppRole>();
        }

        public async Task<BusinessLogicResult<List<UserRoleModel>>> GetAppUsersByPartnerId(int partnerId)
        {
            var sql = @"select u.Id as UserId, u.FullName, u.Email,
                        u.PasswordHash, u.userName, u.Phonenumber,
                        (select STRING_AGG(r.Name, ',') from[Role] r join[UserRole] ur
                        on r.Id = ur.RoleId
                        where ur.UserId = u.Id ) as Roles
                        from[User] u
                        where PartnerId = @p0";

            var result  =  await _context.Database.SqlQuery<UserRoleModel>(sql, partnerId).ToListAsync();

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
