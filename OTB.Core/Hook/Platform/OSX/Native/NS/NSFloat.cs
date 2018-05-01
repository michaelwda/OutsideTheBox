//
// NSFloat.cs
//
// Author:
//       Stefanos A. <stapostol@gmail.com>
//
// Copyright (c) 2006-2014 Stefanos Apostolopoulos
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.Drawing;


namespace OTB.Core.Hook.Platform.OSX.Native.NS
{

    public struct NSFloat
    {
        private IntPtr _value;

        public IntPtr Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public static implicit operator NSFloat(float v)
        {
            NSFloat f = new NSFloat();
            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    f.Value = *(IntPtr*)&v;
                }
                else
                {
                    double d = v;
                    f.Value = *(IntPtr*)&d;
                }
            }
            return f;
        }

        public static implicit operator NSFloat(double v)
        {
            NSFloat f = new NSFloat();
            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    float fv = (float)v;
                    f.Value = *(IntPtr*)&fv;
                }
                else
                {
                    f.Value = *(IntPtr*)&v;
                }
            }
            return f;
        }

        public static implicit operator float(NSFloat f)
        {
            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    return *(float*)&f._value;
                }
                else
                {
                    return (float)*(double*)&f._value;
                }
            }
        }

        public static implicit operator double(NSFloat f)
        {
            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    return (double)*(float*)&f._value;
                }
                else
                {
                    return *(double*)&f._value;
                }
            }
        }
    }
}