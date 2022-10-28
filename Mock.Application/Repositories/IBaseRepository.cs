using System.Linq.Expressions;

namespace Mock.Application.Repositories;

public interface IBaseRepository<TEntity>
{
    void Add(TEntity entity);

    void Delete(TEntity entity, bool isHardDelete = false);

    void Delete(Guid id, bool isHardDelete = false);

    TEntity? GetById(Guid id);

    IQueryable<TEntity> GetQuery();

    IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> expression);
}