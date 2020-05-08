using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace OTB.Core
{
    public class VirtualScreen 
    {
        /*
         * Multiple monitors can be connected and arranged in a variety of ways
         * For example, you may have two 4k monitors, with the second one at a horizontal orientation and shifted down on the Y axis.
         *  ________________    _________
         *  |               |   |       |
         *  |               |   |       |
         *  -----------------   |       |
         *                      |       |
         *                      |       |
         *                      ---------
         *
         * In windows, 0,0 is the top left - so this would correspond to
         * x=0,y=0,width=3840,height=2160
         * x=3840,y=0,width=2160,height=3840
         *
         *                      _________
         *                      |       |
         *                      |       |
         *  ________________    |       |
         *  |               |   |       |
         *  |               |   |       |
         *  -----------------   ---------
         *
         * If you shifted the right monitor up (North), then monitor 1 would be at 0,0 and the other would be have a negative offset on the & axis
         * x=0,y=0,width=3840,height=2160
         * x=3840,y=-1680,width=2160,height=3840
         *
         */
        public double LocalX { get; set; }   
        public double LocalY { get; set; }
        public double Width { get; set; } = 0;
        public double Height { get; set; } = 0;

        /*
         * These coordinates are created by our application depending on how we've arranged our screens    
         */
        public double VirtualX { get; set; } = 0;
        public double VirtualY { get; set; } = 0;

        public string ConnectionId { get; internal set; }
        public string Client { get; set; } = "";
    }

     
}
