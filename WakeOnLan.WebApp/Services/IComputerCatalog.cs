using WakeOnLan.WebApp.Models;

namespace WakeOnLan.WebApp.Services;

public interface IComputerCatalog
{
    IReadOnlyDictionary<string, ComputerOptions> GetComputers(string accountName);
    bool TryGetComputer(string accountName, string computerName, out ComputerOptions computer);
}
