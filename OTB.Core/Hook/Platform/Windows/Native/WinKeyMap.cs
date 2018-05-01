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

namespace OTB.Core.Hook.Platform.Windows.Native
{
    internal static class WinKeyMap
    {
        public static Key GetKey(int code)
        {
            switch (code)
            {
                // 0 - 15
                case 0: return Key.Unknown;
                case 1: return Key.Escape;
                case 2: return Key.Number1;
                case 3: return Key.Number2;
                case 4: return Key.Number3;
                case 5: return Key.Number4;
                case 6: return Key.Number5;
                case 7: return Key.Number6;
                case 8: return Key.Number7;
                case 9: return Key.Number8;
                case 10: return Key.Number9;
                case 11: return Key.Number0;
                case 12: return Key.Minus;
                case 13: return Key.Plus;
                case 14: return Key.BackSpace;
                case 15: return Key.Tab;

                // 16-31
                case 16: return Key.Q;
                case 17: return Key.W;
                case 18: return Key.E;
                case 19: return Key.R;
                case 20: return Key.T;
                case 21: return Key.Y;
                case 22: return Key.U;
                case 23: return Key.I;
                case 24: return Key.O;
                case 25: return Key.P;
                case 26: return Key.BracketLeft;
                case 27: return Key.BracketRight;
                case 28: return Key.Enter;
                case 29: return Key.ControlLeft;
                case 30: return Key.A;
                case 31: return Key.S;

                // 32 - 47
                case 32: return Key.D;
                case 33: return Key.F;
                case 34: return Key.G;
                case 35: return Key.H;
                case 36: return Key.J;
                case 37: return Key.K;
                case 38: return Key.L;
                case 39: return Key.Semicolon;
                case 40: return Key.Quote;
                case 41: return Key.Grave;
                case 42: return Key.ShiftLeft;
                case 43: return Key.BackSlash;
                case 44: return Key.Z;
                case 45: return Key.X;
                case 46: return Key.C;
                case 47: return Key.V;

                // 48 - 63
                case 48: return Key.B;
                case 49: return Key.N;
                case 50: return Key.M;
                case 51: return Key.Comma;
                case 52: return Key.Period;
                case 53: return Key.Slash;
                case 54: return Key.ShiftRight;
                case 55: return Key.PrintScreen;
                case 56: return Key.AltLeft;
                case 57: return Key.Space;
                case 58: return Key.CapsLock;
                case 59: return Key.F1;
                case 60: return Key.F2;
                case 61: return Key.F3;
                case 62: return Key.F4;
                case 63: return Key.F5;

                // 64 - 79
                case 64: return Key.F6;
                case 65: return Key.F7;
                case 66: return Key.F8;
                case 67: return Key.F9;
                case 68: return Key.F10;
                case 69: return Key.NumLock;
                case 70: return Key.ScrollLock;
                case 71: return Key.Home;
                case 72: return Key.Up;
                case 73: return Key.PageUp;
                case 74: return Key.KeypadMinus;
                case 75: return Key.Left;
                case 76: return Key.Keypad5;
                case 77: return Key.Right;
                case 78: return Key.KeypadPlus;
                case 79: return Key.End;

                // 80 - 95
                case 80: return Key.Down;
                case 81: return Key.PageDown;
                case 82: return Key.Insert;
                case 83: return Key.Delete;
                case 84: return Key.Unknown;
                case 85: return Key.Unknown;
                case 86: return Key.NonUSBackSlash;
                case 87: return Key.F11;
                case 88: return Key.F12;
                case 89: return Key.Pause;
                case 90: return Key.Unknown;
                case 91: return Key.WinLeft;
                case 92: return Key.WinRight;
                case 93: return Key.Menu;
                case 94: return Key.Unknown;
                case 95: return Key.Unknown;

                // 96 - 106
                case 96: return Key.Unknown;
                case 97: return Key.Unknown;
                case 98: return Key.Unknown;
                case 99: return Key.Unknown;
                case 100: return Key.F13;
                case 101: return Key.F14;
                case 102: return Key.F15;
                case 103: return Key.F16;
                case 104: return Key.F17;
                case 105: return Key.F18;
                case 106: return Key.F19;

                default: return Key.Unknown;
            }
        }
        public static int GetCode(Key key)
        {
            switch (key)
            {
                // 0 - 15
               case Key.Unknown: return 0;
               case Key.Escape:            return 1; 
               case Key.Number1:           return 2; 
               case Key.Number2:           return 3; 
               case Key.Number3:           return 4; 
               case Key.Number4:           return 5; 
               case Key.Number5:           return 6; 
               case Key.Number6:           return 7; 
               case Key.Number7:           return 8; 
               case Key.Number8:           return 9; 
                case Key.Number9:          return 10;
                case Key.Number0:          return 11;
                case Key.Minus:            return 12;
                case Key.Plus:             return 13;
                case Key.BackSpace:        return 14;
                case Key.Tab:              return 15;

                // 16-31
                 case Key.Q: return 16;
                case Key.W:                                                   return 17;
                 case Key.E:                                                   return 18;
                 case Key.R:                                                   return 19;
                 case Key.T:                                                   return 20;
                 case Key.Y:                                                   return 21;
                 case Key.U:                                                   return 22;
                 case Key.I:                                                   return 23;
                 case Key.O:                                                   return 24;
                 case Key.P:                                                   return 25;
                 case Key.BracketLeft:                                         return 26;
                 case Key.BracketRight:                                        return 27;
                 case Key.Enter:                                               return 28;
                 case Key.ControlLeft:                                         return 29;
                 case Key.A:                                                   return 30;
                 case Key.S:                                                   return 31;

                  // 32 - 47
                 case Key.D:                                                   return 32;
                 case Key.F:                                                   return 33;
                 case Key.G:                                                   return 34;
                 case Key.H:                                                   return 35;
                 case Key.J:                                                   return 36;
                 case Key.K:                                                   return 37;
                 case Key.L:                                                   return 38;
                 case Key.Semicolon:                                           return 39;
                 case Key.Quote:                                               return 40;
                 case Key.Grave:                                               return 41;
                 case Key.ShiftLeft:                                           return 42;
                 case Key.BackSlash:                                           return 43;
                 case Key.Z:                                                   return 44;
                 case Key.X:                                                   return 45;
                 case Key.C:                                                   return 46;
                 case Key.V:                                                   return 47;

                // 48 - 63
                 case Key.B:                                                   return 48;
                 case Key.N:                                                   return 49;
                 case Key.M:                                                   return 50;
                 case Key.Comma:                                               return 51;
                 case Key.Period:                                              return 52;
                 case Key.Slash:                                               return 53;
                 case Key.ShiftRight:                                          return 54;
                 case Key.PrintScreen:                                         return 55;
                 case Key.AltLeft:                                             return 56;
                    case Key.AltRight: return 56;
                 case Key.Space:                                               return 57;
                 case Key.CapsLock:                                            return 58;
                 case Key.F1:                                                  return 59;
                 case Key.F2:                                                  return 60;
                 case Key.F3:                                                  return 61;
                 case Key.F4:                                                  return 62;
                 case Key.F5:                                                  return 63;

                                                                               // 64 - 79
                 case Key.F6:                                                  return 64;
                 case Key.F7:                                                  return 65;
                 case Key.F8:                                                  return 66;
                 case Key.F9:                                                  return 67;
                 case Key.F10:                                                 return 68;
                 case Key.NumLock:                                             return 69;
                 case Key.ScrollLock:                                          return 70;
                 case Key.Home:                                                return 71;
                 case Key.Up:                                                  return 72;
                 case Key.PageUp:                                              return 73;
                 case Key.KeypadMinus:                                         return 74;
                 case Key.Left:                                                return 75;
                 case Key.Keypad5:                                             return 76;
                 case Key.Right:                                               return 77;
                 case Key.KeypadPlus:                                          return 78;
                 case Key.End:                                                 return 79;

                                                                               // 80 - 95
                 case Key.Down:                                                return 80;
                 case Key.PageDown:                                            return 81;
                 case Key.Insert:                                              return 82;
                 case Key.Delete:                                              return 83;
                
                 case Key.NonUSBackSlash:                                      return 86;
                 case Key.F11:                                                 return 87;
                 case Key.F12:                                                 return 88;
                 case Key.Pause:                                               return 89;
                 
                 case Key.WinLeft:                                             return 91;
                 case Key.WinRight:                                            return 92;
                 case Key.Menu:                                                return 93;
                 
                 case Key.F13: return 100;
                case Key.F14:return 101;
                 case Key.F15:return 102;
                 case Key.F16:return 103;
                 case Key.F17:return 104;
                 case Key.F18:return 105;
                 case Key.F19:return 106;

                default: return 0;
            }
        }
        public static Key TranslateKey(int scancode, VirtualKeys vkey, bool extended0, bool extended1, out bool is_valid)
        {
            is_valid = true;

            Key key = GetKey(scancode);

            if (!extended0)
            {
                switch (key)
                {
                    case Key.Insert: key = Key.Keypad0; break;
                    case Key.End: key = Key.Keypad1; break;
                    case Key.Down: key = Key.Keypad2; break;
                    case Key.PageDown: key = Key.Keypad3; break;
                    case Key.Left: key = Key.Keypad4; break;
                    case Key.Right: key = Key.Keypad6; break;
                    case Key.Home: key = Key.Keypad7; break;
                    case Key.Up: key = Key.Keypad8; break;
                    case Key.PageUp: key = Key.Keypad9; break;
                    case Key.PrintScreen: key = Key.KeypadMultiply; break;
                    case Key.Delete: key = Key.KeypadDecimal; break;
                    case Key.NumLock:
                        if (vkey == VirtualKeys.Last)
                        {
                            is_valid = false;
                        }
                        else if (vkey == VirtualKeys.PAUSE)
                        {
                            key = Key.Pause;
                        }
                        break;
                }
            }
            else
            {
                switch (key)
                {
                    case Key.Slash: key = Key.KeypadDivide; break;
                    case Key.Enter: key = Key.KeypadEnter; break;
                    case Key.AltLeft: key = Key.AltRight; break;
                    case Key.AltRight: key = Key.AltLeft; break;
                    case Key.ControlLeft: key = Key.ControlRight; break;
                    case Key.ControlRight: key = Key.ControlLeft; break;
                    case Key.ShiftLeft: is_valid = false; break;
                }
            }

            if (extended1)
            {
                switch (key)
                {
                    case Key.ControlLeft: key = Key.Pause; break;
                }
            }

            return key;
        }

        public static void ReverseTranslateKey(Key key, bool keyUp, bool isAltDown, out int tscancode, out VirtualKeys tvk, out int tflags, out bool extended)
        {

            extended = false;
            tflags = 0;
            tvk = VirtualKeys.UNKNOWN;

            if(keyUp)
                tflags = tflags | ((int)KeyFlags.KF_UP >> 8);
            if (isAltDown && key!=Key.AltLeft && key != Key.AltRight)
                tflags = tflags | ((int)KeyFlags.KF_ALTDOWN >> 8);
            switch (key)
            {

                /*
                 * The extended-key flag indicates whether the keystroke message originated from one of the additional keys on the enhanced keyboard.
                 * The extended keys consist of the ALT and CTRL keys on the right-hand side of the keyboard; the INS, DEL, HOME, END, PAGE UP, PAGE DOWN, and arrow keys in the clusters
                 * to the left of the numeric keypad; the NUM LOCK key; the BREAK (CTRL+PAUSE) key; the PRINT SCRN key; and the divide (/) and ENTER keys in the numeric keypad. The extended-key flag is set if the key is an extended key.
                 */

                //these are keys that have to be given an extended flag
                case Key.Insert: extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
                case Key.Delete: extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
                case Key.Home: extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
                case Key.End: extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
                case Key.PageUp: extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
                case Key.PageDown: extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
                case Key.Left: extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
                case Key.Right: extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
                case Key.Up: extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
                case Key.Down: extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;

                case Key.KeypadDivide: key = Key.Slash; extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
                case Key.KeypadEnter: key = Key.Enter; extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
                case Key.AltRight:
                    key = Key.AltLeft; extended = true;
                    tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8);
                    if(!keyUp)
                        tflags = tflags | ((int)KeyFlags.KF_ALTDOWN >> 8);
                    break;
                case Key.AltLeft:
                    key = Key.AltLeft; extended = false;
                    if (!keyUp)
                        tflags = tflags | ((int)KeyFlags.KF_ALTDOWN >> 8);
                    break;
                case Key.ControlRight: key = Key.ControlLeft; extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
                case Key.ControlLeft: key = Key.ControlLeft; extended = false; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
                case Key.ShiftRight:
                     key = Key.ShiftRight; extended = false; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;

                //so these keys don't get marked as extended from llhook, but when i replay with sendinput I need to indicate that they are extended
                case Key.ShiftLeft: extended = false;
                    break;


                case Key.LWin: extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
                case Key.RWin: extended = true; tflags = tflags | ((int)KeyFlags.KF_EXTENDED >> 8); break;
            }

            if (!extended)
            {
                switch (key) { 
                case Key.Keypad0: key = Key.Insert; break;
                case Key.Keypad1: key = Key.End; break;
                case Key.Keypad2: key = Key.Down; break;
                case Key.Keypad3: key = Key.PageDown; break;
                case Key.Keypad4: key = Key.Left; break;
                case Key.Keypad6: key = Key.Right; break;
                case Key.Keypad7: key = Key.Home; break;
                case Key.Keypad8: key = Key.Up; break;
                case Key.Keypad9: key = Key.PageUp; break;
                case Key.KeypadMultiply: key = Key.PrintScreen; break;
                case Key.KeypadDecimal: key = Key.Delete; break;
                case Key.Pause: tvk = VirtualKeys.PAUSE; key = Key.NumLock;break;
                }

            }

            //return our scan-code
            tscancode = GetCode(key);

              
        }
    }
}