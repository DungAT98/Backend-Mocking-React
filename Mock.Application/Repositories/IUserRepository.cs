using Microsoft.AspNetCore.Identity;
using Mock.Domain.Entities;

namespace Mock.Application.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> FindUser(string username);

    Task<IdentityResult> AddUser(ApplicationUser user, string password);

    Task<bool> CanUserLoggedIn(string username, string password);
    
    Task<bool> CanUserLoggedIn(ApplicationUser? user, string password);
}