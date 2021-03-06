﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Compression;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using OTB.Core.Hook;

namespace OTB.Core
{
    public class ServerEventReceiver
    {
        private readonly ServerConnectionManager _manager;
        private readonly HookManager _hook;
        private readonly VirtualScreenManager _screen;

        public ServerEventReceiver(ServerConnectionManager manager, HookManager hookManager, VirtualScreenManager screen)
        {
            _manager = manager;
            _hook = hookManager;
            _screen = screen;

            _manager.HubConnection.On<double, double>("MouseMove", OnMouseMoveFromServer);
            _manager.HubConnection.On<int, int>("MouseWheel", OnMouseWheelFromServer);
            _manager.HubConnection.On<MouseButton>("MouseDown", OnMouseDownFromServer);
            _manager.HubConnection.On<MouseButton>("MouseUp", OnMouseUpFromServer);
            _manager.HubConnection.On<Key>("KeyUp", OnKeyUpFromServer);
            _manager.HubConnection.On<Key>("KeyDown", OnKeyDownFromServer);
            _manager.HubConnection.On<string>("Clipboard", OnClipboardFromServer);
            _manager.HubConnection.On<List<VirtualScreen>>("ScreenConfigUpdate", OnScreenConfig);
        }
        
         
        private bool ShouldServerBailKeyboard()
        {
            if ((DateTime.Now - ClientState.LastHookEvent_Keyboard).TotalSeconds < 2)
            {
                ClientState.Logger.LogDebug("Bailing keyboard input");
                return true;
            }
            return false;
        }
        private bool ShouldServerBailMouse()
        {
            if ((DateTime.Now - ClientState.LastHookEvent_Mouse).TotalSeconds < 2)
            {
                ClientState.Logger.LogDebug("Bailing mouse input");
                return true;
            }

            return false;
        }
            
        private void OnClipboardFromServer(string value)
        {
            //Console.WriteLine("Received clipboard from server");
            ClientState.Logger.LogDebug("OnClipboardFromServer");
            //i received a hook event for a copy from another client within 2 seconds of pressing my own
            //clipboard. 
            //historically this has been happening by a global hook reading my event taps and replaying back over the network
            //in a feedback loop. This should be solved, but i'm leaving this code here as an extra check.
            if (ShouldServerBailKeyboard())
                return;

            ClientState.LastServerEvent_Keyboard = DateTime.Now;
  
            _hook.Hook.SetClipboard(value);
          
        }
        private void OnMouseMoveFromServer(double x, double y)
        {
            ClientState.Logger.LogDebug("OnMouseMoveFromServer");

            if (ShouldServerBailMouse())
                return;
       
            ClientState.LastServerEvent_Mouse = DateTime.Now;
            
           
            ClientState._virtualX = x;
            ClientState._virtualY = y;

            //send this movement to our virtual screen manager for processing
            var result=_screen.ProcessVirtualCoordinatesUpdate(true);
            if (result.MoveMouse)
            {
                _hook.Hook.SetMousePos(ClientState._lastPositionX, ClientState._lastPositionY);
            }

            if (result.HandleEvent)
            {
                
            }
            
        }
        private void OnMouseWheelFromServer(int dx, int dy)
        {
            if (ShouldServerBailMouse())
                return;

            ClientState.LastServerEvent_Mouse = DateTime.Now;
            //Console.WriteLine("Received mouse wheel from server");
            if (ClientState.CurrentClientFocused)
                _hook.Hook.SendMouseWheel(dx,dy);

        }
        private void OnMouseDownFromServer(MouseButton button)
        {
            if (ShouldServerBailMouse())
                return;

            ClientState.LastServerEvent_Mouse = DateTime.Now;
            //Console.WriteLine("Received mouse down from server: " + button.ToString());
            if(ClientState.CurrentClientFocused)
                _hook.Hook.SendMouseDown(button);

        }
        private void OnMouseUpFromServer(MouseButton button)
        {
            if (ShouldServerBailMouse())
                return;

            ClientState.LastServerEvent_Mouse = DateTime.Now;
            //Console.WriteLine("Received mouse up from server: " + button.ToString());
            if (ClientState.CurrentClientFocused)
                _hook.Hook.SendMouseUp(button);
        }
        private void OnKeyDownFromServer(Key key)
        {
            ClientState.Logger.LogDebug("OnKeyDownFromServer" + key);
            if (ShouldServerBailKeyboard())
                return;

            ClientState.LastServerEvent_Keyboard = DateTime.Now;
            if (ClientState.CurrentClientFocused)
                _hook.Hook.SendKeyDown(key);
        }
        private void OnKeyUpFromServer(Key key)
        {
            ClientState.Logger.LogDebug("OnKeyUpFromServer" + key);
            if (ShouldServerBailKeyboard())
                return;

            ClientState.LastServerEvent_Keyboard = DateTime.Now;
            if (ClientState.CurrentClientFocused)
                _hook.Hook.SendKeyUp(key);
        }
        private void OnScreenConfig(List<VirtualScreen> screens)
        {

            ClientState.ScreenConfiguration.Screens = new ConcurrentDictionary<string, List<VirtualScreen>>();
        
            foreach (var screen in screens)
            {                
                //Console.WriteLine("Screen:"+screen.X+","+screen.Y + ", LocalX:"+screen.LocalX + ", "+screen.LocalY + " , Width:"+screen.Width + " , height:"+screen.Height+", client: "+ screen.Client);
                if(!ClientState.ScreenConfiguration.Screens.ContainsKey(screen.Client))
                {
                    ClientState.ScreenConfiguration.Screens.TryAdd(screen.Client, new List<VirtualScreen>());
                }
                ClientState.ScreenConfiguration.Screens[screen.Client].Add(screen);
                
            }

            if (ClientState.ScreenConfiguration.ValidVirtualCoordinate(ClientState._virtualX, ClientState._virtualY) !=
                null) return;
            //coordinates are invalid, grab a screen
            var s =  ClientState.ScreenConfiguration.GetFurthestLeft();
            ClientState._virtualX = s.X;
            ClientState._virtualY = s.Y;
            if (s.Client != ClientState.ClientName) return;
            //set this local client to have 0,0 coords. then update the other clients with the new virtual position.
            ClientState._lastPositionX = 0;
            ClientState._lastPositionY = 0;
            _hook.Hook.SetMousePos(0, 0);
            //TODO: does this still work without explicitly updating the server again?
//                    _hubConnection.InvokeAsync("MouseMove", _virtualX, _virtualY).ContinueWith(task1 =>
//                    {
//                        if (task1.IsFaulted)
//                        {
//                            Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
//                        }
//                        else
//                        {
//
//                        }
//                    });

        }  
        
    }
}