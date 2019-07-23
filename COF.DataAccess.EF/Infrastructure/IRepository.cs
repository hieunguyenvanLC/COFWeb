using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        #region iqueryable
        IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, object>>[] includeProperties = null, int? countSkip = null, int? countTake = null);
        #endregion

        #region sync
        //get
        T GetById(object id);
        T GetById(object id, params Expression<Func<T, object>>[] includeProperties);
        T GetSingle(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>>[] includeProperties = null);
        List<T> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, object>>[] includeProperties = null, int? countSkip = null, int? countTake = null);
        List<T> GetByFilter(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, object>>[] includeProperties = null, int? countSkip = null, int? countTake = null);

        //insert
        void Add(T entity);
        void AddMultiple(List<T> entities);

        //update
        void Update(T entity);
        void UpdateMultiple(List<T> entities);

        //delete
        void Remove(T entity);
        void Remove(object id);
        void RemoveMultiple(List<T> entities);
        T MarkAsRemove(T entity);
        T MarkAsRemove(object id);
        #endregion

        #region async
        //get
        Task<T> GetByIdAsync(object id);
        Task<T> GetByIdAsync(object id, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties);
        Task<List<T>> GetAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, object>>[] includeProperties = null, int? countSkip = null, int? countTake = null);
        Task<List<T>> GetByFilterAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, object>>[] includeProperties = null, int? countSkip = null, int? countTake = null);
        #endregion
    }
}