using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace OTB.Core
{
    public class ScreenConfiguration
    {
        
        public ConcurrentDictionary<string,List<VirtualScreen>> Screens { get; set; }
        public ScreenConfiguration()
        {
            Screens = new ConcurrentDictionary<string, List<VirtualScreen>>();
        }

        public VirtualScreen AddScreen(VirtualScreen screen, string client, string connectionId)
        {
            var furthestRight = GetFurthestRightScreen();
            if (furthestRight == null)
                return AddScreen(screen.VirtualX, screen.VirtualY, screen.VirtualX, screen.VirtualY, screen.Width, screen.Height, client, connectionId);
            else
            {
                return AddScreen(localX, localY, orig.VirtualX + orig.Width, orig.VirtualY, width, height, client, connectionId);
            }
        }

        private VirtualScreen AddScreen(double localX, double localY, double virtualX, double virtualY, double width, double height, string client, string connectionId)
        {
            double newXBottomCorner = virtualX + width-1;
            double newYBottomCorner = virtualY + height-1;
            //check and make sure we can add this
             
            foreach (var s in Screens.Values.SelectMany(x=>x))
            {
                var existingX = s.VirtualX;
                var existingY = s.VirtualY;
                var existingXBottomCorner = s.VirtualX + s.Width-1;
                var existingYBottomCorner = s.VirtualY + s.Height-1; 
                
                //no overlap of x coords
                if (virtualX > existingXBottomCorner || newXBottomCorner < existingX )
                    continue;

                // If one rectangle is above other
                //screen coordinates have the Y flipped, so we have to adjust this part by flipping the comparisons from what you would normally see
                if (virtualY > existingYBottomCorner || newYBottomCorner < existingY)
                    continue;

                return null; //this is a coordinate overlap
            }

            //all good, add the new screen
            VirtualScreen newScreen=new VirtualScreen();
            newScreen.LocalX = localX;
            newScreen.LocalY = localY;
            newScreen.VirtualX = virtualX;
            newScreen.VirtualY = virtualY;
            newScreen.Width = width;
            newScreen.Height = height;
            newScreen.Client = client;
            newScreen.ConnectionId = connectionId;

            if(!Screens.ContainsKey(client))
            {
                Screens.TryAdd(client, new List<VirtualScreen>());
            }
            Screens[client].Add(newScreen);
          
       
            return newScreen;
        }

        public VirtualScreen GetScreenForVirtualCoordinate(double x, double y)
        {
            foreach (var s in Screens.Values.SelectMany(s=>s))
            {
                if (x >= s.VirtualX && x < (s.VirtualX + s.Width) && y >= s.VirtualY && y < (s.VirtualY + s.Height))
                    return s;
            }

            return null;
        }
        private VirtualScreen GetFurthestRightScreen()
        {
            VirtualScreen furthestRight = null;
            var maxX = double.MinValue;
            foreach (var s in Screens.Values.SelectMany(x => x)) 
            {
                var maxForThisScreen = s.VirtualX + s.Width;
                if (maxForThisScreen > maxX)
                {
                    maxX = maxForThisScreen;
                    furthestRight = s;
                }
            }

            return furthestRight;

        }
        private VirtualScreen GetFurthestLeftScreen()
        {
            VirtualScreen furthestLeft = null;
            var minX = double.MaxValue;
            foreach (var s in Screens.Values.SelectMany(x => x))
            {
                var minForThisScreen = s.VirtualX;
                if (minForThisScreen < minX)
                {
                    minX = minForThisScreen;
                    furthestLeft = s;
                }
            }

            return furthestLeft;

        }
        public List<VirtualScreen> GetScreensForConnection(string connectionId)
        {
            return Screens.Values.SelectMany(x=>x).Where(x => x.ConnectionId == connectionId).ToList();
        }

        //function to support removing screen in an arbitrary place. Will collapse other screens in.
        public void Remove(VirtualScreen s)
        {
            var left = GetFurthestLeftScreen();
            var right = GetFurthestRightScreen();

            //Screens
            Screens[s.Client].Remove(s);

            //so, right now i'm just adding screens left and right. I haven't done much with positioning up and down.
            //i'm going to keep this simple, but eventually we'll want to implement some kind of grid collapsing function
            //like masonry 

            
            if (s == left || s == right)
                return;

            //for every screen with a starting X coord that's greater than this, move it back towards 0
            foreach(var screen in Screens.Values.SelectMany(x=>x).ToList())
            {
                if(screen.VirtualX > s.VirtualX)
                {
                    screen.VirtualX -= s.Width;
                }
            }

        }
    }
}