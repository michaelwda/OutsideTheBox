// Author:
//       Stefanos A. <stapostol@gmail.com>
//
// Copyright (c) 2006-2013 Stefanos Apostolopoulos
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

namespace OTB.Core.Hook.Platform
{
    public abstract class Image : IDisposable
    {
        public void Dispose() { }

        internal void Save(System.IO.Stream s, ImageFormat fomat)
        {
        }
    }

    public sealed class Bitmap : Image
    {
        private int width;
        private int height;

        public Bitmap() { }

        public Bitmap(int width, int height)
        {
            // TODO: Complete member initialization
            this.width = width;
            this.height = height;
        }

        internal Bitmap(int width, int height, int stride, PixelFormat format, IntPtr pixels)
        {
            // TODO: Complete member initialization
            this.width = width;
            this.height = height;
        }

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public Color GetPixel(int x, int y)
        {
            return new Color();
        }

        internal void UnlockBits(BitmapData data)
        {
        }

        internal BitmapData LockBits(Rectangle rectangle, ImageLockMode imageLockMode, PixelFormat pixelFormat)
        {
            return new BitmapData(Width, Height, 0);
        }
        internal sealed class BitmapData
        {
            internal BitmapData(int width, int height, int stride)
            {
                Width = width;
                Height = height;
                Stride = stride;
            }

            public IntPtr Scan0 { get { return IntPtr.Zero; } }
            public int Width { get; private set; }
            public int Height { get; private set; }
            public int Stride { get; private set; }
        }

        internal static int GetPixelFormatSize (PixelFormat format)
        {
            return 0;
        }

        internal IntPtr GetHicon ()
        {
            return IntPtr.Zero;
        }
    }
    internal enum ImageLockMode
    {
        ReadOnly,
        WriteOnly,
        ReadWrite,
        UserInputBuffer
    }

    internal enum PixelFormat
    {
        Format32bppArgb
    }
    
    internal enum ImageFormat {
        Png
    }
}