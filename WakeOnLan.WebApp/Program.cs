using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WakeOnLan.WebApp.Models;
using WakeOnLan.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.Configure<WakeOnLanOptions>(
    builder.Configuration.GetSection("WakeOnLanConfig"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/login";
        options.Cookie.Name = "WakeOnLan.Auth";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(12);
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAccountAuthService, AccountAuthService>();
builder.Services.AddScoped<IComputerCatalog, ComputerCatalog>();
builder.Services.AddScoped<IWakeOnLanService, WakeOnLanService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    ctx.Response.Redirect("/login");
}).RequireAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
