using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace OTB.Core
{
    //todo: I'm not particuarly happy with how this is structured - could likely use improvement
    //but I didn't want to have the hook manager / server code referencing each other.
    //a received event from the server will need to call the hook (like to move the mouse).
    //Instead I seperated out the dispatcher and the receiver so I don't have a cyclical dependency.
    public class ServerConnectionManager : IDisposable
    {
        public HubConnection HubConnection;
        private bool _connected = false;
        public ServerConnectionManager(string serverAddress)
        {
            
            HubConnection = new HubConnectionBuilder()
                //.WithEndPoint(endpoint) //TCP endpoint coming soon. I don't feel like building from dev branch.
                .WithUrl(serverAddress)
                .WithMessagePackProtocol()
                .Build();
            
            HubConnection.Closed+=HubConnectionOnClosed;
          
        }

        private void HubConnectionOnClosed(Exception exception)
        {
            _connected = false;
        }

        public void Start()
        {
            HubConnection.StartAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}",
                        task.Exception.GetBaseException());

                    
                }
                else
                {
                    Console.WriteLine("Connected");
                    _connected = true;

                }
            }).Wait();
        }


        public void Stop()
        {
            _connected = false;
            HubConnection.StopAsync();
            
        }


        public bool IsConnected()
        {
            //checks connectivity. Attempts to reconnect.
            if (_connected)
                return true;
            Start();
            if (_connected)
                return true;
            return false;
        }

        public void Dispose()
        {
            if (HubConnection == null) return;
            HubConnection?.StopAsync();
            HubConnection?.DisposeAsync();
        }
    }
}