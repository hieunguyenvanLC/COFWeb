
using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace COF.DataAccess.EF
{
    public class EFRepository<T> : IRepository<T>, IDisposable where T : BaseEntity
    {
        #region fields
        private readonly EFContext _context;
        public readonly DbSet<T> _dbSet;
        #endregion

        #region ctor
        public EFRepository(EFContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        #endregion

        #region dispose
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
        #endregion

        #region iqueryable
        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, object>>[] includeProperties = null, int? countSkip = null, int? countTake = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null && includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (countSkip.HasValue)
            {
                query = query.Skip(countSkip.Value);
            }

            if (countTake.HasValue)
            {
                query = query.Take(countTake.Value);
            }

            return query;
        }
        #endregion

        #region sync
        //get
        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public T GetById(object id, params Expression<Func<T, object>>[] includeProperties)
        {
            return GetQueryable(null, null, includeProperties).SingleOrDefault(x => x.Id.Equals(id));
        }

        public T GetSingle(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>>[] includeProperties = null)
        {
            return GetQueryable(filter, null, includeProperties).SingleOrDefault();
        }

        public List<T> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, object>>[] includeProperties = null, int? countSkip = null, int? countTake = null)
        {
            return GetQueryable(null, orderBy, includeProperties, countSkip, countTake).ToList();
        }

        public List<T> GetByFilter(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, object>>[] includeProperties = null, int? countSkip = null, int? countTake = null)
        {
            return GetQueryable(filter, orderBy, includeProperties, countSkip, countTake).ToList();
        }

        //insert
        public void Add(T entity)
        {
            entity.CreatedOnUtc = DateTime.UtcNow;
            _dbSet.Add(entity);
        }

        public void AddMultiple(List<T> entities)
        {
            entities.ForEach(x =>
            {
                x.CreatedOnUtc = DateTime.UtcNow;
            });
            _dbSet.AddRange(entities);
        }

        //update 
        public void Update(T entity)
        {
            entity.UpdatedOnUtc = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateMultiple(List<T> entities)
        {
            entities.ForEach(x =>
            {
                x.UpdatedOnUtc = DateTime.UtcNow;
                _context.Entry(x).State = EntityState.Modified;
            });
        }

        //delete
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Remove(object id)
        {
            Remove(GetById(id));
        }

        public void RemoveMultiple(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public T MarkAsRemove(T entity)
        {
            entity.IsDeleted = true;
            Update(entity);
            return entity;
        }

        public T MarkAsRemove(object id)
        {
            return MarkAsRemove(GetById(id));
        }

        
        #endregion

        #region async
        //get
        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> GetByIdAsync(object id, params Expression<Func<T, object>>[] includeProperties)
        {
            return await GetQueryable(null, null, includeProperties).SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties)
        {
            return await GetQueryable(filter, null, includeProperties).SingleOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, object>>[] includeProperties = null, int? countSkip = null, int? countTake = null)
        {
            return await GetQueryable(null, orderBy, includeProperties, countSkip, countTake).ToListAsync();
        }

        public async Task<List<T>> GetByFilterAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, object>>[] includeProperties = null, int? countSkip = null, int? countTake = null)
        {
            return await GetQueryable(filter, orderBy, includeProperties, countSkip, countTake).ToListAsync();
        }
        #endregion
    }
}