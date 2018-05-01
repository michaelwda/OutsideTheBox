using System;
using System.Collections.Generic;
using System.Text;

namespace OTB.Core.Hook
{
    public class Display
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public Display(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
