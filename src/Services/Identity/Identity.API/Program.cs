using Identity.API.Data;
using Identity.API.Extensions;
using Identity.API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalizationServices<AppUser, string>(builder.Configuration);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    var seedLog = services.GetRequiredService<ILogger<ApplicationDbContextSeed>>();
    await context.Database.MigrateAsync();
    await new ApplicationDbContextSeed().SeedAsync(app, seedLog);
}
catch (Exception e)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(e, "An error occurred during migration or seed");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.MapControllers();

app.Run();
