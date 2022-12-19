using Identity.API.Configuration;
using Identity.API.Data;
using Identity.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });

        services.AddControllers(opt =>
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            opt.Filters.Add(new AuthorizeFilter(policy));
        });

        services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddIdentityServer()
            // .AddConfigurationStore(opt =>
            // {
            //     opt.ConfigureDbContext = b => b.UseNpgsql(connectionString,
            //         sql => sql.MigrationsAssembly(migrationsAssembly));
            // })
            // .AddOperationalStore(opt =>
            // {
            //     opt.ConfigureDbContext = b => b.UseNpgsql(connectionString,
            //         sql => sql.MigrationsAssembly(migrationsAssembly));
            // })
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<AppUser>();

        
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        return services;
    }
}