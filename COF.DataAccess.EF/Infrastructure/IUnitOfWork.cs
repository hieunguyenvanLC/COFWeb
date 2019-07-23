using System;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        EFTransaction BeginTransaction();
    }
}