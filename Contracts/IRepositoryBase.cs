using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Contracts;

public interface IRepositoryBase<T> where T : class
{
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<IEnumerable<T>?> GetAllAsync(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<T?> GetSingleOrDefaultAsync(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        bool trackChanges = false);
}
