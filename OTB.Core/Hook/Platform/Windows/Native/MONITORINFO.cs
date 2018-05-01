using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OTB.Core.Hook.Platform.Windows.Native
{


    /// <summary>
    /// Monitor information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MONITORINFO
    {
        public uint size;
        public Rect monitor;
        public Rect work;
        public uint flags;
    }
}
