using Duende.IdentityServer.Services;
using Identity.API.Models;
using Identity.API.Models.AccountViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[Route("[controller]/[action]")]
public class AccountController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IIdentityServerInteractionService _interactionService;

    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        IIdentityServerInteractionService interactionService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _interactionService = interactionService;
    }

    public async Task<IActionResult> Logout(string logoutId)
    {
        await _signInManager.SignOutAsync();
        var result = await _interactionService.GetLogoutContextAsync(logoutId);
        if (string.IsNullOrEmpty(result.PostLogoutRedirectUri))
        {
            return RedirectToAction("Index", "Home");
        }
        return Redirect(result.PostLogoutRedirectUri);
    }

    public IActionResult Login(string returnUrl)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null)
        {
            ModelState.AddModelError("UserName", "User not found");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        if (result.Succeeded)
        {
            return Redirect(model.ReturnUrl);
        }

        ModelState.AddModelError("User", "Something went wrang");
        return View(model);
    }
}