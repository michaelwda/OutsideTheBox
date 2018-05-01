/*
 
The MIT License (MIT)

Copyright (c) 2014 Kolibri

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
  */

using System;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable once CheckNamespace
namespace OTB.Core.Hook.Platform.Windows.Native
{
    public class Clippy
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("kernel32.dll")]
        private static extern uint GetLastError();

        [DllImport("kernel32.dll")]
        private static extern IntPtr LocalFree(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalFree(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        static extern void MoveMemory(IntPtr dest, IntPtr src, int size);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        private static extern IntPtr SetClipboardData(uint uFormat, IntPtr data);

        public enum ResultCode
        {
            Success = 0,

            ErrorOpenClipboard = 1,
            ErrorGlobalAlloc = 2,
            ErrorGlobalLock = 3,
            ErrorSetClipboardData = 4,
            ErrorOutOfMemoryException = 5,
            ErrorArgumentOutOfRangeException = 6,
            ErrorException = 7,
            ErrorInvalidArgs = 8,
            ErrorGetLastError = 9
        };

        public class Result
        {
            public ResultCode ResultCode { get; set; }

            public uint LastError { get; set; }

            public bool OK
            {
                // ReSharper disable once RedundantNameQualifier
                get { return Clippy.ResultCode.Success == ResultCode; }
            }
        }

        [STAThread]
        public static Result PushStringToClipboard(string message)
        {
            var isAscii = (message != null && (message == Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(message))));
            if (isAscii)
            {
                return PushUnicodeStringToClipboard(message);
            }
            else
            {
                return PushAnsiStringToClipboard(message);
            }
        }

        [STAThread]
        public static Result PushUnicodeStringToClipboard(string message)
        {
            return __PushStringToClipboard(message, CF_UNICODETEXT);
        }

        [STAThread]
        public static Result PushAnsiStringToClipboard(string message)
        {
            return __PushStringToClipboard(message, CF_TEXT);
        }

        // ReSharper disable InconsistentNaming
        const uint CF_TEXT = 1;
        const uint CF_UNICODETEXT = 13;
        // ReSharper restore InconsistentNaming

        [STAThread]
        private static Result __PushStringToClipboard(string message, uint format)
        {
            try
            {
                try
                {
                    if (message == null)
                    {
                        return new Result { ResultCode = ResultCode.ErrorInvalidArgs };
                    }

                    if (!OpenClipboard(IntPtr.Zero))
                    {
                        return new Result { ResultCode = ResultCode.ErrorOpenClipboard, LastError = GetLastError() };
                    }

                    try
                    {
                        uint sizeOfChar;
                        switch (format)
                        {
                            case CF_TEXT:
                                sizeOfChar = 1;
                                break;
                            case CF_UNICODETEXT:
                                sizeOfChar = 2;
                                break;
                            default:
                                throw new Exception("Not Reachable");
                        }

                        var characters = (uint)message.Length;
                        uint bytes = (characters + 1) * sizeOfChar;

                        // ReSharper disable once InconsistentNaming
                        const int GMEM_MOVABLE = 0x0002;
                        // ReSharper disable once InconsistentNaming
                        const int GMEM_ZEROINIT = 0x0040;
                        // ReSharper disable once InconsistentNaming
                        const int GHND = GMEM_MOVABLE | GMEM_ZEROINIT;

                        // IMPORTANT: SetClipboardData requires memory that was acquired with GlobalAlloc using GMEM_MOVABLE.
                        var hGlobal = GlobalAlloc(GHND, (UIntPtr)bytes);
                        if (hGlobal == IntPtr.Zero)
                        {
                            return new Result { ResultCode = ResultCode.ErrorGlobalAlloc, LastError = GetLastError() };
                        }

                        try
                        {
                            // IMPORTANT: Marshal.StringToHGlobalUni allocates using LocalAlloc with LMEM_FIXED.
                            //            Note that LMEM_FIXED implies that LocalLock / LocalUnlock is not required.
                            IntPtr source;
                            switch (format)
                            {
                                case CF_TEXT:
                                    source = Marshal.StringToHGlobalAnsi(message);
                                    break;
                                case CF_UNICODETEXT:
                                    source = Marshal.StringToHGlobalUni(message);
                                    break;
                                default:
                                    throw new Exception("Not Reachable");
                            }

                            try
                            {
                                var target = GlobalLock(hGlobal);
                                if (target == IntPtr.Zero)
                                {
                                    return new Result { ResultCode = ResultCode.ErrorGlobalLock, LastError = GetLastError() };
                                }

                                try
                                {
                                    //CopyMemory(target, source, bytes);
                                    MoveMemory(target, source, (int)bytes);
                                }
                                finally
                                {
                                    var ignore = GlobalUnlock(target);
                                }

                                if (SetClipboardData(format, hGlobal).ToInt64() != 0)
                                {
                                    // IMPORTANT: SetClipboardData takes ownership of hGlobal upon success.
                                    hGlobal = IntPtr.Zero;
                                }
                                else
                                {
                                    return new Result { ResultCode = ResultCode.ErrorSetClipboardData, LastError = GetLastError() };
                                }
                            }
                            finally
                            {
                                // Marshal.StringToHGlobalUni actually allocates with LocalAlloc, thus we should theorhetically use LocalFree to free the memory...
                                // ... but Marshal.FreeHGlobal actully uses a corresponding version of LocalFree internally, so this works, even though it doesn't
                                //  behave exactly as expected.
                                Marshal.FreeHGlobal(source);
                            }
                        }
                        catch (OutOfMemoryException)
                        {
                            return new Result { ResultCode = ResultCode.ErrorOutOfMemoryException, LastError = GetLastError() };
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            return new Result { ResultCode = ResultCode.ErrorArgumentOutOfRangeException, LastError = GetLastError() };
                        }
                        finally
                        {
                            if (hGlobal != IntPtr.Zero)
                            {
                                var ignore = GlobalFree(hGlobal);
                            }
                        }
                    }
                    finally
                    {
                        CloseClipboard();
                    }
                    return new Result { ResultCode = ResultCode.Success };
                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.Message);
                    return new Result { ResultCode = ResultCode.ErrorException, LastError = GetLastError() };
                }
            }
            catch (Exception)
            {
                return new Result { ResultCode = ResultCode.ErrorGetLastError };
            }
        }
    }
}