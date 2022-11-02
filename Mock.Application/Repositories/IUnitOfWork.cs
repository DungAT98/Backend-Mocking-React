using Mock.Domain.Entities;

namespace Mock.Application.Repositories;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    
    IBaseRepository<Category> CategoryRepository { get; }

    Task<int> SaveChangesAsync();
}