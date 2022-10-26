using AppointmentService.API.DTOs;
using AppointmentService.API.Services;
using AppointmentService.Domain;
using AppointmentService.Infrastructure.Security;
using AppointmentService.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly DataContext _context;

    public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        ITokenService tokenService, DataContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _context = context;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null) return Unauthorized();

        var result = await _signInManager
            .CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);
        return CreateUserDto(user, roles);
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [HttpPost("master")]
    public async Task<IActionResult> RegisterMaster(RegisterMaster registerMaster)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerMaster.Email))
            {
                ModelState.AddModelError("email", "Почта уже занята");
                return ValidationProblem();
            }

            if (await _userManager.Users.AnyAsync(x => x.UserName == registerMaster.UserName))
            {
                ModelState.AddModelError("username", "Логин занят");
                return ValidationProblem();
            }

            var service = await _context.Services
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == registerMaster.ServiceId);

            if (service == null)
            {
                ModelState.AddModelError("service", "Услуги не существует");
                return ValidationProblem();
            }

            var user = new AppUser
            {
                DisplayName = registerMaster.DisplayName,
                Email = registerMaster.Email,
                UserName = registerMaster.UserName
            };

            var result = await _userManager.CreateAsync(user, registerMaster.Password);

            if (!result.Succeeded) return BadRequest("Возникла проблема при создании мастера");

            await _userManager.AddToRoleAsync(user, Roles.Master);

            await _context.Masters.AddAsync(new Master {User = user, Service = service});
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return BadRequest("Возникла проблема при создании мастера");
        }

        return Ok();
    }

    private UserDto CreateUserDto(AppUser user, IEnumerable<string> roles)
    {
        return new UserDto
        {
            DisplayName = user.DisplayName,
            Token = _tokenService.CreateToken(user, roles),
            UserName = user.UserName
        };
    }
}