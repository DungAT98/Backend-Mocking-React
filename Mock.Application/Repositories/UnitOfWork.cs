using Microsoft.AspNetCore.Identity;
using Mock.Application.Databases;
using Mock.Domain.Abstractions;
using Mock.Domain.Entities;

namespace Mock.Application.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MockContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    private IBaseRepository<Category>? _categoryRepository;

    public UnitOfWork(MockContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IUserRepository UserRepository
    {
        get
        {
            if (_userRepository == null)
            {
                _userRepository = new UserRepository(_userManager);
            }

            return _userRepository;
        }
    }

    private IUserRepository? _userRepository;

    public IBaseRepository<Category> CategoryRepository
    {
        get
        {
            if (_categoryRepository == null)
            {
                _categoryRepository = new BaseRepository<Category>(_context);
            }

            return _categoryRepository;
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            if (entry.Entity is EntityBase entityEntry)
            {
                entityEntry.ModifiedTime = DateTime.Now;
            }
        }

        return await _context.SaveChangesAsync();
    }
}