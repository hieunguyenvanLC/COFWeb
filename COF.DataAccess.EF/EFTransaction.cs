using COF.DataAccess.EF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace COF.DataAccess.EF
{
    public class EFTransaction : ITransaction
    {
        private readonly DbContextTransaction _transaction;

        public EFTransaction(EFContext context)
        {
            _transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }
    }
}
