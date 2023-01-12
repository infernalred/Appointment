using System.Globalization;
using Identity.API.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;

namespace Identity.API.Extensions;

public static class LocalizationServiceExtensions
{
    public static IServiceCollection AddLocalizationServices<TUser, TKey>(this IServiceCollection services,
        IConfiguration configuration)
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        services.AddLocalization();

        services.AddControllersWithViews()
            .AddViewLocalization(opt => opt.ResourcesPath = "Resources")
            .AddDataAnnotationsLocalization();

        var cultureConfiguration = configuration.GetSection(nameof(CultureConfiguration)).Get<CultureConfiguration>();

        services.Configure<RequestLocalizationOptions>(opt =>
        {
            var supportedCultureCodes = (cultureConfiguration?.Cultures?.Count() > 0
                ? cultureConfiguration.Cultures.Intersect(CultureConfiguration.AvailableCultures)
                : CultureConfiguration.AvailableCultures).ToArray();

            if (!supportedCultureCodes.Any())
            {
                supportedCultureCodes = CultureConfiguration.AvailableCultures;
            }

            var supportedCultures = supportedCultureCodes.Select(x => new CultureInfo(x)).ToList();

            var defaultCultureCode = string.IsNullOrEmpty(cultureConfiguration?.DefaultCulture)
                ? CultureConfiguration.DefaultRequestCulture
                : cultureConfiguration.DefaultCulture;

            if (!supportedCultureCodes.Contains(defaultCultureCode))
            {
                defaultCultureCode = supportedCultureCodes.First();
            }

            opt.DefaultRequestCulture = new RequestCulture(defaultCultureCode);
            opt.SupportedCultures = supportedCultures;
            opt.SupportedUICultures = supportedCultures;
        });

        return services;
    }
}