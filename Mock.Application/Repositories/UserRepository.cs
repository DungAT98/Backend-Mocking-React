using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mock.Domain.Entities;

namespace Mock.Application.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> FindUser(string username)
    {
        var result = await _userManager.Users.FirstOrDefaultAsync(n => n.UserName.ToLower() == username.ToLower());

        return result;
    }

    public async Task<IdentityResult> AddUser(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);

        return result;
    }

    public async Task<bool> CanUserLoggedIn(string username, string password)
    {
        var user = await FindUser(username);

        var result = await CanUserLoggedIn(user, password);

        return result;
    }

    public async Task<bool> CanUserLoggedIn(ApplicationUser? user, string password)
    {
        if (user == null)
        {
            return false;
        }
        
        var result = await _userManager.CheckPasswordAsync(user, password);

        return result;
    }
}