using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ThesisERP.Application.Interfaces;

public interface IRepositoryBase<T> where T : class
{
    Task<T?> GetByIdAsync<TId>(TId id) where TId : notnull;

    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null,
                              Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                              Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null);

    Task<int> CountAsync(Expression<Func<T, bool>>? expression = null);

    T Add(T entity);

    void Update(T entity);

    void Delete(T entity);

    void DeleteRange(IEnumerable<T> entities);

    Task SaveChangesAsync();
}
