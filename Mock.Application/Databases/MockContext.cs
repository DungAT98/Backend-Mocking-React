using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Mock.Application.Extensions;
using Mock.Domain.Abstractions;
using Mock.Domain.Entities;

namespace Mock.Application.Databases;

public class MockContext : IdentityDbContext
{
    public MockContext(DbContextOptions<MockContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyGlobalFilters<ISoftDelete>(n => !n.IsDeleted);
        
        var hasher = new PasswordHasher<IdentityUser>();
        var user = new IdentityUser()
        {
            UserName = "systemadmin",
            NormalizedUserName = "SYSTEMADMIN",
            PasswordHash = hasher.HashPassword(null!, "123")
        };

        builder.Entity<IdentityUser>().HasData(user);
    }
}