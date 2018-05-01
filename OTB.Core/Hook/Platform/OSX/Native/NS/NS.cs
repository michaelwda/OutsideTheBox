using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OTB.Core.Hook.Platform.OSX.Native.NS
{
    public static class NS
    {

        [DllImport(Frameworks.CocoaFramework)]
        public static extern IntPtr NSSelectorFromString(IntPtr cfstr);
    }
}
