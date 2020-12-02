using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace WOL
{
    class Program
    {
        static void Main(string[] args)
        {
            // http://jodies.de/ipcalc
            if (args.Length == 2)
            {
                string ip = args[0];
                string mac = string.Join("", args[1].Split('-'));
                WakeFunction(ip, mac, 9);
            }
            else
                Console.WriteLine("Usage: WOL.exe [ip Broadcast] [MacAddress]");
        }

        public class WOLClass : UdpClient
        {
            public WOLClass() : base()
            { }
            //this is needed to send broadcast packet
            public void SetClientToBrodcastMode()
            {
                if (this.Active)
                    this.Client.SetSocketOption(SocketOptionLevel.Socket,
                                              SocketOptionName.Broadcast, 0);
            }
        }
        static private void WakeFunction(string ip, string MAC_ADDRESS, int port)
        {
            Console.WriteLine("Wake-on-LAN packet sent: ip:{0} mac:{1} port:{2}", ip, MAC_ADDRESS, port);
            WOLClass client = new WOLClass();

            client.Connect(IPAddress.Parse(ip), port);
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
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
    }
}
