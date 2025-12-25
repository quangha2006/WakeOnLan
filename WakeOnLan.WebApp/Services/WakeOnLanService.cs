using WakeOnLan.Core;
using WakeOnLan.Core.Utils;
using WakeOnLan.WebApp.Models;

namespace WakeOnLan.WebApp.Services;

public sealed class WakeOnLanService : IWakeOnLanService
{
    public async Task WakeAsync(ComputerOptions computer, CancellationToken ct)
    {
        string ip = "";
        string subnet = "";

        var mac = computer.MacAddress?.Trim();

        if (string.IsNullOrWhiteSpace(mac))
            throw new ArgumentException("MacAddress is missing.");

        var ipBroadcast = string.IsNullOrWhiteSpace(computer.IpBroadcast)
            ? null
            : computer.IpBroadcast.Trim();

        if (ipBroadcast == null)
        {
            if (ip == "" || subnet == "")
            {
                NetUtils.TryGetCurrentIP(ref ip, ref subnet);
            }
            ipBroadcast = NetUtils.GetIPBroadcast(ip, subnet);
        }

        await WakeOnLanClient.SendMagicPacketAsync(
            macAddress: mac,
            ipBroadcast: ipBroadcast,
            port: 9,
            cancellationToken: ct
        );
    }
}
