using Identity.API.Models;
using Identity.API.Models.AccountViewModel;
using Identity.API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[Route("[controller]")]
public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILoginService<AppUser> _loginService;

    public AccountController(UserManager<AppUser> userManager,  SignInManager<AppUser> signInManager, ILoginService<AppUser> loginService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _loginService = loginService;
    }

    // [AllowAnonymous]
    // [HttpPost("[action]")]
    // public async Task<IActionResult> Register(RegisterViewModel model)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return ValidationProblem();
    //     }
    //
    //     var user = await _userManager.FindByNameAsync(model.Email);
    //     if (user != null)
    //     {
    //         ModelState.AddModelError("email", "Email taken");
    //         return ValidationProblem();
    //     }
    //
    //     user = new AppUser {UserName = model.Email, Email = model.Email, DisplayName = model.DisplayName};
    //     var result = await _userManager.CreateAsync(user, model.Password);
    //     if (result.Succeeded)
    //     {
    //         return Ok();
    //     }
    //
    //     return BadRequest(result.Errors);
    // }

    [AllowAnonymous]
    [Route("[action]")]
    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        return View();
    }

    [AllowAnonymous]
    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        // var user = await _loginService.FindByUsername(model.Email);
        //
        // if (user == null) return Unauthorized();
        //
        // if (!await _loginService.ValidateCredentials(user, model.Password)) return Unauthorized();
        //
        // var props = new AuthenticationProperties
        // {
        //     ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120),
        //     AllowRefresh = true,
        //     RedirectUri = "/"
        // };
        //
        // await _loginService.SignInAsync(user, props);
        //
        // return Ok();
        return View();
    }
}