using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using OTB.Core.Hook;
using OTB.Core.Hook.Platform.OSX;
using OTB.Core.Hook.Platform.OSX.Native;
using OTB.Core.Hook.Platform.OSX.Native.CF;
using OTB.Core.Hook.Platform.Windows;
using OTB.Core.Hook.Platform.Windows.Native;



namespace OTB.Core
{
    public class OTBClient : IDisposable
    {
        private HookManager _hook;
        private ServerConnectionManager _connection;
        private ServerEventDispatcher _dispatcher;
        private ServerEventReceiver _receiver;
        private MouseUpdateManager _screen;
        public OTBClient(string serverAddress)
        {
            ClientState.ClientName = Environment.MachineName;
            
            _connection=new ServerConnectionManager(serverAddress); 
            _dispatcher=new ServerEventDispatcher(_connection);
            _screen=new MouseUpdateManager(); 
            _hook=new HookManager(_dispatcher, _screen);
            _receiver=new ServerEventReceiver(_connection, _hook, _screen);

            ClientState.Logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("OTB");
        }

        public bool Start()
        {
            _connection.Start();            
            _hook.Init();

            VirtualScreen s=null;
            foreach (var display in _hook.GetDisplays())
            {
                Console.WriteLine($"x={display.X},y={display.Y},width={display.Width},height={display.Height}");
                var vs=new VirtualScreen(){LocalX = display.X, LocalY = display.Y, VirtualX = display.X, VirtualY = display.Y, Width = display.Width, Height = display.Height};
                ClientState.ScreenConfiguration.AddScreen(vs, ClientState.ClientName,"");
            }
            
            _dispatcher.ClientCheckin(ClientState.ClientName, ClientState.ScreenConfiguration.Screens.Values.SelectMany(x=>x).ToList());
            _hook.Hook.SetMousePos(ClientState._lastPositionX, ClientState._lastPositionY);
            _hook.Start();
                
            return true;
        }

        public void RunMessageLoop()
        {
            //Windows needs a message pump
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Msg msg;
                while (true && NativeMethods.GetMessage(out msg, IntPtr.Zero, 0, 0) > 0)
                {
                    NativeMethods.TranslateMessage(ref msg);
                    NativeMethods.DispatchMessage(ref msg);
                }
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                //Cocoa run loop
                var runLoop = CF.CFRunLoopGetMain();
                if (runLoop == IntPtr.Zero)
                {
                    runLoop = CF.CFRunLoopGetCurrent();
                }
                if (runLoop == IntPtr.Zero)
                {
                    throw new InvalidOperationException();
                }

                //TODO: this is functional but wrong. no exit condition? will need to re-engineer this to support starting and stopping
                
                while (true)
                {
                    CF.CFRunLoopRunInMode(CF.RunLoopModeDefault, 0, false);
                }
            }
        }

        public void Dispose()
        {
            _hook?.Dispose();
			_connection?.Dispose();
        }

       
    }
    
    
    
}
