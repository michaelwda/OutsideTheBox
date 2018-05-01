using OTB.Core.Hook.Platform.OSX.Native.NS;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OTB.Core.Hook.Platform.OSX.Native
{
    public static class Cocoa
    {

        public static IntPtr ToNSString(string str)
        {
            if (str == null)
            {
                return IntPtr.Zero;
            }

            unsafe
            {
                fixed (char* ptrFirstChar = str)
                {
                    var handle = Cocoa.SendIntPtr(Cocoa.objc_getClass("NSString"), Selector.Get("alloc"));
                    handle = Cocoa.SendIntPtr(handle, Selector.Get("initWithCharacters:length:"), (IntPtr)ptrFirstChar, str.Length);
                    return handle;
                }
            }
        }

        [DllImport(Frameworks.CocoaFramework, EntryPoint = "objc_msgSend")]
        public extern static int SendInt(IntPtr receiver, IntPtr selector);


        [DllImport(Frameworks.CocoaFramework, EntryPoint = "objc_msgSend")]
        public extern static void SendVoid(IntPtr receiver, IntPtr selector);

        [DllImport(Frameworks.CocoaFramework, EntryPoint = "objc_msgSend")]
        public extern static void SendVoid(IntPtr receiver, IntPtr selector, uint uint1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint = "objc_msgSend")]
        public extern static void SendVoid(IntPtr receiver, IntPtr selector, uint uint1, IntPtr intPtr1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint = "objc_msgSend")]
        public extern static void SendVoid(IntPtr receiver, IntPtr selector, IntPtr intPtr1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint = "objc_msgSend")]
        public extern static void SendVoid(IntPtr receiver, IntPtr selector, IntPtr intPtr1, int int1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint = "objc_msgSend")]
        public extern static void SendVoid(IntPtr receiver, IntPtr selector, IntPtr intPtr1, IntPtr intPtr2);

        [DllImport(Frameworks.CocoaFramework, EntryPoint = "objc_msgSend")]
        public extern static void SendVoid(IntPtr receiver, IntPtr selector, int int1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint = "objc_msgSend")]
        public extern static void SendVoid(IntPtr receiver, IntPtr selector, bool bool1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint = "objc_msgSend")]
        public extern static void SendVoid(IntPtr receiver, IntPtr selector, NSPoint point1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint = "objc_msgSend")]
        public extern static void SendVoid(IntPtr receiver, IntPtr selector, NSRect rect1, bool bool1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint = "objc_msgSend")]
        public extern static void SendVoid(IntPtr receiver, IntPtr selector, NSRect rect1, IntPtr intPtr1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector);

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, ulong ulong1);

 

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, NSSize size);

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, IntPtr intPtr1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, IntPtr intPtr1, int int1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, IntPtr intPtr1, IntPtr intPtr2);

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, IntPtr intPtr1, IntPtr intPtr2, IntPtr intPtr3);

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, IntPtr intPtr1, IntPtr intPtr2, IntPtr intPtr3, IntPtr intPtr4, IntPtr intPtr5);

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, IntPtr p1, NSPoint p2);

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, bool p1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, NSPoint p1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, NSRect rectangle1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, NSRect rectangle1, int int1, int int2, bool bool1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, uint uint1, IntPtr intPtr1, IntPtr intPtr2, bool bool1);

        [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, NSRect rectangle1, int int1, IntPtr intPtr1, IntPtr intPtr2);

       /* [DllImport(Frameworks.CocoaFramework, EntryPoint="objc_msgSend")]
        public extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, IntPtr p1, int p2, int p3, int p4, int p5, int p6, int p7, IntPtr p8, NSBitmapFormat p9, int p10, int p11);
*/
        
        
        [DllImport(Frameworks.CocoaFramework, CharSet = CharSet.Ansi)]
        public static extern IntPtr objc_getClass(string name);

        [DllImport (Frameworks.CocoaFramework, EntryPoint="objc_msgSend_stret")]
        private extern static void SendRect(out NSRect retval, IntPtr receiver, IntPtr selector);

        [DllImport (Frameworks.CocoaFramework, EntryPoint="objc_msgSend_stret")]
        private extern static void SendRect(out NSRect retval, IntPtr receiver, IntPtr selector, NSRect rect1);

        public static NSRect SendRect(IntPtr receiver, IntPtr selector)
        {
            NSRect r;
            SendRect(out r, receiver, selector);
            return r;
        }

        public static NSRect SendRect(IntPtr receiver, IntPtr selector, NSRect rect1)
        {
            NSRect r;
            SendRect(out r, receiver, selector, rect1);
            return r;
        }

       
        // Not the _stret version, perhaps because a NSPoint fits in one register?
        // thefiddler: gcc is indeed using objc_msgSend for NSPoint on i386
        [DllImport(Frameworks.CocoaFramework, EntryPoint = "objc_msgSend")]
        public extern static NSPointF SendPointF(IntPtr receiver, IntPtr selector);
        [DllImport(Frameworks.CocoaFramework, EntryPoint = "objc_msgSend")]
        public extern static NSPointD SendPointD(IntPtr receiver, IntPtr selector);

        public static NSPoint SendPoint(IntPtr receiver, IntPtr selector)
        {
            NSPoint r = new NSPoint();

            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    NSPointF pf = SendPointF(receiver, selector);
                    r.X.Value = *(IntPtr*)&pf.X;
                    r.Y.Value = *(IntPtr*)&pf.Y;
                }
                else
                {
                    NSPointD pd = SendPointD(receiver, selector);
                    r.X.Value = *(IntPtr*)&pd.X;
                    r.Y.Value = *(IntPtr*)&pd.Y;
                }
            }

            return r;
        }


    }
}
