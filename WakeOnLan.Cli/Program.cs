using WakeOnLan.Core;
using WakeOnLan.Core.Utils;

namespace WakeOnLan
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0 || (args.Length >= 1 && args[0] is "-h" or "--help"))
            {
                PrintUsage();
                Environment.Exit(0);
            }
            await ProcessCommandLine(args);
        }
        public static async Task ProcessCommandLine(string[] args)
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
                        case 'I':
                        case 'i':
                            if (args[i].Length == 2)
                            {
                                ip = args[++i];
                                if (!NetUtils.ValidateIP(ip))
                                {
                                    Console.WriteLine("Invalid IP address: {0}", ip);
                                    ExitWithError();
                                }
                            }
                            else if (args[i].Length == 3 && (args[i][2] == 'b' || args[i][2] == 'B'))
                            {
                                ipBroadcast = args[++i];
                                if (!NetUtils.ValidateIP(ipBroadcast))
                                {
                                    Console.WriteLine("Invalid IP BroadCast: {0}", ipBroadcast);
                                    ExitWithError();
                                }
                            }
                            break;
                        case 'S':
                        case 's':
                            subnet = args[++i];
                            if (!NetUtils.ValidateIP(subnet))
                            {
                                Console.WriteLine("Invalid SubNet Mask: {0}", subnet);
                                ExitWithError();
                            }
                            break;
                        case 'M':
                        case 'm':
                            mac = args[++i];
                            if (!NetUtils.TryParseMac(mac, out byte[] bytes))
                            {
                                Console.WriteLine($"1 Invalid MAC address: {mac}, Accept: AA:BB:CC:DD:EE:FF | AA-BB-CC-DD-EE-FF | AABBCCDDEEFF");
                                ExitWithError();
                            }
                            break;
                        default:
                            Console.WriteLine("Unknow Option: {0}", args[i]);
                            PrintUsage();
                            ExitWithError();
                            break;
                    }
                }
            }

            if (args.Length == 0 || mac == "")
            {
                PrintUsage();
                ExitWithError();
            }

            if (ipBroadcast == "")
            {
                if (ip == "" || subnet == "")
                {
                    NetUtils.TryGetCurrentIP(ref ip, ref subnet);
                }
                ipBroadcast = NetUtils.GetIPBroadcast(ip, subnet);
            }

            //Perform sent magic packet!
            try
            {
                await WakeOnLanClient.SendMagicPacketAsync(mac, ipBroadcast, 9);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ExitWithError();
            }
            Environment.Exit(0);
        }
        private static void PrintUsage()
        {
            Console.WriteLine();
            Console.WriteLine("Usage: WakeOnLan <option>\r\n");
            Console.WriteLine(" -I  <ip>             IP of destination computer.");
            Console.WriteLine(" -S  <SubNet>         Subnet Mask.");
            Console.WriteLine(" -IB <IP Broadcast>   IP Broadcast of destination computer.");
            Console.WriteLine(" -M  <mac Address>    Mac Address of destination computer.\r\n");
            Console.WriteLine("Note: If you have no input ip and subnet mask (or IP Broadcast), The IP Broadcast will retrieve from your default network");
        }
        static public void ExitWithError()
        {
            Environment.Exit(-1);
        }
    }
}