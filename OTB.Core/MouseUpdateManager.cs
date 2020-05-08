using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using OTB.Core.Hook;

namespace OTB.Core
{
    public enum CoordinateCalculationResult
    {
        Valid,
        Invalid
    }

    public class CoordinateUpdateResult
    {
        public bool MoveMouse { get; set; }
        public bool HandleEvent { get; set; }
        
    }
  
    public class MouseUpdateManager
    {
        public MouseUpdateManager()
        {
           
        }
        public CoordinateCalculationResult UpdateVirtualMouseCoordinates(MouseMoveEventArgs e)
        {
            //calculate the change from previous stored coordinates
            double deltaX = e.Mouse.X - ClientState._lastPositionX;
            double deltaY = e.Mouse.Y - ClientState._lastPositionY;
            
            ClientState._virtualX += deltaX;
            ClientState._virtualY += deltaY;
            
            var s = ClientState.ScreenConfiguration.GetScreenForVirtualCoordinate(ClientState._virtualX, ClientState._virtualY);

            if (s != null)
            {                
                return CoordinateCalculationResult.Valid;
            }
            
            ClientState._virtualX -= deltaX;
            ClientState._virtualY -= deltaY;
            
            return CoordinateCalculationResult.Invalid;

        }
        
 
        //TODO: rewrite this. there are really only  3 outcomes of this function.
        // 1) hide the mouse, show the mouse, replay the mouse from a remote server
        // 2) translate virtual coords to screen coords.
        // 2) update a field tracking the last local position of the mouse  -WHY DON'T I JUST ASK THE OS????

        //based on the current position of the virtual coordinates
        //decide whether to hide the mouse, pass the coords to the hook, or handle the event, or some combo.
        public CoordinateUpdateResult ProcessVirtualCoordinatesUpdate(bool replay=false)
        {
            CoordinateUpdateResult result=new CoordinateUpdateResult();
            
            //find the current screen
            //these coordinates may have just been updated from the server
            var s = ClientState.ScreenConfiguration.GetScreenForVirtualCoordinate(ClientState._virtualX, ClientState._virtualY);
            if (s == null)
                return result;
          
            if(s.Client==ClientState.ClientName)
            {
                
                var localX =Math.Abs(ClientState._virtualX - s.VirtualX) + s.LocalX;
                var localY = Math.Abs(ClientState._virtualY - s.VirtualY) + s.LocalY;
                
                ClientState._lastPositionX = localX;
                ClientState._lastPositionY = localY;
                
                //we previous weren't focused, but now we are
                if (!ClientState.CurrentClientFocused)
                {                   
                    //_hook.Hook.SetMousePos((int)localX, (int)localY);
                    result.MoveMouse = true;
                    result.HandleEvent = true;
                    //Console.WriteLine("regaining focus: " + localX + ","+ localY);
                    
                    //mark this as handled since we manually set the cursor and don't want it to rubberband around
                }
                ClientState.CurrentClientFocused = true;
               
                if (replay)
                {
                    result.MoveMouse = true;                     
                }
                
            }
            else
            {
                
                
                if (ClientState.CurrentClientFocused)
                {
                    //we have moved off screen now - hide the mouse                    
                    var screen = ClientState.ScreenConfiguration.Screens[ClientState.ClientName].First();

                    var localMaxX = 0+screen.Width-1;
                    var localMaxY = 0+screen.Height-1;
                    //hide mouse
                    //Console.WriteLine("detected coordinates outside of our current screen - hiding mouse at " + localMaxX + "," + localMaxY);
                    //_hook.Hook.SetMousePos((int)localMaxX, (int)localMaxY);
                    result.MoveMouse = true;
                    ClientState._lastPositionX = (int)localMaxX;
                    ClientState._lastPositionY = (int)localMaxY;
                    //Console.WriteLine("Setting last known position of mouse to " + localMaxX + "," + localMaxY);
                }

                //we are offscreen
                ClientState.CurrentClientFocused = false;
                result.HandleEvent = true;
            }

            return result;
        }

    }
    
    
}