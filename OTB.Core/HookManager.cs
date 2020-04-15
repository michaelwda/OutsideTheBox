using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using System.Runtime.InteropServices;
using OTB.Core.Hook;
using OTB.Core.Hook.Platform.OSX;
using OTB.Core.Hook.Platform.OSX.Native.CG;
using OTB.Core.Hook.Platform.Windows;

namespace OTB.Core
{
    //I should turn this into a singleton...
    public class HookManager : IDisposable
    {
        private readonly ServerEventDispatcher _dispatcher;
        private readonly VirtualScreenManager _screen;
        public readonly IGlobalHook Hook;
        
        public HookManager(ServerEventDispatcher dispatcher, VirtualScreenManager screen)
        {
            _dispatcher = dispatcher;
            _screen = screen;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Hook = new WindowsGlobalHook();
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Hook = new OsxGlobalHook();
            }
        }
        //On some platforms this does some setup work needed - like settings the process DPI aware
        public void Init()
        {
            Hook.Init();
            
        }
        public void Start()
        {
            Hook.Start();
            Hook.MouseMove += _globalHook_MouseMove;
            Hook.MouseDown += _globalHook_MouseDown;
            Hook.MouseWheel += _globalHook_MouseWheel;
            Hook.MouseUp += _globalHook_MouseUp;
            Hook.KeyDown += _globalHook_KeyDown;
            Hook.KeyUp += _globalHook_KeyUp;
            Hook.Clipboard += _globalHook_Clipboard;

            var c = Hook.GetMousePos();                     
            
            ClientState._virtualX = c.X;
            ClientState._virtualY = c.Y;
            ClientState._lastPositionX = c.X;
            ClientState._lastPositionY = c.Y;
        }

        public void Stop()
        {
            //TODO: Implement unhook events here.
            Hook.Stop();
        }
        
        
        private void _globalHook_Clipboard(object sender, ClipboardChangedEventArgs e)
        {
            if (!ClientState.CurrentClientFocused)
            {
                //Console.WriteLine("Trying to set the clipboard when we're not the current client...");
                return;
                
            }
            
            //if our application has received a clipboard push from the server, this event still fires, so bail out if we are currently syncing the clipboard.
            //don't process a hook event within 2 seconds 
            if (ShouldHookBailKeyboard())
                return;

            ClientState.LastHookEvent_Keyboard = DateTime.Now;
            //Console.WriteLine("Sending clipboard to server");

            _dispatcher.Clipboard(e.Value);
            
			
        }
        private void _globalHook_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!ClientState.CurrentClientFocused)
                e.Handled = true;

            //don't process a hook event within 2 seconds of receiving network events. 
            if (ShouldHookBailMouse())
                return;
            ClientState.LastHookEvent_Mouse = DateTime.Now;
            _dispatcher.MouseWheel(e.DeltaX, e.DeltaY);
			
        }
        private void _globalHook_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!ClientState.CurrentClientFocused)
                e.Handled = true;
            //don't process a hook event within 2 seconds 
            if (ShouldHookBailMouse())
                return;
            ClientState.LastHookEvent_Mouse = DateTime.Now;
            _dispatcher.MouseDown(e.Button);
			
            
        }
        private void _globalHook_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!ClientState.CurrentClientFocused)
                e.Handled = true;
            //don't process a hook event within 2 seconds 
            if (ShouldHookBailMouse())
                return;
            ClientState.LastHookEvent_Mouse = DateTime.Now;
            _dispatcher.MouseUp(e.Button);
			

            

        }
        private void _globalHook_MouseMove(object sender, MouseMoveEventArgs e)
        {

           
            
            //don't process a hook event within 2 seconds of a server event
            if (ShouldHookBailMouse())
                return;
            
            
            ClientState.LastHookEvent_Mouse = DateTime.Now;

            var result = _screen.UpdateVirtualMouseCoordinates(e);
            if (result == CoordinateCalculationResult.Valid)
            {
                
                var presult=_screen.ProcessVirtualCoordinatesUpdate();
                if (presult.MoveMouse)
                {
                    //Console.WriteLine("Moving mouse to a position");
                    if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        //OSX won't let us cancel movement events, so we have to warp and set a flag. 
                        ((OsxGlobalHook)Hook).WarpMouse(ClientState._lastPositionX, ClientState._lastPositionY);
                    }
                    else
                    {
                        Hook.SetMousePos(ClientState._lastPositionX, ClientState._lastPositionY);    
                    }
                    
                    
                }

                if (presult.HandleEvent) //we are receiving local input, but mouse is on a virtual monitor. We need to lock the cursor in a position.
                {
                    

                    e.Handled = true; //windows obeys this
                    if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        //OSX needs a warp event to correctly position because we can't cancel the event propogation.
                        ((OsxGlobalHook)Hook).WarpMouse(ClientState._lastPositionX, ClientState._lastPositionY);
                    
                    }
                    
                    
                }
                
                //send over the net
                _dispatcher.MouseMove(ClientState._virtualX, ClientState._virtualY);
            }
            else
            {
                //if we're the current client, i'm letting this through to enable smoother scrolling along edges.

                //if we're not the current client, handle it.
                if (!ClientState.CurrentClientFocused)
                {
                    e.Handled = true;
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        //Again, OSX needs the warp here.  
                        ((OsxGlobalHook)Hook).WarpMouse(ClientState._lastPositionX, ClientState._lastPositionY);
                    }
                     
                }
            }
           


        }
        private void _globalHook_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            //TODO: remove this
            //I put this here for when I wanted a fail-safe bailout :)
            if(e.Key==Key.Tilde)
                Environment.Exit(-1);
            if (!ClientState.CurrentClientFocused)
                e.Handled = true;
            
            if (ShouldHookBailKeyboard())
                return;

            ClientState.LastHookEvent_Keyboard = DateTime.Now;
            
            
            _dispatcher.KeyDown(e.Key);
			

         
        }
        private void _globalHook_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (!ClientState.CurrentClientFocused)
                e.Handled = true;

            if (ShouldHookBailKeyboard())
                return;

            ClientState.LastHookEvent_Keyboard = DateTime.Now;
            _dispatcher.KeyUp(e.Key);
			
        }
        private bool ShouldHookBailKeyboard()
        {
            if ((DateTime.Now - ClientState.LastServerEvent_Keyboard).TotalSeconds < 1)
                return true;

            return false;
        }
        private bool ShouldHookBailMouse()
        {
            if ((DateTime.Now - ClientState.LastServerEvent_Mouse).TotalSeconds < 1)
                return true;

            return false;
        }
        
        public void Dispose()
        {
            
            Hook?.Dispose();
        }

        public List<Display> GetDisplays()
        {
            return Hook.GetDisplays();
            
        }
    }
}