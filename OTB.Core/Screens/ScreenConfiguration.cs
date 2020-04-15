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
            Screens= new ConcurrentDictionary<string, List<VirtualScreen>>();
        }
        
        public VirtualScreen AddScreen(double localX, double localY, double virtualX, double virtualY, double width, double height, string client, string connectionId)
        {
            double newXBottomCorner = virtualX + width-1;
            double newYBottomCorner = virtualY + height-1;
            //check and make sure we can add this
             
            foreach (var s in Screens.Values.SelectMany(x=>x))
            {
                var existingX = s.X;
                var existingY = s.Y;
                var existingXBottomCorner = s.X + s.Width-1;
                var existingYBottomCorner = s.Y + s.Height-1; 
                
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
            newScreen.X = virtualX;
            newScreen.Y = virtualY;
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

        public VirtualScreen AddScreenRight(VirtualScreen orig, double localX, double localY, double width, double height, string client, string connectionId)
        {            
            return AddScreen(localX, localY, orig.X + orig.Width, orig.Y, width, height, client, connectionId);            
        }
        public VirtualScreen AddScreenLeft(VirtualScreen orig, double localX, double localY, double width, double height, string client, string connectionId)
        {
            return AddScreen(localX, localY, orig.X - width, orig.Y, width, height, client, connectionId);
        }
        public VirtualScreen AddScreenAbove(VirtualScreen orig, double localX, double localY, double width, double height, string client, string connectionId)
        {
            return AddScreen(localX, localY, orig.X, orig.Y-height, width, height, client, connectionId);
        }
        public VirtualScreen AddScreenBelow(VirtualScreen orig, double localX, double localY, double width, double height, string client, string connectionId)
        {
            return AddScreen(localX, localY, orig.X, orig.Y + orig.Height, width, height, client, connectionId);
        }

        public VirtualScreen ValidVirtualCoordinate(double x, double y)
        {
            foreach (var s in Screens.Values.SelectMany(s=>s))
            {
                if (x >= s.X && x < (s.X + s.Width) && y >= s.Y && y < (s.Y + s.Height))
                    return s;
            }

            return null;
        }

        public VirtualScreen GetFurthestRight()
        {
            VirtualScreen furthestRight = null;
            var maxX = double.MinValue;
            foreach (var s in Screens.Values.SelectMany(x => x)) 
            {
                var maxForThisScreen = s.X + s.Width;
                if (maxForThisScreen > maxX)
                {
                    maxX = maxForThisScreen;
                    furthestRight = s;
                }
            }

            return furthestRight;

        }

        public VirtualScreen GetFurthestLeft()
        {
            VirtualScreen furthestLeft = null;
            var minX = double.MaxValue;
            foreach (var s in Screens.Values.SelectMany(x => x))
            {
                var minForThisScreen = s.X;
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
            var left = GetFurthestLeft();
            var right = GetFurthestRight();

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
                if(screen.X>s.X)
                {
                    screen.X -= s.Width;
                }
            }

        }
    }
}