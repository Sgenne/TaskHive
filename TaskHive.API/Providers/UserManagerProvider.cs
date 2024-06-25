using Microsoft.AspNetCore.Identity;
using TaskHive.API.Models;

namespace TaskHive.API.Providers;

public interface IUserManagerProvider
{
    Task<User?> FindByEmailAsync(string email);
    Task<IdentityResult> CreateAsync(User user, string password);
    Task<List<string>> GetRolesAsync(User user);
    Task<bool> CheckPasswordAsync(User user, string password);
}

public class UserManagerProvider(UserManager<User> userManager) : IUserManagerProvider
{
    public async Task<User?> FindByEmailAsync(string email)
    {
        var result = await userManager.FindByEmailAsync(email);

        return result;
    }

    public async Task<IdentityResult> CreateAsync(User user, string password)
    {
        var result = await userManager.CreateAsync(user, password);

        return result;
    }

    public async Task<List<string>> GetRolesAsync(User user)
    {
        var roles = await userManager.GetRolesAsync(user);

        return roles.ToList();
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        var result = await userManager.CheckPasswordAsync(user, password);

        return result;
    }
}
