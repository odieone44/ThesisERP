using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ThesisERP.Application.Interfaces;

namespace ThesisERP.Infrastracture.Data
{
    public abstract class BaseRepository<T> : IRepositoryBase<T> where T : class
    {
        private readonly DbContext _dbContext;

        public BaseRepository(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);

            await SaveChangesAsync();

            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            await SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);

            await SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);

            await SaveChangesAsync();
        }

        public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync<TId>(TId id) where TId : notnull
        {
            return await _dbContext.Set<T>().FindAsync(new object[] { id });
        }
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? expression = null)
        {
            return await _dbContext.Set<T>().CountAsync(expression);
        }

        public virtual async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }


    }
}
