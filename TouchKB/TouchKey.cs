using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace TouchKB
{
    class TouchKey
    {
        private Point pos_;
        private Keys key_;
        private int scanCode_;
        private string[] text_ = new string[8];

        public Point Position { get { return pos_; } }
        public Keys Key { get { return key_; } }
        public int ScanCode { get { return scanCode_; } }
        public Rectangle TextRect { get { return GetTextRect(); } }

        public delegate void AssociatedAction( TouchKey Key );

        public AssociatedAction KeyDownAction = null;
        public AssociatedAction KeyUpAction = null;

        public TouchKey( Point Position, int ScanCode, InputLanguage InLang )
        {
            pos_ = Position;
            scanCode_ = ScanCode;
            key_ = ScanCodeToVk( ScanCode );
            text_[0] = GetKeyString(key_, false, false, false, InLang);
            text_[1] = GetKeyString(key_, true, false, false, InLang);
            text_[2] = GetKeyString(key_, false, true, false, InLang);
            text_[3] = GetKeyString(key_, true, true, false, InLang);
            text_[4] = GetKeyString(key_, false, false, true, InLang);
            text_[5] = GetKeyString(key_, true, false, true, InLang);
            text_[6] = GetKeyString(key_, false, true, true, InLang);
            text_[7] = GetKeyString(key_, true, true, true, InLang);
        }

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode,
           byte[] lpKeyState, UInt16[] pwszBuff,
           int cchBuff, uint wFlags, IntPtr dwhkl);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int GetKeyboardState(byte[] keystate);

        private char GetKeyChar(Keys key, bool ShiftPressed, bool AltGrPressed, bool CapsLockPressed, InputLanguage InLng)
        {
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);
            keyboardState[(int)Keys.ShiftKey] ^= ShiftPressed ? (byte)0x80 : (byte)0;
            keyboardState[(int)Keys.CapsLock] ^= CapsLockPressed ? (byte)0x01 : (byte)0;
            keyboardState[(int)Keys.Menu] ^= AltGrPressed ? (byte)0x80 : (byte)0;
            keyboardState[(int)Keys.ControlKey] ^= AltGrPressed ? (byte)0x80 : (byte)0;
            UInt16[] b = new UInt16[4];
            ToUnicodeEx((uint)key, 0, keyboardState, b, b.Length, 0, InLng.Handle);
            switch (ToUnicodeEx((uint)key, 0, keyboardState, b, b.Length, 0, InLng.Handle))
            {
                case 0:
                    return (char)0;
                default:
                    return b[0] != 0 ? Convert.ToChar(b[0]) : (char)0;
            }
        }

        private string GetKeyString(Keys key, bool ShiftPressed, bool AltGrPressed, bool CapsLockPressed, InputLanguage InLng)
        {
            char c = GetKeyChar(key, ShiftPressed, AltGrPressed, CapsLockPressed, InLng);
            if (c != (char)0)
            {
                if (!Char.IsControl(c))
                    return c.ToString();
            }
            return "";
        }
        
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int MapVirtualKey(int wCode, int wMapType);

        public Keys ScanCodeToVk(int ScanCode)
        {
            return (Keys)MapVirtualKey(ScanCode, 1);
        }

        protected virtual Rectangle GetTextRect()
        {
            return new Rectangle(3, 3, 54, 54);;
        }

        public virtual Bitmap GetReleasedBmp(bool Locked)
        {
            return DoGetReleasedBmp(Locked);
        }

        public virtual Bitmap GetPressedBmp(bool Locked)
        {
            return DoGetPressedBmp(Locked);
        }

        protected virtual Bitmap DoGetReleasedBmp(bool Locked)
        {
            return null;
        }

        protected virtual Bitmap DoGetPressedBmp(bool Locked)
        {
            return null;
        }

        private int GetTextIndex(bool ShiftPressed, bool AltGrPressed, bool CapsLockPressed)
        {
            return (ShiftPressed ? 1 : 0) +
                   (AltGrPressed ? 2 : 0) +
                   (CapsLockPressed ? 4 : 0);
        }

        public string GetKeyText(bool ShiftPressed, bool AltGrPressed, bool CapsLockPressed)
        {
            return text_[GetTextIndex(ShiftPressed, AltGrPressed, CapsLockPressed)];
        }
    }
}
