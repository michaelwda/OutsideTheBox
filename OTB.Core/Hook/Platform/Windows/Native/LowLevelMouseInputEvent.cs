using System;
using System.Runtime.InteropServices;

namespace OTB.Core.Hook.Platform.Windows.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LowLevelMouseInputEvent
    {

        public Point Point;

        public uint Mousedata;

        public uint Flags;

        public uint Time;

        public IntPtr AdditionalInformation;

    }
}