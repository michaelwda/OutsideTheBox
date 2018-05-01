using System;
using Avalonia;
using Avalonia.Logging.Serilog;

namespace OTB.ClientUI
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<ConnectingWindow>();
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug();
    }
}
