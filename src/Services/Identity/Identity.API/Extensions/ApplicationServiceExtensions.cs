using Identity.API.Configuration;
using Identity.API.Data;
using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(opt => { opt.UseNpgsql(connectionString); });

        services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.ConfigureApplicationCookie(opt =>
        {
            opt.Cookie.Name = "IdentityServer.Cookies";
        });

        services.AddIdentityServer()
            .AddAspNetIdentity<AppUser>()
            .AddInMemoryClients(Config.Clients)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryIdentityResources(Config.IdentityResources);

        return services;
    }
}