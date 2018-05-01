using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OTB.Core.Hook
{   
    public interface IGlobalHook : IDisposable
    {
       
        event EventHandler<KeyboardKeyEventArgs> KeyDown;
        event EventHandler<KeyPressEventArgs> KeyPress;
        event EventHandler<KeyboardKeyEventArgs> KeyUp;
        event EventHandler<MouseButtonEventArgs> MouseDown;
        event EventHandler<MouseButtonEventArgs> MouseUp;
        event EventHandler<MouseMoveEventArgs> MouseMove;
        event EventHandler<MouseWheelEventArgs> MouseWheel;
        event EventHandler<ClipboardChangedEventArgs> Clipboard;
        
        void SetMousePos(double x, double y);
        MousePoint GetMousePos();

        void SendMouseDown(MouseButton button);
        void SendMouseUp(MouseButton button);
        void SendMouseWheel(int dx, int dy);
        void SendKeyDown(Key key);
        void SendKeyUp(Key key);
        void SetClipboard(string value);
        List<Display> GetDisplays();

        void Init();
        void Start();

        
    }

    
    
}
