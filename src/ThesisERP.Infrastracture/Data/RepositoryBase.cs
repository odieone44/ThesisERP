using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Exceptions;

namespace ThesisERP.Infrastracture.Data;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly DbContext _dbContext;
    private const int UniqueIndexViolationException = 2601;

    public RepositoryBase(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual T Add(T entity)
    {
        _dbContext.Set<T>().Add(entity);

        return entity;
    }

    public virtual void Update(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public virtual void DeleteRange(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().RemoveRange(entities);

    }

    public async virtual Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null)
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

    public async virtual Task<T?> GetByIdAsync<TId>(TId id) where TId : notnull
    {
        return await _dbContext.Set<T>().FindAsync(new object[] { id });
    }
    public async virtual Task<int> CountAsync(Expression<Func<T, bool>>? expression = null)
    {
        return await _dbContext.Set<T>().CountAsync(expression);
    }

    public async virtual Task SaveChangesAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException is SqlException sqe && sqe.Number == UniqueIndexViolationException)
            {
                throw new ThesisERPUniqueConstraintException();
            }

            throw;
        }
    }
}
