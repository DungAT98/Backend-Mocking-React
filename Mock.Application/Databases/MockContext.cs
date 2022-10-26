using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mock.Application.Databases;

public class MockContext : IdentityDbContext
{
    public MockContext(DbContextOptions<MockContext> options) : base(options) 
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
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