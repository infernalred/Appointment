using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp1.Models;

namespace WebApp1.Controllers;

[Route("[controller]/[action]")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Logout()
    {
        return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [Authorize]
    public async Task<IActionResult> Privacy()
    {
        var jsonToken = await HttpContext.GetTokenAsync("access_token");
        var token = (JwtSecurityToken)new JwtSecurityTokenHandler().ReadToken(jsonToken);
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}