namespace OTB.Core.Hook
{
    public class MousePoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public MousePoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public MousePoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}