using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataAccess.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected AppDbContext _context;

    public RepositoryBase(AppDbContext context)
        => _context = context;

    public IQueryable<T> FindAll(bool trackChanges)
    {
        return !trackChanges ?
          _context.Set<T>().AsNoTracking() :
          _context.Set<T>();
    }

    public async Task<IEnumerable<T>?> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default)
    {
        return await GetQueryable(predicate, include).ToListAsync();
    }

    public async Task<T?> GetSingleOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        return await GetQueryable(predicate, include).SingleOrDefaultAsync();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate,
    bool trackChanges)
    {
        return !trackChanges ?
          _context.Set<T>().Where(predicate).AsNoTracking() :
          _context.Set<T>().Where(predicate);
    }

    public void Create(T entity) => _context.Set<T>().Add(entity);

    public void Update(T entity) => _context.Entry(entity).State = EntityState.Modified;

    public void Delete(T entity) => _context.Set<T>().Remove(entity);

    private IQueryable<T> GetQueryable(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default)
    {
        var query = _context.Set<T>().AsNoTracking();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }
        if (include is not null)
        {
            query = include(query);
        }

        return query.AsNoTracking();
    }
}
