using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mock.Application.Extensions;
using Mock.Domain.Abstractions;
using Mock.Domain.Entities;

namespace Mock.Application.Databases;

public class MockContext : IdentityDbContext<ApplicationUser>
{
    public MockContext(DbContextOptions<MockContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyGlobalFilters<ISoftDelete>(n => !n.IsDeleted);

        var hasher = new PasswordHasher<ApplicationUser>();
        var user = new ApplicationUser()
        {
            UserName = "systemadmin",
            NormalizedUserName = "SYSTEMADMIN",
            PasswordHash = hasher.HashPassword(null!, "123")
        };

        builder.Entity<ApplicationUser>().HasData(user);
    }
}