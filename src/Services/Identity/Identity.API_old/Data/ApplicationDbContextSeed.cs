using System.Text.RegularExpressions;
using Identity.API.Extensions;
using Identity.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Data;

public class ApplicationDbContextSeed
{
    //private readonly IPasswordHasher<AppUser> _passwordHasher = new PasswordHasher<AppUser>();

    public async Task SeedAsync(WebApplication app, ILogger<ApplicationDbContextSeed> logger)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        const string adminRole = "Admin";
        const string managerRole = "Manager";
        const string masterRole = "Master";

        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
            await roleManager.CreateAsync(new IdentityRole(managerRole));
            await roleManager.CreateAsync(new IdentityRole(masterRole));
        }

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        if (!userManager.Users.Any())
        {
            var users = GetUsersFromFile(env.ContentRootPath, logger);
            foreach (var user in users)
            {
                await userManager.CreateAsync(user, user.PasswordHash ?? "P@ssword1");
            }
        }
    }

    private IEnumerable<AppUser> GetUsersFromFile(string contentRootPath, ILogger logger)
    {
        var file = Path.Combine(contentRootPath, "Setup", "Users.csv");

        if (!File.Exists(file))
        {
            return GetDefaultUsers();
        }

        string[] csvHeaders;
        try
        {
            string[] requiredHeaders =
            {
                "email", "displayname",
                "username", "password"
            };
            csvHeaders = GetHeaders(requiredHeaders, file);
        }
        catch (Exception e)
        {
            logger.LogError(e, "EXCEPTION ERROR: {Message}", e.Message);

            return GetDefaultUsers();
        }

        var users = File.ReadAllLines(file)
            .Skip(1)
            .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
            .SelectTry(column => CreateAppUser(column, csvHeaders))
            .OnCaughtException(ex =>
            {
                logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                return null;
            })
            .Where(x => x != null)
            .ToList();

        return users;
    }

    private AppUser CreateAppUser(IReadOnlyList<string> column, string[] headers)
    {
        if (column.Count != headers.Length)
        {
            throw new Exception($"column count '{column.Count}' not the same as headers count'{headers.Length}'");
        }

        var user = new AppUser
        {
            Email = column[Array.IndexOf(headers, "email")].Trim('"').Trim(),
            Id = Guid.NewGuid().ToString(),
            UserName = column[Array.IndexOf(headers, "username")].Trim('"').Trim(),
            DisplayName = column[Array.IndexOf(headers, "displayname")].Trim('"').Trim(),
            PasswordHash = column[Array.IndexOf(headers, "password")].Trim('"').Trim(), // Note: This is the password
        };
        //user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);

        return user;
    }

    private IEnumerable<AppUser> GetDefaultUsers()
    {
        var adminUser = new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            DisplayName = "Admin",
            UserName = "admin",
            Email = "admin@test.com"
        };
        //adminUser.PasswordHash = _passwordHasher.HashPassword(adminUser, "P@ssword1");

        return new List<AppUser>
        {
            adminUser
        };
    }

    private static string[] GetHeaders(IReadOnlyCollection<string> requiredHeaders, string csvFile)
    {
        var csvHeaders = File.ReadLines(csvFile).First().ToLowerInvariant().Split(',');

        if (csvHeaders.Length != requiredHeaders.Count)
        {
            throw new Exception(
                $"requiredHeader count '{requiredHeaders.Count}' is different then read header '{csvHeaders.Length}'");
        }

        foreach (var requiredHeader in requiredHeaders)
        {
            if (!csvHeaders.Contains(requiredHeader))
            {
                throw new Exception($"does not contain required header '{requiredHeader}'");
            }
        }

        return csvHeaders;
    }
}