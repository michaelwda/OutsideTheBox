//
// The Open Toolkit Library License
//
// Copyright (c) 2006 - 2010 the Open Toolkit library.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OTB.Core.Hook.Platform.OSX.Native.CF
{
    using CFAllocatorRef = IntPtr;
    using CFIndex = System.IntPtr;
    using CFRunLoop = System.IntPtr;
    using CFRunLoopRef = IntPtr;
    using CFRunLoopSourceRef = IntPtr;
    using CFStringRef = System.IntPtr;
    using CFTypeRef = System.IntPtr;
    using CFMachPortRef = IntPtr;

    public class CF
    {

        [DllImport(Frameworks.ApplicationServices)]
        internal static extern int CFArrayGetCount(IntPtr theArray);

        [DllImport(Frameworks.ApplicationServices)]
        internal static extern IntPtr CFArrayGetValueAtIndex(IntPtr theArray, int idx);

        [DllImport(Frameworks.ApplicationServices)]
        internal static extern int CFDictionaryGetCount(IntPtr theDictionary);

        [DllImport(Frameworks.ApplicationServices)]
        internal static extern IntPtr CFDictionaryGetValue(IntPtr theDictionary, IntPtr theKey);

        [DllImport(Frameworks.ApplicationServices)]
        internal static extern IntPtr CFGetTypeID(IntPtr v);

        [DllImport(Frameworks.ApplicationServices)]
        public static extern IntPtr CFRetain(CFTypeRef cf);

        [DllImport(Frameworks.ApplicationServices)]
        public static extern void CFRelease(CFTypeRef cf);

        // this mirrors the definition in CFString.h.
        // I don't know why, but __CFStringMakeConstantString is marked as "private and should not be used directly"
        // even though the CFSTR macro just calls it.
        [DllImport(Frameworks.ApplicationServices)]
        private static extern IntPtr __CFStringMakeConstantString(string cStr);
        internal static IntPtr CFSTR(string cStr)
        {
            return __CFStringMakeConstantString(cStr);
        }

        [DllImport(Frameworks.ApplicationServices)]
        internal static extern Boolean CFStringGetCString(
            CFStringRef theString,
            byte[] buffer,
            CFIndex bufferSize,
            CFStringEncoding encoding
        );

        internal static string CFStringGetCString(IntPtr cfstr)
        {
            CFIndex length = CFStringGetLength(cfstr);
            if (length != IntPtr.Zero)
            {
                byte[] utf8_chars = new byte[length.ToInt32() + 1];
                if (CFStringGetCString(cfstr, utf8_chars, new IntPtr(utf8_chars.Length), CFStringEncoding.UTF8))
                {
                    return Encoding.UTF8.GetString(utf8_chars);
                }
            }
            return String.Empty;
        }

        [DllImport(Frameworks.ApplicationServices)]
        internal static extern CFIndex CFStringGetLength(
            CFStringRef theString
        );

        [DllImport(Frameworks.ApplicationServices)]
        internal static extern bool CFNumberGetValue (IntPtr number, CFNumberType theType, out int valuePtr);
        [DllImport(Frameworks.ApplicationServices)]
        internal static extern bool CFNumberGetValue (IntPtr number, CFNumberType theType, out long valuePtr);
        [DllImport(Frameworks.ApplicationServices)]
        internal static extern bool CFNumberGetValue(IntPtr number, CFNumberType theType, out double valuePtr);

        internal enum CFNumberType
        {
            kCFNumberSInt8Type = 1,
            kCFNumberSInt16Type = 2,
            kCFNumberSInt32Type = 3,
            kCFNumberSInt64Type = 4,
            kCFNumberFloat32Type = 5,
            kCFNumberFloat64Type = 6,
            kCFNumberCharType = 7,
            kCFNumberShortType = 8,
            kCFNumberIntType = 9,
            kCFNumberLongType = 10,
            kCFNumberLongLongType = 11,
            kCFNumberFloatType = 12,
            kCFNumberDoubleType = 13,
            kCFNumberCFIndexType = 14,
            kCFNumberNSIntegerType = 15,
            kCFNumberCGFloatType = 16,
            kCFNumberMaxType = 16
        };

        public static readonly IntPtr RunLoopModeDefault = CF.CFSTR("kCFRunLoopDefaultMode");

        [DllImport(Frameworks.ApplicationServices)]
        internal static extern CFRunLoop CFRunLoopGetCurrent();

        [DllImport(Frameworks.ApplicationServices)]
        internal static extern CFRunLoop CFRunLoopGetMain();

        [DllImport(Frameworks.ApplicationServices)]
        internal static extern CFRunLoopExitReason CFRunLoopRunInMode(
            IntPtr cfstrMode, double interval, bool returnAfterSourceHandled);

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CFMachPortCreateRunLoopSource")]
        internal static extern CFRunLoopSourceRef MachPortCreateRunLoopSource(
            CFAllocatorRef allocator,
            CFMachPortRef port,
            CFIndex order);

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CFRunLoopAddSource")]
        internal static extern void RunLoopAddSource(
            CFRunLoopRef rl,
            CFRunLoopSourceRef source,
            CFStringRef mode);

        [DllImport(Frameworks.ApplicationServices, EntryPoint = "CFRunLoopRemoveSource")]
        internal static extern void RunLoopRemoveSource(
            CFRunLoopRef rl,
            CFRunLoopSourceRef source,
            CFStringRef mode);
    }
}