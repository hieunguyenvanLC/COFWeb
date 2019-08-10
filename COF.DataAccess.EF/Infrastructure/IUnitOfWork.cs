using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        EFTransaction BeginTransaction();
    }
}