using OTB.Core.Hook.Platform.OSX.Native.NS;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OTB.Core.Hook.Platform.OSX.Native.CG
{
    using CGDirectDisplayID = System.IntPtr;
    using CGEventTapProxy = IntPtr;
    using CGEventRef = IntPtr;
    using CFMachPortRef = IntPtr;
    public static class CG
    {
        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGGetActiveDisplayList")]
        internal unsafe static extern CGDisplayErr GetActiveDisplayList(int maxDisplays, IntPtr* activeDspys, out int dspyCnt);

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGMainDisplayID")]
        internal static extern IntPtr MainDisplayID();

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGWarpMouseCursorPosition")]
        internal static extern CGError WarpMouseCursorPosition(NSPoint newCursorPosition);

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGCursorIsVisible")]
        internal static extern bool CursorIsVisible();

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGDisplayShowCursor")]
        internal static extern CGError DisplayShowCursor(CGDirectDisplayID display);

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGDisplayHideCursor")]
        internal static extern CGError DisplayHideCursor(CGDirectDisplayID display);

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGAssociateMouseAndMouseCursorPosition")]
        internal static extern CGError AssociateMouseAndMouseCursorPosition(bool connected);

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGSetLocalEventsSuppressionInterval")]
        internal static extern CGError SetLocalEventsSuppressionInterval(double seconds);
        
        
       [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate CGEventRef EventTapCallBack(
            CGEventTapProxy proxy,
            CGEventType type,
            CGEventRef @event,
            IntPtr refcon);

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGEventTapCreate")]
        public static extern CFMachPortRef EventTapCreate(
            CGEventTapLocation tap,
            CGEventTapPlacement place,
            CGEventTapOptions options,
            CGEventMask eventsOfInterest,
            [MarshalAs(UnmanagedType.FunctionPtr)]
            EventTapCallBack callback,
            IntPtr refcon);

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGEventTapCreate")]
        public static extern void EventTapEnable(CFMachPortRef tap, bool enable);

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGEventGetDoubleValueField")]
        internal static extern double EventGetDoubleValueField(
            CGEventRef @event,
            CGEventField field);

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGEventGetIntegerValueField")]
        internal static extern int EventGetIntegerValueField(
            CGEventRef @event,
            CGEventField field);
        
        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGEventSetIntegerValueField")]
        internal static extern void EventSetIntegerValueField(
            CGEventRef @event,
            CGEventField field, int value);

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGEventGetLocation")]
        internal static extern NSPointF EventGetLocationF(CGEventRef @event);
        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGEventGetLocation")]
        internal static extern NSPointD EventGetLocationD(CGEventRef @event);

        internal static NSPoint EventGetLocation(CGEventRef @event)
        {
            NSPoint r = new NSPoint();

            unsafe {
                if (IntPtr.Size == 4)
                {
                    NSPointF pf = EventGetLocationF(@event);
                    r.X.Value = *(IntPtr *)&pf.X;
                    r.Y.Value = *(IntPtr *)&pf.Y;
                }
                else
                {
                    NSPointD pd = EventGetLocationD(@event);
                    r.X.Value = *(IntPtr *)&pd.X;
                    r.Y.Value = *(IntPtr *)&pd.Y;
                }
            }

            return r;
        }
        
        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGEventCreateMouseEvent")]
        internal static extern CGEventRef CGEventCreateMouseEvent(IntPtr source, CGEventType mouseType, CGPoint mouseCursorPosition, CGMouseButton mouseButton);
        
        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGEventCreateScrollWheelEvent")]
        internal static extern CGEventRef CGEventCreateScrollWheelEvent(IntPtr source, CGScrollEventUnit units, 
            int wheelCount, int wheelX, int wheelY);
        
        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGEventCreateKeyboardEvent")]
        internal static extern CGEventRef CGEventCreateKeyboardEvent(IntPtr source, ushort virtualKey, bool keyDown);

        
        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGEventPost")]
        internal static extern void CGEventPost(CGEventTapLocation tap, CGEventRef @event);
        
        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGEventSetFlags")]
        internal static extern void CGEventSetFlags(CGEventRef @event, CGEventFlags flags);
        
        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CGEventGetFlags")]
        internal static extern CGEventFlags CGEventGetFlags(CGEventRef @event);
    }

    /*
     * #define NX_ALPHASHIFTMASK                   0x00010000
#define NX_SHIFTMASK                        0x00020000
#define NX_CONTROLMASK                      0x00040000
#define NX_ALTERNATEMASK                    0x00080000
#define NX_COMMANDMASK                      0x00100000
#define NX_NUMERICPADMASK                   0x00200000
#define NX_HELPMASK                         0x00400000
#define NX_SECONDARYFNMASK                  0x00800000
#define NX_ALPHASHIFT_STATELESS_MASK        0x01000000
*/
     
    [Flags]
    public enum CGEventFlags
    {
        CapsLock=0x00010000,
        Shift = 0x00020000,
        Control = 0x00040000,
        Alt = 0x00080000,
        Command = 0x00100000,
        NumericPad=0x00200000,
        Help = 0x00400000,
        SecondaryFn=0x00800000,
        NonCoalesced = 0x00000100,
        
    }
    
    public enum CGScrollEventUnit{
        UnitPixel=0,
        UnitLine
    }

    public enum CGEventTapLocation
    {
        HIDEventTap = 0,
        SessionEventTap,
        AnnotatedSessionEventTap
    }

    public enum CGMouseButton
    {
        Left=0,
        Right,
        Center
    }

    public enum CGEventTapPlacement
    {
        HeadInsert = 0,
        TailAppend
    }

    public enum CGEventTapOptions
    {
        Default = 0x00000000,
        ListenOnly = 0x00000001
    }

    [Flags]
    public enum CGEventMask : long
    {
        LeftMouseDown       = 1 << CGEventType.LeftMouseDown,
        LeftMouseUp         = 1 << CGEventType.LeftMouseUp,
        RightMouseDown      = 1 << CGEventType.RightMouseDown,
        RightMouseUp        = 1 << CGEventType.RightMouseUp,
        MouseMoved          = 1 << CGEventType.MouseMoved,
        LeftMouseDragged    = 1 << CGEventType.LeftMouseDown,
        RightMouseDragged   = 1 << CGEventType.RightMouseDown,
        KeyDown             = 1 << CGEventType.KeyDown,
        KeyUp               = 1 << CGEventType.KeyUp,
        FlagsChanged        = 1 << CGEventType.FlagsChanged,
        ScrollWheel         = 1 << CGEventType.ScrollWheel,
        TabletPointer       = 1 << CGEventType.TabletPointer,
        TabletProximity     = 1 << CGEventType.TabletProximity,
        OtherMouseDown      = 1 << CGEventType.OtherMouseDown,
        OtherMouseUp        = 1 << CGEventType.OtherMouseUp,
        OtherMouseDragged   = 1 << CGEventType.OtherMouseDragged,
        All = -1,
        AllMouse =
            LeftMouseDown | LeftMouseUp | LeftMouseDragged |
            RightMouseDown | RightMouseUp | RightMouseDragged |
            OtherMouseDown | OtherMouseUp | OtherMouseDragged |
            ScrollWheel | MouseMoved
    }

    public enum CGEventType
    {
        Null                = 0,
        LeftMouseDown       = 1,
        LeftMouseUp         = 2,
        RightMouseDown      = 3,
        RightMouseUp        = 4,
        MouseMoved          = 5,
        LeftMouseDragged    = 6,
        RightMouseDragged   = 7,
        KeyDown             = 10,
        KeyUp               = 11,
        FlagsChanged        = 12,
        ScrollWheel         = 22,
        TabletPointer       = 23,
        TabletProximity     = 24,
        OtherMouseDown      = 25,
        OtherMouseUp        = 26,
        OtherMouseDragged   = 27,
        TapDisabledByTimeout = -2,
        TapDisabledByUserInput = -1
    }

    internal enum CGEventField
    {
        MouseEventNumber = 0,
        MouseEventClickState = 1,
        MouseEventPressure = 2,
        MouseEventButtonNumber = 3,
        MouseEventDeltaX = 4,
        MouseEventDeltaY = 5,
        MouseEventInstantMouser = 6,
        MouseEventSubtype = 7,
        KeyboardEventAutorepeat = 8,
        KeyboardEventKeycode = 9,
        KeyboardEventKeyboardType = 10,
        ScrollWheelEventDeltaAxis1 = 11,
        ScrollWheelEventDeltaAxis2 = 12,
        ScrollWheelEventDeltaAxis3 = 13,
        ScrollWheelEventFixedPtDeltaAxis1 = 93,
        ScrollWheelEventFixedPtDeltaAxis2 = 94,
        ScrollWheelEventFixedPtDeltaAxis3 = 95,
        ScrollWheelEventPointDeltaAxis1 = 96,
        ScrollWheelEventPointDeltaAxis2 = 97,
        ScrollWheelEventPointDeltaAxis3 = 98,
        ScrollWheelEventInstantMouser = 14,
        TabletEventPointX = 15,
        TabletEventPointY = 16,
        TabletEventPointZ = 17,
        TabletEventPointButtons = 18,
        TabletEventPointPressure = 19,
        TabletEventTiltX = 20,
        TabletEventTiltY = 21,
        TabletEventRotation = 22,
        TabletEventTangentialPressure = 23,
        TabletEventDeviceID = 24,
        TabletEventVendor1 = 25,
        TabletEventVendor2 = 26,
        TabletEventVendor3 = 27,
        TabletProximityEventVendorID = 28,
        TabletProximityEventTabletID = 29,
        TabletProximityEventPointerID = 30,
        TabletProximityEventDeviceID = 31,
        TabletProximityEventSystemTabletID = 32,
        TabletProximityEventVendorPointerType = 33,
        TabletProximityEventVendorPointerSerialNumber = 34,
        TabletProximityEventVendorUniqueID = 35,
        TabletProximityEventCapabilityMask = 36,
        TabletProximityEventPointerType = 37,
        TabletProximityEventEnterProximity = 38,
        EventTargetProcessSerialNumber = 39,
        EventTargetUnixProcessID = 40,
        EventSourceUnixProcessID = 41,
        EventSourceUserData = 42,
        EventSourceUserID = 43,
        EventSourceGroupID = 44,
        EventSourceStateID = 45,
        ScrollWheelEventIsContinuous = 88
    }
    
    internal struct CGPoint {
        public double x;
        public double y;
    }
}

