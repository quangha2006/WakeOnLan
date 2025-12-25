using Microsoft.Extensions.Options;
using WakeOnLan.WebApp.Models;

namespace WakeOnLan.WebApp.Services;

public sealed class AccountAuthService : IAccountAuthService
{
    private readonly IOptionsMonitor<WakeOnLanOptions> _options;

    public AccountAuthService(IOptionsMonitor<WakeOnLanOptions> options)
    {
        _options = options;
    }

    public bool Validate(string accountName, string password)
    {
        if (string.IsNullOrWhiteSpace(accountName) || string.IsNullOrWhiteSpace(password))
            return false;

        var accounts = _options.CurrentValue.Accounts;
        if (!accounts.TryGetValue(accountName, out var acc))
        {
            Console.WriteLine("Invalid account: " + accountName);
            return false;
        }
        if (string.Equals(acc.Password, password, StringComparison.Ordinal))
        {
            return true;
        }
        Console.WriteLine("Wrong passwod: " + password);
        return false;
    }
}
