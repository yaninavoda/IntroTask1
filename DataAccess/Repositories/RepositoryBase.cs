using Contracts;
using Microsoft.EntityFrameworkCore;
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

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
    bool trackChanges)
    {
        return !trackChanges ?
          _context.Set<T>().Where(expression).AsNoTracking() :
          _context.Set<T>().Where(expression);
    }

    public void Create(T entity) => _context.Set<T>().Add(entity);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public void Delete(T entity) => _context.Set<T>().Remove(entity);
}
