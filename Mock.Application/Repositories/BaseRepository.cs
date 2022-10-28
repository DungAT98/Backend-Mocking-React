using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mock.Application.Databases;
using Mock.Domain.Abstractions;

namespace Mock.Application.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : EntityBase
{
    private readonly MockContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public BaseRepository(MockContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }
    
    public void Add(TEntity entity)
    {
        entity.CreatedTime = DateTime.Now;
        entity.ModifiedTime = DateTime.Now;
        _dbSet.Add(entity);
        _context.Entry(entity).State = EntityState.Added;
    }

    public void Delete(TEntity entity, bool isHardDelete = false)
    {
        if (isHardDelete)
        {
            _dbSet.Remove(entity);
            _context.Entry(entity).State = EntityState.Deleted;    
        }
        else
        {
            entity.IsDeleted = true;
        }
    }

    public void Delete(Guid id, bool isHardDelete = false)
    {
        var entity = GetById(id);
        if (entity != null)
        {
            Delete(entity, isHardDelete);
        }
    }

    public TEntity? GetById(Guid id)
    {
        return GetQuery(n => n.Id == id).FirstOrDefault();
    }

    public IQueryable<TEntity> GetQuery()
    {
        return _dbSet;
    }

    public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> expression)
    {
        return _dbSet.Where(expression);
    }
}