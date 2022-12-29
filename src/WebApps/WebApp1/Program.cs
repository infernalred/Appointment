using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, opt =>
    {
        opt.Authority = "https://localhost:5001";
        opt.ClientId = "client_id_mvc1";
        opt.ClientSecret = "client_secret_mvc";
        opt.SaveTokens = true;
        opt.ResponseType = "code";
        opt.RequireHttpsMetadata = false;
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();