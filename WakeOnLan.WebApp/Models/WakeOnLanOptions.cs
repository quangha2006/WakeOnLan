namespace WakeOnLan.WebApp.Models;

public sealed class WakeOnLanOptions
{
    public Dictionary<string, AccountOptions> Accounts { get; init; } = new();
}

public sealed class AccountOptions
{
    public string Password { get; init; } = string.Empty;

    public Dictionary<string, ComputerOptions> ComputerList { get; init; } = new();
}

public sealed class ComputerOptions
{
    public string MacAddress { get; init; } = string.Empty;
    public string? IpBroadcast { get; init; }
    public string? IpAddress { get; init; }
    public string? SubNetMask { get; init; }
}
