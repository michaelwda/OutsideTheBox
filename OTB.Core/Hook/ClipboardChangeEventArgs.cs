using System;

namespace OTB.Core.Hook
{
    public class ClipboardChangedEventArgs : EventArgs
    {
        public string Value { get; set; }
    }
}