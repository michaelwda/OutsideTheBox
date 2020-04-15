using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OTB.Core;
using OTB.Core.Hook;

namespace OTB.Client
{
    public class OTBHub : Hub<IOTBClient>
    {
        private readonly ILogger _logger;
        static readonly ScreenConfiguration ScreenConfiguration = new ScreenConfiguration();
        public OTBHub(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("Hub");
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("Connected!");
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var screens=ScreenConfiguration.GetScreensForConnection(Context.ConnectionId).ToList();
            foreach(var s in screens)
            {
                ScreenConfiguration.Remove(s);
            }
            await Clients.All.ScreenConfigUpdate(ScreenConfiguration.Screens.Values.SelectMany(x => x).ToList());
        }

        public Task Clipboard(string value)
        {
            return Clients.Others.Clipboard(value);
        }

		public Task MouseMove(double x, double y)
        {
            _logger.LogDebug($"{x},{y}");
            return Clients.Others.MouseMove(x, y);
        }
		public Task MouseWheel(int x, int y)
        {
			return Clients.Others.MouseWheel(x, y);
        }
		public Task MouseDown(MouseButton button)
        {
            return Clients.Others.MouseDown(button);
        }
		public Task MouseUp(MouseButton button)
        {
            return Clients.Others.MouseUp(button);
        }
		public Task KeyDown(Key key)
        {
            return Clients.Others.KeyDown(key);
        }
		public Task KeyUp(Key key)
        {
            return Clients.Others.KeyUp(key);
        }

        //we have multiple monitors for a client. 
        //we may want to remove a single monitor. We should probably track a unique id per monitor that can be retrieved. 
		public Task ClientCheckin(string client, List<VirtualScreen> screens) 
        {
            var connectionId = Context.ConnectionId;

            if (screens.Count < 1)
				return Task.CompletedTask;

            //add each of these displays
            if (ScreenConfiguration.Screens.ContainsKey(client) && ScreenConfiguration.Screens[client].Any())
            {
                //this client is already configured.
                //TODO: we actually need to handle this - like if they add a new monitor...
                return Clients.All.ScreenConfigUpdate(ScreenConfiguration.Screens.Values.SelectMany(x => x).ToList());
            }
            else
            {
                //for now, as clients start, we're just adding them to the right
                if (!ScreenConfiguration.Screens.Any())
                {
                    foreach (var screen in screens)
                    {
                        var newScreen = ScreenConfiguration.AddScreen(screen.X, screen.Y, screen.X, screen.Y, screen.Width, screen.Height, client, connectionId);
                    }

                    return Clients.All.ScreenConfigUpdate(ScreenConfiguration.Screens.Values.SelectMany(x => x).ToList());
                }
                else
                {

                    foreach (var screen in screens)
                    {
                        //attempt to simply add this screen in the requested position for a reconnect
                        var newScreen = ScreenConfiguration.AddScreen(screen.X, screen.Y, screen.X, screen.Y, screen.Width, screen.Height, client, connectionId);

                        if (newScreen == null)
                        {
                            VirtualScreen s = ScreenConfiguration.GetFurthestRight();
                            ScreenConfiguration.AddScreenRight(s, screen.X, screen.Y, screen.Width, screen.Height, client, connectionId);
                           
                        }
                    }

					return Clients.All.ScreenConfigUpdate(ScreenConfiguration.Screens.Values.SelectMany(x=>x).ToList());
                }

            }
        }
    }
}
