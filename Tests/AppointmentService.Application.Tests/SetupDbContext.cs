using AppointmentService.Domain;
using AppointmentService.Persistence;
using AppointmentService.Persistence.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AppointmentService.Application.Tests;

public static class SetupDbContext
{
    public static async Task<DataContext> Generate()
    {
        var options = CreateNewContextOptions();
        var context = new DataContext(options);
        await context.Database.EnsureCreatedAsync();
        CreateUser(context);
        await Seed.SeedData(context);

        await context.SaveChangesAsync();

        return context;
    }

    private static void CreateUser(DataContext context)
    {
        var master1 = new AppUser {Id = "745bf0a5-9e0d-433d-afb4-a15b8e630d19", DisplayName = "Master1", UserName = "master1", Email = "master1@test.com"};
        var master2 = new AppUser {Id = "8b8eb53e-59ab-4a9b-9ae7-2fca51dc7494", DisplayName = "Master2", UserName = "master2", Email = "master2@test.com"};
        var master3 = new AppUser {Id = "5d15e1d8-6fb9-48a8-b941-7faf4815f24c", DisplayName = "Master3", UserName = "master3", Email = "master3@test.com"};
        
        context.Users.Add(master1);
        context.Users.Add(master2);
        context.Users.Add(master3);
        context.SaveChanges();
    }

    private static DbContextOptions<DataContext> CreateNewContextOptions()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite(connection);

        return builder.Options;
    }
}