using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ThesisERP.Application.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T?> GetByIdAsync<TId>(TId id) where TId : notnull;
        
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null,
                                  Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                  Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
      
        Task<int> CountAsync(Expression<Func<T, bool>>? expression = null);

        Task<T> AddAsync(T entity);
        
        Task UpdateAsync(T entity);
        
        Task DeleteAsync(T entity);
        
        Task DeleteRangeAsync(IEnumerable<T> entities);
        
        Task SaveChangesAsync();
    }
}
