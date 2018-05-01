using Avalonia;
using Avalonia.Markup.Xaml;
using OTB.Core;

namespace OTB.ClientUI
{
    public class App : Application
    {
        
        public static OTBClient client;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void StartClient()
        {
            if (!client.Start())
            {
                Exit();
            }
        }
    }
}
