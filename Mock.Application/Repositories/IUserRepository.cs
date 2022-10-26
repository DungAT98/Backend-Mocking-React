using Microsoft.AspNetCore.Identity;

namespace Mock.Application.Repositories;

public interface IUserRepository
{
    Task<IdentityUser?> FindUser(string username);
    
    Task<IdentityResult> AddUser(IdentityUser user, string password);

    Task<bool> CanUserLoggedIn(string username, string password);
}