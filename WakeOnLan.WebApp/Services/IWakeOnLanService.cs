using WakeOnLan.WebApp.Models;

namespace WakeOnLan.WebApp.Services
{
    public interface IWakeOnLanService
    {
        Task WakeAsync(ComputerOptions computer, CancellationToken ct);
    }
}
