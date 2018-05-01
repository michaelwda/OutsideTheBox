using System;
using System.Collections.Generic;
using System.Text;


namespace OTB.Core.Hook.Platform
{
    public abstract class GlobalHookBase : IGlobalHook
    {
        public event EventHandler<KeyboardKeyEventArgs> KeyDown = delegate { };
        public event EventHandler<KeyPressEventArgs> KeyPress = delegate { };
        public event EventHandler<KeyboardKeyEventArgs> KeyUp = delegate { };
        public event EventHandler<MouseButtonEventArgs> MouseDown = delegate { };
        public event EventHandler<MouseButtonEventArgs> MouseUp = delegate { };
        public event EventHandler<MouseMoveEventArgs> MouseMove = delegate { };
        public event EventHandler<MouseWheelEventArgs> MouseWheel = delegate { };
        public event EventHandler<ClipboardChangedEventArgs> Clipboard = delegate { };

  
        public abstract void SetMousePos(double x, double y);
        public abstract MousePoint GetMousePos();
        public abstract void SendMouseDown(MouseButton button);
        public abstract void SendMouseUp(MouseButton button);
        public abstract void SendMouseWheel(int dx, int dy);
        public abstract void SendKeyDown(Key key);
        public abstract void SendKeyUp(Key key);
        public abstract void SetClipboard(string value);
        public abstract List<Display> GetDisplays();
        public abstract void Init();
        public abstract void Start();

        protected readonly ClipboardChangedEventArgs ClipboardChangedEventArgs=new ClipboardChangedEventArgs();
        protected readonly MouseButtonEventArgs MouseDownArgs = new MouseButtonEventArgs();
        protected readonly MouseButtonEventArgs MouseUpArgs = new MouseButtonEventArgs();
        protected readonly MouseMoveEventArgs MouseMoveArgs = new MouseMoveEventArgs();
        protected readonly MouseWheelEventArgs MouseWheelArgs = new MouseWheelEventArgs();

        protected readonly KeyboardKeyEventArgs KeyDownArgs = new KeyboardKeyEventArgs();
        protected readonly KeyboardKeyEventArgs KeyUpArgs = new KeyboardKeyEventArgs();
        protected readonly KeyPressEventArgs KeyPressArgs = new KeyPressEventArgs((char)0);

        protected MouseState MouseState = new MouseState();
        protected KeyboardState KeyboardState = new KeyboardState();
       

        internal GlobalHookBase()
        {
            MouseState.SetIsConnected(true);            
            KeyboardState.SetIsConnected(true);
        
           
        }

        protected void OnKeyDown(Key key)
        {
            KeyboardState.SetKeyState(key, true);

            var e = KeyDownArgs;
            e.Keyboard = KeyboardState;
            e.Key = key;
            e.Handled = false;
            KeyDown(this, e);
        }

        protected void OnKeyPress(char c)
        {
            var e = KeyPressArgs;
            e.KeyChar = c;
            KeyPress(this, e);
        }

        protected void OnKeyUp(Key key)
        {
            KeyboardState.SetKeyState(key, false);

            var e = KeyUpArgs;
            e.Keyboard = KeyboardState;
            e.Key = key;
            e.Handled = false;

            KeyUp(this, e);
        }

        protected void OnClipboardChanged(string value)
        {
            var e = ClipboardChangedEventArgs;
            e.Value = value;
            Clipboard(this, e);
        }
        /// \internal
        /// <summary>
        /// Call this method to simulate KeyDown/KeyUp events
        /// on platforms that do not generate key events for
        /// modifier flags (e.g. Mac/Cocoa).
        /// Note: this method does not distinguish between the
        /// left and right variants of modifier keys.
        /// </summary>
        /// <param name="mods">Mods.</param>
        protected void UpdateModifierFlags(KeyModifiers mods)
        {
            bool alt = (mods & KeyModifiers.Alt) != 0;
            bool control = (mods & KeyModifiers.Control) != 0;
            bool shift = (mods & KeyModifiers.Shift) != 0;

            if (alt)
            {
                OnKeyDown(Key.AltLeft);
                OnKeyDown(Key.AltRight);
            }
            else
            {
                if (KeyboardState[Key.AltLeft])
                {
                    OnKeyUp(Key.AltLeft);
                }
                if (KeyboardState[Key.AltRight])
                {
                    OnKeyUp(Key.AltRight);
                }
            }

            if (control)
            {
                OnKeyDown(Key.ControlLeft);
                OnKeyDown(Key.ControlRight);
            }
            else
            {
                if (KeyboardState[Key.ControlLeft])
                {
                    OnKeyUp(Key.ControlLeft);
                }
                if (KeyboardState[Key.ControlRight])
                {
                    OnKeyUp(Key.ControlRight);
                }
            }

            if (shift)
            {
                OnKeyDown(Key.ShiftLeft);
                OnKeyDown(Key.ShiftRight);
            }
            else
            {
                if (KeyboardState[Key.ShiftLeft])
                {
                    OnKeyUp(Key.ShiftLeft);
                }
                if (KeyboardState[Key.ShiftRight])
                {
                    OnKeyUp(Key.ShiftRight);
                }
            }
        }

        protected void OnMouseDown(MouseButton button)
        {
            MouseState[button] = true;

            var e = MouseDownArgs;
            e.Button = button;
            e.IsPressed = true;
            e.Mouse = MouseState;
            e.Handled = false;

            MouseDown(this, e);
        }

        protected void OnMouseUp(MouseButton button)
        {
           
            MouseState[button] = false;

            var e = MouseUpArgs;
            e.Button = button;
            e.IsPressed = false;
            e.Mouse = MouseState;
            e.Handled = false;

            MouseUp(this, e);
        }

        protected void OnMouseMove(int x, int y)
        {
            MouseState.X = x;
            MouseState.Y = y;

            var e = MouseMoveArgs;
            e.Mouse = MouseState;             
            e.Handled = false;

           

            MouseMove(this, e);
            
        }

        protected void OnMouseWheel(int dx, int dy)
        {

            
            var e = MouseWheelArgs;
            e.Mouse = MouseState;
            e.DeltaX = dx;
            e.DeltaY = dy;
            e.Handled = false;
           
            MouseWheel(this, e);
        }


        public abstract void Dispose();
    }
}
