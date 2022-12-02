﻿using Microsoft.OpenApi.Models;

namespace AppointmentService.API.Extensions;

public static class SwaggerServiceExtensions
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(x => x.FullName);
            c.ResolveConflictingActions(x => x.First());
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Appointment API v1.0", Version = "v1" });
 
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Please insert JWT token into field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}