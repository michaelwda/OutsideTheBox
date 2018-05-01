namespace OTB.Core.Hook.Platform.Windows.Native
{
    public enum MouseState
    {
        MouseMove = 0x0200,
        LeftButtonDown = 0x0201,

        LeftButtonUp = 0x0202,
        LBUTTONDBLCLK = 0x0203,
        

        MouseWheel = 0x020A,
        MouseHWheel = 0x020E,
        RightButtonDown = 0x0204,

        RightButtonUp = 0x0205,
        RBUTTONDBLCLK = 0x0206,

        MBUTTONDOWN = 0x0207,
        MBUTTONUP = 0x0208,
        MBUTTONDBLCLK = 0x0209,
        XBUTTONDOWN = 0x020B,
        XBUTTONUP = 0x020C,
        XBUTTONDBLCLK = 0x020D,
    }
}