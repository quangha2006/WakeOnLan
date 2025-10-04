using System.Net;
using System.Net.Sockets;
using WakeOnLan.Core.Utils;

namespace WakeOnLan.Core
{
    public static class WakeOnLanClient
    {
        public static async Task<string> SendMagicPacketAsync(
            string macAddress,
            string ipBroadcast = "255.255.255.255",
            int port = 9,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(macAddress))
                throw new ArgumentException("MAC address is empty.", nameof(macAddress));

            var macBytes = NetUtils.ParseMac(macAddress);

            //buffer to be send, this is structure of magic packet
            byte[] magicPacket = new byte[6 + 16 * 6];  // 6 bytes 0xFF + 16 MAC

            //first 6 bytes should be 0xFF
            for (int i = 0; i < 6; i++)
            {
                magicPacket[i] = 0xFF;
            }

            //now repeate MAC 16 times
            for (int i = 0; i < 16; i++)
            {
                Buffer.BlockCopy(macBytes, 0, magicPacket, 6 + i * 6, 6);
            }

            var client = new WakeOnLanUdpClient();

            client.EnableBroadcast = true;

            client.Connect(IPAddress.Parse(ipBroadcast), port);

            client.SetClientToBrodcastMode();

            //now send wake up packet
            int reterned_value = await client.SendAsync(magicPacket, magicPacket.Length);

            if (reterned_value <= 0)
            {
                throw new Exception($"Wake-on-LAN send packet failed: {ipBroadcast}, MAC:{macAddress}");
            }

            return $"Wake-on-LAN packet sent to IP: {ipBroadcast}, MAC:{macAddress}";
        }
    }
    public class WakeOnLanUdpClient : UdpClient
    {
        //this is needed to send broadcast packet
        public void SetClientToBrodcastMode()
        {
            if (Active)
            {
                Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 0);
            }
        }
    }
}
