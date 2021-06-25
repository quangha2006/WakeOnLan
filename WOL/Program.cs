using System;
using System.Linq;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WOL
{
    class Program
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(int dwProcessId);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [STAThread]
        static void Main(string[] args)
        {
            if (!IsRanFromConsole(args)) //Winform mode
            {
                var handle = GetConsoleWindow();

                // Hide console
                ShowWindow(handle, SW_HIDE);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else // Console mode
            {
                //AttachConsole(-1);
                //AllocConsole();
                ProcessCommandLine(args);
                //FreeConsole();
            }
            
        }
        public static void ProcessCommandLine(string[] args)
        {
            string ip = "";
            string subnet = "";
            string mac = "";
            string ipBroadcast = "";

            //Parse arguments
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i][0] == '-' && args[i].Length > 1 && args.Length > i + 1)
                {
                    switch (args[i][1])
                    {
                        case 'i':
                            if (args[i].Length == 2)
                            {
                                ip = args[++i];
                                if (!ValidateIP(ip))
                                {
                                    Console.WriteLine("Invalid IP address: {0}", ip);
                                    return;
                                }
                            }
                            else if (args[i].Length == 3 && args[i][2] == 'b')
                            {
                                ipBroadcast = args[++i];
                                if (!ValidateIP(ipBroadcast))
                                {
                                    Console.WriteLine("Invalid IP BroadCast: {0}", ipBroadcast);
                                    return;
                                }
                            }
                            break;
                        case 's':
                            subnet = args[++i];
                            if (!ValidateIP(subnet))
                            {
                                Console.WriteLine("Invalid SubNet Mask: {0}", subnet);
                                return;
                            }
                            break;
                        case 'm':
                            mac = args[++i];
                            mac = mac.Replace(':', '-');
                            if (!ValidateMac(mac))
                            {
                                Console.WriteLine("Invalid Mac Address: {0}", mac);
                                return;
                            }
                            break;
                        default:
                            Console.WriteLine("Unknow Option: {0}", args[i]);
                            PrintUsage();
                            return;
                    }
                }
            }

            if (args.Length == 0 || mac == "")
            {
                PrintUsage();
                return;
            }

            if (ipBroadcast == "")
            {
                if (ip == "" || subnet == "")
                {
                    GetCurrentIP(ref ip, ref subnet);
                }
                ipBroadcast = GetIPBroadcast(ip, subnet);
            }

            //Perform sent magic packet!
            WakeFunction(ipBroadcast, mac, 9);
        }

        static private void WakeFunction(string ip, string mac, int port)
        {
            Console.WriteLine("Wake-on-LAN packet sent to IP: {0}, MAC:{1}", ip, mac);

            string MAC_ADDRESS = string.Join("", mac.Split('-'));
            
            WOLClass client = new WOLClass();

            client.EnableBroadcast = true;

            client.Connect(IPAddress.Parse(ip), port);

            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send, this is structure of magic packet
            byte[] bytes = new byte[1024];   // more than enough :-)
                                             //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] =
                        byte.Parse(MAC_ADDRESS.Substring(i, 2),
                        NumberStyles.HexNumber);
                    i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
        }
        static public void PrintUsage()
        {
            Console.WriteLine();
            Console.WriteLine("Usage: WakeOnLan <option>\r\n");
            Console.WriteLine(" -i <ip>             IP of destination computer.");
            Console.WriteLine(" -s <SubNet>         Subnet Mask.");
            Console.WriteLine(" -ib <IP Broadcast>  IP Broadcast of destination computer.");
            Console.WriteLine(" -m <mac Address>    Mac Address of destination computer.\r\n");
            Console.WriteLine("Note: If you have no input ip and subnet mask (or IP Broadcast), The IP Broadcast will retrieve from your default network");
        }
        static bool GetCurrentIP(ref string ipout, ref string subnetout)
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
                    if (address.IsTransient)
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
        static string GetIPBroadcast(string ipAddress, string subnetMask)
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

        static public NetworkInterface GetDefaultInterface()
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
        static public bool ValidateIP(string ip)
        {
            var ipArray = ip.Split('.');

            if (ipArray.Length != 4)
                return false;

            foreach(var scope in ipArray)
            {
                int scopenumber;
                if (!int.TryParse(scope,out scopenumber))
                    return false;

                if (scopenumber < 0 || scopenumber > 255)
                    return false;
            }
            return true;
        }
        static public bool ValidateMac(string mac)
        {
            var macArray = mac.Split('-');

            if (macArray.Length != 6)
                return false;

            foreach (var scope in macArray)
            {
                if (scope.Length != 2)
                    return false;
            }
            return true;
        }
        static public bool IsRanFromConsole(string[] args) // avoid user run from console without any argument, Will print usage.
        {
            if (args.Length > 0)
                return true;

            string[] consoleNames = {"cmd", "bash", "ash", "dash", "ksh", "zsh", "csh","tcsh", "ch", "eshell", "fish", "psh", "pwsh", "rc","sash", "scsh", "powershell", "tcc"};

            string parentProc = Process.GetCurrentProcess().Parent().ProcessName;

            bool isConsole = Array.IndexOf(consoleNames, parentProc) > -1;

            return isConsole;
        }
        
    }
    public class WOLClass : UdpClient
    {
        public WOLClass() : base()
        { }
        //this is needed to send broadcast packet
        public void SetClientToBrodcastMode()
        {
            if (this.Active)
            {
                this.Client.SetSocketOption(SocketOptionLevel.Socket,
                                          SocketOptionName.Broadcast, 0);
            }
        }
    }
    public static class ProcessExtensions
    {
        private static string FindIndexedProcessName(int pid)
        {
            var processName = Process.GetProcessById(pid).ProcessName;
            var processesByName = Process.GetProcessesByName(processName);
            string processIndexdName = null;

            for (var index = 0; index < processesByName.Length; index++)
            {
                processIndexdName = index == 0 ? processName : processName + "#" + index;
                var processId = new PerformanceCounter("Process", "ID Process", processIndexdName);
                if ((int)processId.NextValue() == pid)
                {
                    return processIndexdName;
                }
            }

            return processIndexdName;
        }

        private static Process FindPidFromIndexedProcessName(string indexedProcessName)
        {
            var parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
            return Process.GetProcessById((int)parentId.NextValue());
        }

        public static Process Parent(this Process process)
        {
            return FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
        }
    }
}
