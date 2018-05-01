using System;
using System.Collections.Generic;
using System.Text;

namespace OTB.Core.Hook.Platform.Windows.Native
{
    [Flags]
    public enum KeyFlags : int
    {
        KF_EXTENDED = 0x0100,
        KF_DLGMODE = 0x0800,
        KF_MENUMODE = 0x1000,
        KF_ALTDOWN = 0x2000,
        KF_REPEAT = 0x4000,
        KF_UP = 0x8000
    }
}
