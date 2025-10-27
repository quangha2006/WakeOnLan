using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace WakeOnLan.Core.Utils
{
    public static class NetUtils
    {
        /// <summary>Parse MAC address to 6 bytes, validate format.</summary>
        public static byte[] ParseMac(string mac)
        {
            if (string.IsNullOrWhiteSpace(mac))
                throw new ArgumentException("2 MAC address is empty.", nameof(mac));

            var cleaned = Regex.Replace(mac.Trim(), "[-:\\s]", "").ToUpperInvariant();
            if (cleaned.Length != 12 || !Regex.IsMatch(cleaned, "^([0-9A-F]{12})$"))
            {
                throw new FormatException($"Invalid MAC address: {mac}, Accept: AA:BB:CC:DD:EE:FF | AA-BB-CC-DD-EE-FF | AABBCCDDEEFF");
            }

            var bytes = new byte[6];
            for (int i = 0; i < 6; i++)
                bytes[i] = Convert.ToByte(cleaned.Substring(i * 2, 2), 16);

            return bytes;
        }
        /// <summary>
        /// TryParse MAC address → 6 bytes.
        /// </summary>
        public static bool TryParseMac(string? mac, out byte[] bytes)
        {
            try
            {
                bytes = ParseMac(mac ?? string.Empty);
                return true;
            }
            catch
            {
                bytes = Array.Empty<byte>();
                return false;
            }
        }
        public static NetworkInterface GetDefaultInterface()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var intf in interfaces)
            {
                if (intf.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }
                if (intf.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                {
                    continue;
                }

                var properties = intf.GetIPProperties();
                if (properties == null)
                {
                    continue;
                }
                var gateways = properties.GatewayAddresses;
                if ((gateways == null) || (gateways.Count == 0))
                {
                    continue;
                }
                var addresses = properties.UnicastAddresses;
                if ((addresses == null) || (addresses.Count == 0))
                {
                    continue;
                }
                return intf;
            }
            return null;
        }

        public static bool ValidateIP(string ip)
        {
            var ipArray = ip.Split('.');

            if (ipArray.Length != 4)
                return false;

            foreach (var scope in ipArray)
            {
                int scopenumber;
                if (!int.TryParse(scope, out scopenumber))
                    return false;

                if (scopenumber < 0 || scopenumber > 255)
                    return false;
            }
            return true;
        }
        public static bool TryGetCurrentIP(ref string ipout, ref string subnetout)
        {
            // Check network
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                return false;
            }

            // Get the IP  
            var defaultNetworkInterface = GetDefaultInterface();
            if (defaultNetworkInterface != null)
            {
                foreach (var address in defaultNetworkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                    {
                        continue;
                    }
                    if (OperatingSystem.IsWindows() && address.IsTransient)
                    {
                        continue;
                    }

                    ipout = address.Address.ToString();
                    subnetout = address.IPv4Mask.ToString();
                    return true;
                }
            }
            return false;
        }
        public static string GetIPBroadcast(string ipAddress, string subnetMask)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            IPAddress subnet = IPAddress.Parse(subnetMask);

            byte[] ipBytes = ip.GetAddressBytes();
            byte[] maskBytes = subnet.GetAddressBytes();

            byte[] startIPBytes = new byte[ipBytes.Length];
            byte[] endIPBytes = new byte[ipBytes.Length];

            // Calculate the bytes of the start and end IP addresses.
            for (int i = 0; i < ipBytes.Length; i++)
            {
                startIPBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
                endIPBytes[i] = (byte)(ipBytes[i] | ~maskBytes[i]);
            }

            // Convert the bytes to IP addresses.
            IPAddress startIP = new IPAddress(startIPBytes);
            IPAddress endIP = new IPAddress(endIPBytes);

            return endIP.ToString();
        }
    }
}
