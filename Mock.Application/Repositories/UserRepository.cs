using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mock.Application.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserRepository(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityUser?> FindUser(string username)
    {
        var result = await _userManager.Users.FirstOrDefaultAsync(n => n.UserName.ToLower() == username.ToLower());

        return result;
    }

    public async Task<IdentityResult> AddUser(IdentityUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);

        return result;
    }

    public async Task<bool> CanUserLoggedIn(string username, string password)
    {
        var user = await FindUser(username);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.CheckPasswordAsync(user, password);

        return result;
    }
}