using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using OTB.Core.Hook.Platform.OSX.Native;
using OTB.Core.Hook.Platform.OSX.Native.CF;
using OTB.Core.Hook.Platform.OSX.Native.CG;
using OTB.Core.Hook.Platform.OSX.Native.NS;

namespace OTB.Core.Hook.Platform.OSX
{
    public class OsxGlobalHook : GlobalHookBase, IDisposable
    {
        
        private CG.EventTapCallBack _eventHookProc;
        private IntPtr _eventTap;
        private IntPtr runLoop;
        private bool _ignoreNextMovement;
       

        public override void SendMouseDown(MouseButton button)
        {
            var pos = _getMousePoint();
            MouseState[button] = true;
            if ((button == MouseButton.Left))
            {
                var e = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.LeftMouseDown, new CGPoint(){x=pos.X, y=pos.Y}, CGMouseButton.Left);
                //CG.EventSetIntegerValueField(e, CGEventField.MouseEventClickState,1);             
                CG.CGEventPost(CGEventTapLocation.HIDEventTap, e);
                CF.CFRelease(e);
            }
            if ((button == MouseButton.Right))
            {
                var e = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.RightMouseDown, new CGPoint() { x = pos.X, y = pos.Y }, CGMouseButton.Right);
                

                CG.CGEventPost(CGEventTapLocation.HIDEventTap, e);
                CF.CFRelease(e);
            }
            if ((button == MouseButton.Middle))
            {
                var e = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.OtherMouseDown, new CGPoint() { x = pos.X, y = pos.Y }, CGMouseButton.Center);
              

                CG.CGEventPost(CGEventTapLocation.HIDEventTap, e);
                CF.CFRelease(e);
            }
            if ((button == MouseButton.Button1))
            {
                var e = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.OtherMouseDown, new CGPoint() { x = pos.X, y = pos.Y }, CGMouseButton.Right);
                CG.EventSetIntegerValueField(e, CGEventField.MouseEventButtonNumber,3);

                CG.CGEventPost(CGEventTapLocation.HIDEventTap, e);
                CF.CFRelease(e);
            }
            if ((button == MouseButton.Button2))
            {
                var e = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.OtherMouseDown, new CGPoint() { x = pos.X, y = pos.Y }, CGMouseButton.Right);
                CG.EventSetIntegerValueField(e, CGEventField.MouseEventButtonNumber, 4);

                CG.CGEventPost(CGEventTapLocation.HIDEventTap, e);
                CF.CFRelease(e);
            }
        }
        public override void SendMouseUp(MouseButton button)
        {
            
            var pos = _getMousePoint();
            MouseState[button] = false;
            if ((button == MouseButton.Left))
            {
                var e = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.LeftMouseUp, new CGPoint() { x = pos.X, y = pos.Y }, CGMouseButton.Left);                
                CG.CGEventPost(CGEventTapLocation.HIDEventTap, e);
                CF.CFRelease(e);
            }
            if ((button == MouseButton.Right))
            {
                var e = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.RightMouseUp, new CGPoint() { x = pos.X, y = pos.Y }, CGMouseButton.Right);


                CG.CGEventPost(CGEventTapLocation.HIDEventTap, e);
                CF.CFRelease(e);
            }
            if ((button == MouseButton.Middle))
            {
                var e = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.OtherMouseUp, new CGPoint() { x = pos.X, y = pos.Y }, CGMouseButton.Center);


                CG.CGEventPost(CGEventTapLocation.HIDEventTap, e);
                CF.CFRelease(e);
            }
            if ((button == MouseButton.Button1))
            {
                var e = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.OtherMouseUp, new CGPoint() { x = pos.X, y = pos.Y }, CGMouseButton.Right);
                CG.EventSetIntegerValueField(e, CGEventField.MouseEventButtonNumber, 3);

                CG.CGEventPost(CGEventTapLocation.HIDEventTap, e);
                CF.CFRelease(e);
            }
            if ((button == MouseButton.Button2))
            {
                var e = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.OtherMouseUp, new CGPoint() { x = pos.X, y = pos.Y }, CGMouseButton.Right);
                CG.EventSetIntegerValueField(e, CGEventField.MouseEventButtonNumber, 4);

                CG.CGEventPost(CGEventTapLocation.HIDEventTap, e);
                CF.CFRelease(e);
            }



        }

        public override void SendMouseWheel(int dx, int dy)
        {
            var e = CG.CGEventCreateScrollWheelEvent(IntPtr.Zero, CGScrollEventUnit.UnitPixel, 2, dy, dx);
            CG.CGEventPost(CGEventTapLocation.HIDEventTap, e);
        }


        public override void SetClipboard(string value)
        {
            //grab pasteboard
            var pasteBoard = Cocoa.SendIntPtr(Cocoa.objc_getClass("NSPasteboard"), Selector.Get("generalPasteboard"));
            //clear it
            Cocoa.SendInt(pasteBoard, Selector.Get("clearContents"));


            var data = Cocoa.ToNSString(value);
         
            var arr= Cocoa.SendIntPtr(Cocoa.objc_getClass("NSArray"), Selector.Get("arrayWithObjects:"), data, IntPtr.Zero);


            //send contents
            Cocoa.SendVoid(pasteBoard, Selector.Get("writeObjects:"), arr);
        }

        public override List<Display> GetDisplays()
        {
            //I strongly suspect this is going to be broken for multiple monitors due to the way
            //cocoa treats the coordiante system.
            //a second monitor above the current would be given positive coords, rather than the negative coords i'm expecting.
            //solution is... -1 *
            
            var screens = Cocoa.SendIntPtr(Cocoa.objc_getClass("NSScreen"), Selector.Get("screens"));
            var primary = Cocoa.SendIntPtr(screens,Selector.Get("objectAtIndex:"), 0);
            var screenRect=Cocoa.SendRect(primary,Selector.Get("frame"));
			return new List<Display> { new Display(screenRect.X, -1*screenRect.Y, screenRect.Width, screenRect.Height) };
            
        }

        public override void Init()
        {             
            //init initial cursor position       
            this.MouseState.Position=new MousePoint(0,0);
           
        }

        public override void Start()
        {
            _eventHookProc = EventProc;

            runLoop = CF.CFRunLoopGetMain();
            if (runLoop == IntPtr.Zero)
                runLoop = CF.CFRunLoopGetCurrent();
            if (runLoop == IntPtr.Zero)
            {
                Debug.Print("[Error] No CFRunLoop found for {0}", GetType().FullName);
                throw new InvalidOperationException();
            }
            CF.CFRetain(runLoop);
            
            _eventTap = CG.EventTapCreate(CGEventTapLocation.HIDEventTap, CGEventTapPlacement.HeadInsert, CGEventTapOptions.Default, CGEventMask.All, _eventHookProc, IntPtr.Zero);
            CG.EventTapEnable(_eventTap, true);
            var runLoopSource = CF.MachPortCreateRunLoopSource(IntPtr.Zero, _eventTap, IntPtr.Zero);
            
            CF.RunLoopAddSource(runLoop, runLoopSource, CF.RunLoopModeDefault);
        }
        
        
        private IntPtr EventProc(IntPtr proxy, CGEventType type, IntPtr @event, IntPtr refcon)
        {
             
            if (type == CGEventType.KeyDown)
            {
                var code = CG.EventGetIntegerValueField(@event, CGEventField.KeyboardEventKeycode);
                if (!Enum.IsDefined(typeof(MacOSKeyCode), code))
                {
                    Console.WriteLine("THIS KEY IS NOT DEFINED!!!");
                    return @event;
                }
                
                var key = MacOSKeyMap.GetKey((MacOSKeyCode)code);
                OnKeyDown(key);
                return KeyDownArgs.Handled ? IntPtr.Zero : @event;
            }
            if (type == CGEventType.KeyUp)
            {
                var code = CG.EventGetIntegerValueField(@event, CGEventField.KeyboardEventKeycode);
                if (!Enum.IsDefined(typeof(MacOSKeyCode), code))
                {
                    Console.WriteLine("THIS KEY IS NOT DEFINED!!!");
                    return @event;
                }
                
                var key = MacOSKeyMap.GetKey((MacOSKeyCode)code);
                OnKeyUp(key);
                return KeyUpArgs.Handled ? IntPtr.Zero : @event;
            }
            if (type == CGEventType.FlagsChanged) //this may be important for persistent effects. like a caps lock key press will return a keydown, and not immediately a keyup. This is likely relevant so we can track caps state. 
            //will likely need to sync caps state across clients. How can we enable OSX LED light??
            //TODO: caps lock sync and led
            {
                var code = CG.EventGetIntegerValueField(@event, CGEventField.KeyboardEventKeycode);
                if (!Enum.IsDefined(typeof(MacOSKeyCode), code))
                {
                    Console.WriteLine("THIS KEY IS NOT DEFINED!!!");
                    return @event;
                }
               // Console.WriteLine(code);
            }
            
            if (type == CGEventType.MouseMoved)
            {
                //using the mouse point here messes us up because of the warp command and inability to cancel mouse
                //move events.
                //var pt = _getMousePoint();
                var deltaX = CG.EventGetIntegerValueField(@event, CGEventField.MouseEventDeltaX);
                var deltaY = CG.EventGetIntegerValueField(@event, CGEventField.MouseEventDeltaY);
                
                var newX = (int)ClientState._lastPositionX + deltaX;
                var newY = (int)ClientState._lastPositionY + deltaY;
                //OSX won't let me handle these events, so I instead post an additional move the event tap. Ignore these
                if (_ignoreNextMovement)
                {
                    _ignoreNextMovement = false;
                    
                    return @event;
                }
                
                
                this.OnMouseMove(newX, newY);
               
                return MouseMoveArgs.Handled ? IntPtr.Zero : @event;
            }

            if (type == CGEventType.LeftMouseDown)
            {
                this.OnMouseDown(MouseButton.Left);                 
                return MouseDownArgs.Handled ? IntPtr.Zero : @event;
            }
            if (type == CGEventType.RightMouseDown)
            {
                this.OnMouseDown(MouseButton.Right);                 
                return MouseDownArgs.Handled ? IntPtr.Zero : @event;
            }
            if (type == CGEventType.OtherMouseDown)
            {
                var btn = CG.EventGetIntegerValueField(@event, CGEventField.MouseEventButtonNumber);
                if (btn == 2)
                {
                    this.OnMouseDown(MouseButton.Middle);  
                    return MouseDownArgs.Handled ? IntPtr.Zero : @event;
                }
                if (btn == 3)
                {
                    this.OnMouseDown(MouseButton.Button1);  
                    return MouseDownArgs.Handled ? IntPtr.Zero : @event;
                }
                if (btn == 4)
                {
                    this.OnMouseDown(MouseButton.Button2);  
                    return MouseDownArgs.Handled ? IntPtr.Zero : @event;
                }

                return @event;
            }
            if (type == CGEventType.LeftMouseUp)
            {
                this.OnMouseUp(MouseButton.Left);                 
                return MouseUpArgs.Handled ? IntPtr.Zero : @event;
            }
            if (type == CGEventType.RightMouseUp)
            {
                this.OnMouseUp(MouseButton.Right);                 
                return MouseUpArgs.Handled ? IntPtr.Zero : @event;
            }
            if (type == CGEventType.OtherMouseUp)
            {
                var btn = CG.EventGetIntegerValueField(@event, CGEventField.MouseEventButtonNumber);
                if (btn == 2)
                {
                    this.OnMouseUp(MouseButton.Middle);  
                    return MouseUpArgs.Handled ? IntPtr.Zero : @event;
                }
                if (btn == 3)
                {
                    this.OnMouseUp(MouseButton.Button1);  
                    return MouseUpArgs.Handled ? IntPtr.Zero : @event;
                }
                if (btn == 4)
                {
                    this.OnMouseUp(MouseButton.Button2);  
                    return MouseUpArgs.Handled ? IntPtr.Zero : @event;
                }

                return @event;
            }

            if (type == CGEventType.ScrollWheel)
            {
                var wheelDeltaY = CG.EventGetIntegerValueField(@event, CGEventField.ScrollWheelEventDeltaAxis1);
                var wheelDeltaX = CG.EventGetIntegerValueField(@event, CGEventField.ScrollWheelEventDeltaAxis2);
                
                this.OnMouseWheel(wheelDeltaX, wheelDeltaY);
                return MouseWheelArgs.Handled ? IntPtr.Zero : @event;
            }
             
            
            return @event;
        }

        public void WarpMouse(double x, double y)
        {
            
            CG.WarpMouseCursorPosition(new NSPoint() {X = x, Y = y});
            _ignoreNextMovement = true;
        }

        
        
        public override void SetMousePos(double x, double y)
        {
            //we are moving, check if we have a mouse button pushed down. If so, we create a drag event for each one pressed.
            if(!MouseState.IsDragging && MouseState.IsAnyButtonDown)
            {
                //we have a button down, we're moving, but we're not currently dragging. Send a drag event
                if(MouseState.LeftButton==ButtonState.Pressed)
                {
                    var drag = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.LeftMouseDragged, new CGPoint() { x = x, y = y }, CGMouseButton.Left);
                    CG.CGEventPost(CGEventTapLocation.HIDEventTap, drag);
                    CF.CFRelease(drag);
                }
                if (MouseState.RightButton == ButtonState.Pressed)
                {
                    var drag = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.RightMouseDragged, new CGPoint() { x = x, y = y }, CGMouseButton.Right);
                    CG.CGEventPost(CGEventTapLocation.HIDEventTap, drag);
                    CF.CFRelease(drag);
                }
                if (MouseState.MiddleButton == ButtonState.Pressed)
                {
                    var drag = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.OtherMouseDragged, new CGPoint() { x = x, y = y }, CGMouseButton.Center);
                    CG.CGEventPost(CGEventTapLocation.HIDEventTap, drag);
                    CF.CFRelease(drag);
                }
                if (MouseState.XButton1 == ButtonState.Pressed)
                {
                    var drag = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.OtherMouseDragged, new CGPoint() { x = x, y = y }, CGMouseButton.Right);
                    CG.EventSetIntegerValueField(drag, CGEventField.MouseEventButtonNumber, 3);
                    CG.CGEventPost(CGEventTapLocation.HIDEventTap, drag);
                    CF.CFRelease(drag);
                }
                if (MouseState.XButton2 == ButtonState.Pressed)
                {
                    var drag = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.OtherMouseDragged, new CGPoint() { x = x, y = y }, CGMouseButton.Right);
                    CG.EventSetIntegerValueField(drag, CGEventField.MouseEventButtonNumber, 4);
                    CG.CGEventPost(CGEventTapLocation.HIDEventTap, drag);
                    CF.CFRelease(drag);
                }
            }

            _ignoreNextMovement = true;
           

            var e = CG.CGEventCreateMouseEvent(IntPtr.Zero, CGEventType.MouseMoved, new CGPoint() { x = x, y = y}, 0);
            CG.CGEventPost(CGEventTapLocation.HIDEventTap, e);


            CF.CFRelease(e);
        }
        
        
        public override MousePoint GetMousePos()
        {
            //get mouse point is using cocoa, which has the coordinates starting in the bottom left.
            //the function here has already performed the flip for us.
            var p = _getMousePoint();
            return new MousePoint(p.X, p.Y);
        }

        private NSPoint _getMousePointBeforeTranslation()
        {
            var nsEvent=Cocoa.objc_getClass("NSEvent");           
            var point=Cocoa.SendPoint(nsEvent, Selector.Get("mouseLocation"));
     
            CF.CFRelease(nsEvent);
            return point;
        }
        private NSPoint _getMousePoint()
        {
            //NSEvent treats coordinates as starting in bottom left.
            //We apply a transform to make them match our windows counterpart.
            var point = _getMousePointBeforeTranslation();
         
            var screenRect =
                Cocoa.SendRect(
                    Cocoa.SendIntPtr(
                        Cocoa.SendIntPtr(Cocoa.objc_getClass("NSScreen"), Selector.Get("screens")),
                        Selector.Get("objectAtIndex:"), 0),
                    Selector.Get("frame"));

            point.Y = screenRect.Height - point.Y;
          
            return point;
        }


        public override void SendKeyDown(Key key)
        {
            KeyboardState.SetKeyState(key, true);
            var osxkey = MacOSKeyMap.GetKey(key);

            //just testing if I can do a key conversion here
            if (key == Key.WinRight)
                osxkey = MacOSKeyCode.Fn;
            
            var e = CG.CGEventCreateKeyboardEvent(IntPtr.Zero, (ushort)osxkey, true);
            CGEventFlags flags = BuildFlags(e);
            CG.CGEventSetFlags(e,flags);
            
            CG.CGEventPost(CGEventTapLocation.HIDEventTap, e);
            CF.CFRelease(e);
        }

        public override void SendKeyUp(Key key)
        {
            KeyboardState.SetKeyState(key, false);
            var osxkey = MacOSKeyMap.GetKey(key);
            
            //just testing if I can do a key conversion here
            if (key == Key.WinRight)
                osxkey = MacOSKeyCode.Fn;
            
            var e = CG.CGEventCreateKeyboardEvent(IntPtr.Zero, (ushort)osxkey, false);
            CGEventFlags flags = BuildFlags(e);
            CG.CGEventSetFlags(e,flags);
            CG.CGEventPost(CGEventTapLocation.HIDEventTap, e);
            CF.CFRelease(e);
        }

        private CGEventFlags BuildFlags(IntPtr e)
        {
            var flags = CG.CGEventGetFlags(e);
            if (KeyboardState.IsKeyDown(Key.ShiftLeft) || KeyboardState.IsKeyDown(Key.ShiftLeft))
            {                
                flags = flags | CGEventFlags.Shift;
            }

            if (KeyboardState.IsKeyDown(Key.CapsLock)) //not sure about this, i likely need to poll for caps state and then toggle.          
            {
                flags = flags | CGEventFlags.CapsLock;
            }
            if (KeyboardState.IsKeyDown(Key.AltLeft)|| KeyboardState.IsKeyDown(Key.AltRight)) //not sure about this, i likely need to poll for caps state and then toggle.          
            {
                flags = flags | CGEventFlags.Alt;
            }
            if (KeyboardState.IsKeyDown(Key.ControlLeft)||KeyboardState.IsKeyDown(Key.ControlRight)) //not sure about this, i likely need to poll for caps state and then toggle.          
            {
                flags = flags | CGEventFlags.Control;
            }
            
            if (KeyboardState.IsKeyDown(Key.WinLeft)) //not sure about this, i likely need to poll for caps state and then toggle.          
            {
                flags = flags | CGEventFlags.Command;
            }
            
            //treat winright as a function key on osx??? Do i really need to set this flag?
            if (KeyboardState.IsKeyDown(Key.WinRight)) //not sure about this, i likely need to poll for caps state and then toggle.          
            {
                flags = flags | CGEventFlags.SecondaryFn;
            }
            
            if (KeyboardState.IsKeyDown(Key.KeypadDecimal)
                ||KeyboardState.IsKeyDown(Key.KeypadPeriod)
                ||KeyboardState.IsKeyDown(Key.KeypadAdd)
                ||KeyboardState.IsKeyDown(Key.KeypadPlus)
                ||KeyboardState.IsKeyDown(Key.KeypadMinus)
                ||KeyboardState.IsKeyDown(Key.KeypadSubtract)
                ||KeyboardState.IsKeyDown(Key.KeypadMultiply)
                ||KeyboardState.IsKeyDown(Key.KeypadDivide)
                ||KeyboardState.IsKeyDown(Key.KeypadEnter)
                ||KeyboardState.IsKeyDown(Key.Keypad0)
                ||KeyboardState.IsKeyDown(Key.Keypad1)
                ||KeyboardState.IsKeyDown(Key.Keypad2)
                ||KeyboardState.IsKeyDown(Key.Keypad3)
                ||KeyboardState.IsKeyDown(Key.Keypad4)
                ||KeyboardState.IsKeyDown(Key.Keypad5)
                ||KeyboardState.IsKeyDown(Key.Keypad6)
                ||KeyboardState.IsKeyDown(Key.Keypad7)
                ||KeyboardState.IsKeyDown(Key.Keypad8)
                ||KeyboardState.IsKeyDown(Key.Keypad9))           
            {
                flags = flags | CGEventFlags.NumericPad;
            }

            return flags;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // because we can unhook only in the same thread, not in garbage collector thread
                if (_eventTap != IntPtr.Zero)
                {
                    CG.EventTapEnable(_eventTap, false);
                    CF.CFRelease(_eventTap);
                   
                    
                    _eventTap = IntPtr.Zero;

                    // ReSharper disable once DelegateSubtraction
                    _eventHookProc -= EventProc;
                }

                if (runLoop != IntPtr.Zero)
                {
                    CF.CFRelease(runLoop);
                    
                }
           
            }
        }

        ~OsxGlobalHook()
        {
            Dispose(false);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
