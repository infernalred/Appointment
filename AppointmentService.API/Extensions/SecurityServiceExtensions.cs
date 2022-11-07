using System.Text;
using AppointmentService.Application.Contracts;
using AppointmentService.Infrastructure.Security;
using AppointmentService.API.Services;
using AppointmentService.Domain;
using AppointmentService.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AppointmentService.API.Extensions;

public static class SecurityServiceExtensions
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 8;
            })
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<DataContext>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddDefaultTokenProviders();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true
                };
            });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("IsSlotOwner", policy => { policy.Requirements.Add(new IsSlotOwnerRequirement()); });
        });
        services.AddTransient<IAuthorizationHandler, IsSlotOwnerRequirementHandler>();
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}