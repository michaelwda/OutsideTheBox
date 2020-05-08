using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OTB.Core;
using OTB.Core.Hook;
using OTB.Core.Hook.Platform.OSX;
using OTB.Core.Hook.Platform.OSX.Native;

namespace OTB.Client
{
    class Program
    {
        private static BackgroundWorker _udpDiscoveryBroadcaster;
        private const int DISCOVERY_PORT=11000;
        static void Main(string[] args)
        {
         
            if (args[0] == "s")
            {
                //server
                StartUDPDiscoveryBroadcast();
                StartServer();
            }
            else
            {
                //client
                var ip = StartUDPDiscoveryListen();
                StartClient(ip);
            }
        }

        
        private static void StartServer()
        {
            var config = new ConfigurationBuilder()
                .Build();
            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseSetting(WebHostDefaults.PreventHostingStartupKey, "true")
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Debug);
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
            host.Run();
        }
        private static void StartClient(string ip)
        {
            var otpip = "http://" + ip + ":8088/OTB";
            OTBClient client = new OTBClient(otpip);
            if (client.Start())
            {
                client.RunMessageLoop();
            }
        }
        private static void StartUDPDiscoveryBroadcast()
        {
            _udpDiscoveryBroadcaster = new BackgroundWorker();
            _udpDiscoveryBroadcaster.DoWork += UdpDiscoveryBroadcaster_DoWork;
            _udpDiscoveryBroadcaster.RunWorkerAsync();
        }
        private static void UdpDiscoveryBroadcaster_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                var s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                    if (networkInterface.OperationalStatus != OperationalStatus.Up) continue;

                    var unicastAddresses = networkInterface.GetIPProperties().UnicastAddresses;
                    foreach (var ip in unicastAddresses)
                    {
                        //TODO: ipv6
                        if (ip.Address.AddressFamily != AddressFamily.InterNetwork) continue;
                        var addressInt = BitConverter.ToInt32(ip.Address.GetAddressBytes(), 0);
                        var maskInt = BitConverter.ToInt32(ip.IPv4Mask.GetAddressBytes(), 0);
                        var broadcastInt = addressInt | ~maskInt;
                        var broadcast = new IPAddress(BitConverter.GetBytes(broadcastInt));

                        //send a broadcast of the local IP
                        var broadcastEndpoint = new IPEndPoint(broadcast, 11000);
                        var sendBuffer = ip.Address.GetAddressBytes();
                        s.SendTo(sendBuffer, broadcastEndpoint);
                    }
                }
                //Broadcast every 5 seconds
                Thread.Sleep(5000);
            }

        }
        private static string StartUDPDiscoveryListen()
        {
            //find server via UDP broadcast
            var done = false;
            var ip = "";
            var listener = new UdpClient(DISCOVERY_PORT);
            var groupEP = new IPEndPoint(IPAddress.Any, DISCOVERY_PORT);
            try
            {
                while (!done)
                {
                    Console.WriteLine("Waiting for server");
                    var bytes = listener.Receive(ref groupEP);
                    ip = new IPAddress(bytes).ToString();
                    done = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }

            return ip;
        }
        
    }

    
}
