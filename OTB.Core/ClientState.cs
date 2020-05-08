using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using OTB.Core.Hook;

namespace OTB.Core
{
    //TODO: probably not threadsafe. guess i need a lock object on all of these?
    //TODO: furthermore, a bunch of stuff is referencing these. really ugly stuff. let's put all the 
    //logic into various container classes. like, if you want to update the current client, you need to call a 
    //method on a the virtual screen manager.
    
    public static class ClientState
    {
        public static bool CurrentClientFocused { get; set; } = true;       
        public static string ClientName { get; set; } = "";
        
        public static ScreenConfiguration ScreenConfiguration { get; set; } = new ScreenConfiguration();
        public static double _virtualX = double.MaxValue; //computed position
        public static double _virtualY = double.MaxValue; //computed position   
        public static double _lastPositionX; //stored value of our last position before going off-screen
        public static double _lastPositionY; //stored value of our last position before going off-screen
        public static DateTime LastHookEvent_Mouse { get; set; } = DateTime.Now;
        public static DateTime LastHookEvent_Keyboard { get; set; } = DateTime.Now;
        public static DateTime LastServerEvent_Mouse { get; set; } = DateTime.Now;
        public static DateTime LastServerEvent_Keyboard { get; set; } = DateTime.Now;

        public static ILogger Logger { get; set; }
    }
}