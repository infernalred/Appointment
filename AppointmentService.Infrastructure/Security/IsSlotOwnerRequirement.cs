using System.Security.Claims;
using AppointmentService.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AppointmentService.Infrastructure.Security;

public class IsSlotOwnerRequirement : IAuthorizationRequirement
{
}

public class IsSlotOwnerRequirementHandler : AuthorizationHandler<IsSlotOwnerRequirement>
{
    private readonly DataContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IsSlotOwnerRequirementHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsSlotOwnerRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (userId == null) return Task.CompletedTask;

        var slotId = Guid.Parse(_httpContextAccessor.HttpContext?.Request.RouteValues
            .SingleOrDefault(x => x.Key == "id").Value?.ToString() ?? throw new InvalidOperationException());

        var slot = _dbContext.TimeSlots
            .SingleOrDefaultAsync(x => x.MasterId == userId && x.Id == slotId).Result;

        if (slot != null) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}