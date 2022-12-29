using Identity.API.Data;
using Identity.API.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCorsServices(builder.Configuration);

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

await app.RunAsync();