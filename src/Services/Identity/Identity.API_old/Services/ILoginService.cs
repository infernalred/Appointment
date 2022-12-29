using Identity.API.Models;
using Microsoft.AspNetCore.Authentication;

namespace Identity.API.Services;

public interface ILoginService<in T>
{
    Task<bool> ValidateCredentials(T user, string password);

    Task<AppUser?> FindByUsername(string user);

    Task SignIn(T user);

    Task SignInAsync(T user, AuthenticationProperties properties, string? authenticationMethod = null);
}