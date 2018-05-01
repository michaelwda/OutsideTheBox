using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace OTB.ClientUI
{
    public class ConnectingWindow : Window
    {
        BackgroundWorker _serverDiscovery;
        private const int discoveryPort = 11000;
        public ConnectingWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            _serverDiscovery = new BackgroundWorker();
            _serverDiscovery.DoWork += _serverDiscovery_DoWork;
            _serverDiscovery.RunWorkerCompleted += _serverDiscovery_RunWorkerCompleted;
            _serverDiscovery.RunWorkerAsync();
        }

        private void _serverDiscovery_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            App.client = new Core.OTBClient(e.Result+"");
            MainWindow main = new MainWindow();
            App.client.Start();
            main.Show();
            this.Hide();
        }

        private void _serverDiscovery_DoWork(object sender, DoWorkEventArgs e)
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
                    
                    byte[] bytes = listener.Receive(ref groupEP);
                    
                    var ipAddress = new IPAddress(bytes);
                    otpip = "http://" + ipAddress + ":8088/OTB";
                    done = true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }
            e.Result = otpip;
        }
    }
}
