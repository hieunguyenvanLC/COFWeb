using COF.DataAccess.EF.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace COF.DataAccess.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly EFContext _context;

        public EFUnitOfWork(EFContext context)
        {
            _context = context;
        }

        public EFTransaction BeginTransaction()
        {
            return new EFTransaction(_context);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}