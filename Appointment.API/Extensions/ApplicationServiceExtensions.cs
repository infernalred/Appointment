using Appointment.Application.Helpers;
using Appointment.Application.Masters;
using Appointment.Persistence.Context;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Appointment.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddMemoryCache();
        services.AddAutoMapper(typeof(MappingProfiles).Assembly);
        services.AddMediatR(typeof(Details).Assembly);
        services.AddControllers(opt =>
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            opt.Filters.Add(new AuthorizeFilter(policy));
        });

        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        return services;
    }
}