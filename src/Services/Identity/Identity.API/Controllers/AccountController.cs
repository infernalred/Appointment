using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Identity.API.Configuration;
using Identity.API.Models;
using Identity.API.Models.AccountViewModel;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[Route("[controller]/[action]")]
public class AccountController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly IClientStore _clientStore;
    private readonly IAuthenticationHandlerProvider _handlerProvider;

    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        IIdentityServerInteractionService interactionService, IClientStore clientStore,
        IAuthenticationHandlerProvider handlerProvider)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _interactionService = interactionService;
        _clientStore = clientStore;
        _handlerProvider = handlerProvider;
    }

    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl)
    {
        var vm = await BuildLoginViewModelAsync(returnUrl);

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(await BuildLoginViewModelAsync(model));
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("UserName", "User not found");
            return View(await BuildLoginViewModelAsync(model));
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
        if (result.Succeeded)
        {
            return Redirect(_interactionService.IsValidReturnUrl(model.ReturnUrl)
                ? model.ReturnUrl
                : "~/");
        }

        ModelState.AddModelError("User", "Something went wrong");
        var vm = await BuildLoginViewModelAsync(model);
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        // await _signInManager.SignOutAsync();
        // var result = await _interactionService.GetLogoutContextAsync(logoutId);
        // if (string.IsNullOrEmpty(result.PostLogoutRedirectUri))
        // {
        //     return RedirectToAction("Index", "Home");
        // }
        // return Redirect(result.PostLogoutRedirectUri);
        var vm = await BuildLogoutViewModelAsync(logoutId);

        if (!vm.ShowLogoutPrompt)
        {
            return await Logout(vm);
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutViewModel model)
    {
        var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

        if (User.Identity?.IsAuthenticated == true)
        {
            await _signInManager.SignOutAsync();
        }

        if (vm.TriggerExternalSignOut)
        {
            var url = Url.Action("Logout", new {logoutId = vm.LogoutId});
            return SignOut(new AuthenticationProperties {RedirectUri = url}, vm.ExternalAuthenticationScheme);
        }

        return View("LoggedOut", vm);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid) return View(model);

        var user = new AppUser
        {
            UserName = model.UserName,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return LocalRedirect(returnUrl);
        }

        AddErrors(result);

        return View(model);
    }


    private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
    {
        var context = await _interactionService.GetAuthorizationContextAsync(returnUrl);
        if (context?.IdP != null)
        {
            throw new NotImplementedException("External login is not implemented!");
        }

        var allowLocal = true;
        if (context?.Client.ClientId != null)
        {
            var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
            if (client != null)
            {
                allowLocal = client.EnableLocalLogin;
            }
        }

        return new LoginViewModel
        {
            AllowRememberLogin = AccountOptions.AllowRememberLogin,
            EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
            ReturnUrl = returnUrl,
            Email = context?.LoginHint ?? string.Empty,
        };
    }

    private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
    {
        var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
        vm.Email = model.Email;
        vm.RememberMe = model.RememberMe;
        return vm;
    }

    private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
    {
        var vm = new LogoutViewModel
        {
            LogoutId = logoutId,
            ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt
        };

        if (User.Identity?.IsAuthenticated != true)
        {
            vm.ShowLogoutPrompt = false;
            return vm;
        }

        var context = await _interactionService.GetLogoutContextAsync(logoutId);
        if (context?.ShowSignoutPrompt != false) return vm;

        vm.ShowLogoutPrompt = false;
        return vm;
    }

    private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string? logoutId)
    {
        var logout = await _interactionService.GetLogoutContextAsync(logoutId);

        var vm = new LoggedOutViewModel
        {
            AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
            PostLogoutRedirectUri = logout.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout.ClientName) ? logout.ClientId : logout.ClientName,
            SignOutIframeUrl = logout.SignOutIFrameUrl,
            LogoutId = logoutId
        };

        if (User.Identity?.IsAuthenticated == true)
        {
            var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
            {
                var handler = await _handlerProvider.GetHandlerAsync(HttpContext, idp);
                if (handler is IAuthenticationSignOutHandler)
                {
                    if (vm.LogoutId == null)
                    {
                        vm.LogoutId = await _interactionService.CreateLogoutContextAsync();
                    }

                    vm.ExternalAuthenticationScheme = idp;
                }
            }
        }

        return vm;
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}