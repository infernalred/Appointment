using System.Security.Claims;
using AppointmentService.Application.Contracts;
using Microsoft.AspNetCore.Http;

namespace AppointmentService.Infrastructure.Security;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUsername() => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)!;
}