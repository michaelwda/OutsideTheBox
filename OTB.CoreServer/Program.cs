using System;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OTB.Core;
using OTB.Core.Hook;
using System.ComponentModel;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net.NetworkInformation;
using System.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OTB.CoreServer
{
	public class Program
    {
        private static BackgroundWorker UdpDiscoveryBroadcaster;
        private static bool _running;
        public static void Main(string[] args)
        {
            UdpDiscoveryBroadcaster = new BackgroundWorker();
            UdpDiscoveryBroadcaster.DoWork += UdpDiscoveryBroadcaster_DoWork;

            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseSetting(WebHostDefaults.PreventHostingStartupKey, "true")
                 .ConfigureLogging(logging =>
                 {
                     logging.SetMinimumLevel(LogLevel.Information);
                     logging.AddConsole();
                   
                 })
                .UseKestrel(options =>
                {
                    // Default port
                    options.ListenAnyIP(8088);
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            
            UdpDiscoveryBroadcaster.RunWorkerAsync();
            host.Run();
           
        }

        private static void UdpDiscoveryBroadcaster_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface Interface in Interfaces)
                {
                    if (Interface.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                    if (Interface.OperationalStatus != OperationalStatus.Up) continue;

                    //Console.WriteLine(Interface.Description);
                    UnicastIPAddressInformationCollection UnicastIPInfoCol = Interface.GetIPProperties().UnicastAddresses;
                    foreach (UnicastIPAddressInformation UnicatIPInfo in UnicastIPInfoCol)
                    {
                        //only using ipv4.
                        //TODO: ipv6??
                        if (UnicatIPInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            var addressInt = BitConverter.ToInt32(UnicatIPInfo.Address.GetAddressBytes(), 0);
                            var maskInt = BitConverter.ToInt32(UnicatIPInfo.IPv4Mask.GetAddressBytes(), 0);
                            var broadcastInt = addressInt | ~maskInt;
                            var broadcast = new IPAddress(BitConverter.GetBytes(broadcastInt));

                            //send a broadcast
                            IPEndPoint ep = new IPEndPoint(broadcast, 11000);
                            byte[] sendbuf = UnicatIPInfo.Address.GetAddressBytes();//Encoding.ASCII.GetBytes("OTB");
                            s.SendTo(sendbuf, ep);
                        }
                    }
                }
                Thread.Sleep(5000);
            }
            
        }

        public static IPAddress GetBroadcastAddress(UnicastIPAddressInformation unicastAddress)
        {
            return GetBroadcastAddress(unicastAddress.Address, unicastAddress.IPv4Mask);
        }

        public static IPAddress GetBroadcastAddress(IPAddress address, IPAddress mask)
        {
            uint ipAddress = BitConverter.ToUInt32(address.GetAddressBytes(), 0);
            uint ipMaskV4 = BitConverter.ToUInt32(mask.GetAddressBytes(), 0);
            uint broadCastIpAddress = ipAddress | ~ipMaskV4;

            return new IPAddress(BitConverter.GetBytes(broadCastIpAddress));
        }
    }
}
