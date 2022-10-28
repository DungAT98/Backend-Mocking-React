using Mock.Domain.Entities;

namespace Mock.Application.Repositories;

public interface IUnitOfWork
{
    IBaseRepository<Category> CategoryRepository { get; }

    Task<int> SaveChangesAsync();
}