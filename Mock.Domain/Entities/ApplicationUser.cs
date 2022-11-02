using Microsoft.AspNetCore.Identity;

namespace Mock.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiredIn { get; set; }
}