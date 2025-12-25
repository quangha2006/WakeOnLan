using Microsoft.Extensions.Options;
using WakeOnLan.WebApp.Models;

namespace WakeOnLan.WebApp.Services;

public sealed class ComputerCatalog : IComputerCatalog
{
    private readonly IOptionsMonitor<WakeOnLanOptions> _options;

    public ComputerCatalog(IOptionsMonitor<WakeOnLanOptions> options)
    {
        _options = options;
    }

    public IReadOnlyDictionary<string, ComputerOptions> GetComputers(string accountName)
    {
        var accounts = _options.CurrentValue.Accounts;
        if (!accounts.TryGetValue(accountName, out var acc))
            return new Dictionary<string, ComputerOptions>();

        return acc.ComputerList;
    }

    public bool TryGetComputer(string accountName, string computerName, out ComputerOptions computer)
    {
        computer = default!;
        var accounts = _options.CurrentValue.Accounts;
        if (!accounts.TryGetValue(accountName, out var acc))
            return false;

        return acc.ComputerList.TryGetValue(computerName, out computer!);
    }
}
