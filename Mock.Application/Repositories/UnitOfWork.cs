using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mock.Application.Databases;
using Mock.Domain.Abstractions;
using Mock.Domain.Entities;

namespace Mock.Application.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MockContext _context;

    public UnitOfWork(MockContext context)
    {
        _context = context;
    }

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
        foreach (EntityEntry entry in _context.ChangeTracker.Entries())
        {
            if (entry.Entity is EntityBase entityEntry)
            {
                entityEntry.ModifiedTime = DateTime.Now;
            }
        }

        return await _context.SaveChangesAsync();
    }

    private IBaseRepository<Category>? _categoryRepository;
}