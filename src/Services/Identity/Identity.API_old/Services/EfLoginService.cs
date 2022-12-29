using Identity.API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services;

public class EfLoginService : ILoginService<AppUser>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public EfLoginService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> ValidateCredentials(AppUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<AppUser?> FindByUsername(string user)
    {
        return await _userManager.FindByEmailAsync(user);
    }

    public Task SignIn(AppUser user)
    {
        return _signInManager.SignInAsync(user, true);
    }

    public Task SignInAsync(AppUser user, AuthenticationProperties properties, string? authenticationMethod = null)
    {
        return _signInManager.SignInAsync(user, properties, authenticationMethod);
    }
}