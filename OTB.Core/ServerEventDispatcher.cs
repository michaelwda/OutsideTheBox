using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;
using OTB.Core.Hook;

namespace OTB.Core
{
    public class ServerEventDispatcher
    {
        private readonly ServerConnectionManager _manager;

        public ServerEventDispatcher(ServerConnectionManager manager)
        {
            _manager = manager;
        }

        public void MoveScreenRight()
        {
            if (!_manager.IsConnected())
                return;
            _manager.HubConnection.InvokeAsync("MoveScreenRight").ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {
                    Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                }

            });
        }
        public void MoveScreenLeft()
        {
            if (!_manager.IsConnected())
                return;
            _manager.HubConnection.InvokeAsync("MoveScreenLeft").ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {
                    Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                }

            });
        }

        public void Clipboard(string value)
        {
            if (!_manager.IsConnected())
                return;
            _manager.HubConnection.InvokeAsync("Clipboard", value).ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {
                    Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                }

            });
        }

        public void MouseWheel(int deltaX, int deltaY)
        {
            if (!_manager.IsConnected())
                return;
            _manager.HubConnection.InvokeAsync("MouseWheel", deltaX, deltaY).ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {
                    Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                }
                 
            });
        }

        public void MouseDown(MouseButton button)
        {
             if (!_manager.IsConnected())
                   return;
             _manager.HubConnection.InvokeAsync("MouseDown", button).ContinueWith(task1 => {
                 if (task1.IsFaulted)
                 {
                     Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                 }
             });
        }

        public void MouseUp(MouseButton button)
        {
            if (!_manager.IsConnected())
                return;
            _manager.HubConnection.InvokeAsync("MouseUp", button).ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {
                    Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                }
            });
        }

        public void MouseMove(double virtualX, double virtualY)
        {
            if (!_manager.IsConnected())
                return;
            _manager.HubConnection.InvokeAsync("MouseMove", virtualX, virtualY).ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {
                    Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                }
            });
        }

        public void KeyDown(Key key)
        {
            if (!_manager.IsConnected())
                return;
            _manager.HubConnection.InvokeAsync("KeyDown", key).ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {
                    Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                }
            });
        }

        public void KeyUp(Key key)
        {
            if (!_manager.IsConnected())
                return;
            _manager.HubConnection.InvokeAsync("KeyUp", key).ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {
                    Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                }
            });
        }

        public void ClientCheckin(string clientName, List<VirtualScreen> screens)
        {
            //check in this client
            if (!_manager.IsConnected())
                return;
            _manager.HubConnection.InvokeAsync<string>("ClientCheckin", clientName, screens).ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {
                    Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                }
            }).Wait();
        }
    }
}