using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WakeOnLan.WebApp.Services;

namespace WakeOnLan.WebApp.Pages;

public sealed class LoginModel : PageModel
{
    private readonly IAccountAuthService _auth;

    public LoginModel(IAccountAuthService auth)
    {
        _auth = auth;
    }

    [BindProperty]
    public string AccountName { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string? Error { get; set; }

    public IActionResult OnGet()
    {
        if (User?.Identity?.IsAuthenticated == true)
            return RedirectToPage("/Index");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!_auth.Validate(AccountName, Password))
        {
            Error = "Invalid account or password.";
            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, AccountName)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return RedirectToPage("/Index");
    }
}
