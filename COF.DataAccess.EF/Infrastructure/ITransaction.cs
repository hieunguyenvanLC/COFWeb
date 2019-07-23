using System;
using System.Collections.Generic;
using System.Text;

namespace COF.DataAccess.EF.Infrastructure
{
    public interface ITransaction : IDisposable
    {
        //Commits all changes made to the database in the current transaction.
        void Commit();

        //Discards all changes made to the database in the current transaction.
        void Rollback();
    }
}
