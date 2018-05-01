namespace OTB.Core
{
    public class VirtualScreen 
    {
        public double LocalX { get; set; }   
        public double LocalY { get; set; }
        public string Client { get; set; } = "";
        public double Width { get; set; } = 0;
        public double Height { get; set; } = 0;
        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;
        public string ConnectionId { get; internal set; }
    }
}
