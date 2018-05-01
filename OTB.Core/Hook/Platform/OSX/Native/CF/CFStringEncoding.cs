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

namespace OTB.Core.Hook.Platform.OSX.Native.CF
{
    public enum CFStringEncoding
    {
        MacRoman = 0,
        WindowsLatin1 = 0x0500,
        ISOLatin1 = 0x0201,
        NextStepLatin = 0x0B01,
        ASCII = 0x0600,
        Unicode = 0x0100,
        UTF8 = 0x08000100,
        NonLossyASCII = 0x0BFF,

        UTF16 = 0x0100,
        UTF16BE = 0x10000100,
        UTF16LE = 0x14000100,
        UTF32 = 0x0c000100,
        UTF32BE = 0x18000100,
        UTF32LE = 0x1c000100
    }
}