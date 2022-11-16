using AppointmentService.Application.Helpers;
using AppointmentService.Application.Masters;
using AppointmentService.Persistence.Context;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace AppointmentService.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), 
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        LogManager.Configuration.Variables["DefaultConnection"] = configuration.GetConnectionString("DefaultConnection");
        services.AddAutoMapper(typeof(MappingProfiles).Assembly);
        services.AddMediatR(typeof(Details).Assembly);
        services.AddControllers(opt =>
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            opt.Filters.Add(new AuthorizeFilter(policy));
        });

        services.AddValidatorsFromAssemblyContaining(typeof(SlotParamsValidator));
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        return services;
    }
}