using System.Runtime.InteropServices;

namespace OTB.Core.Hook.Platform.Windows.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }


    }
}