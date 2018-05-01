namespace OTB.Core.Hook
{
    public class MouseWheelEventArgs : MouseEventArgs
    { 
        public int DeltaX { get; internal set; }
        public int DeltaY { get; set; }
    }
}