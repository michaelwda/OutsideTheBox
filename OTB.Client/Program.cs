using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using OTB.Core;
using OTB.Core.Hook;
using OTB.Core.Hook.Platform.OSX;
using OTB.Core.Hook.Platform.OSX.Native;

namespace OTB.Client
{

    class Program
    {
        private const int discoveryPort=11000;
        private static OTBClient client;
        static void Main(string[] args)
        {
            //find server via UDP broadcast
            bool done = false;
            string otpip = "";
            UdpClient listener = new UdpClient(discoveryPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, discoveryPort);
            try
            {
                while (!done)
                {
                    Console.WriteLine("Waiting for server");
                    byte[] bytes = listener.Receive(ref groupEP);

                    var ipAddress = new IPAddress(bytes);
                    otpip = "http://" + ipAddress + ":8088/OTB";
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

            OTBClient client = new OTBClient(otpip);
            if (client.Start())
            {
                client.RunMessageLoop();
            }
         
        }

        private static void ServerDiscovery_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }

    
}
