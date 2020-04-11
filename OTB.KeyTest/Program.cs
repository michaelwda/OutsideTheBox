using Microsoft.Extensions.Logging;
using OTB.Core;
using System;

namespace OTB.KeyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new ServerConnectionManager("http://localhost");
            var dispatcher = new ServerEventDispatcher(connection);
            var screen = new VirtualScreenManager();
            var hook = new HookManager(dispatcher, screen);
            ClientState.Logger = LoggerFactory.Create(builder=>builder.AddConsole().SetMinimumLevel(LogLevel.Warning)).CreateLogger("OTB");
            hook.Start();

            hook.Hook.SendKeyDown(Core.Hook.Key.WinLeft);
            hook.Hook.SendKeyUp(Core.Hook.Key.WinLeft);
            
            hook.Stop();
            Console.ReadKey();
            Console.Read();

        }
    }
}
