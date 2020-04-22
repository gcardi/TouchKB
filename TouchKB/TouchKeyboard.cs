using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Resources;
//using System.Diagnostics;

namespace TouchKB
{
    public partial class TouchKeyboard : UserControl
    {
        private Bitmap pressedKeyBmp_ = null;
        private Bitmap releasedKeyBmp_ = null;
        private int hKeySpace_ = 0;
        private int vKeySpace_ = 9;
        private int[] xOfs_ = new int[] { 18, 54, 18, 0 };
        private int yOfs_ = 94;
        private TouchKey pressedkey_ = null;
        private Point pressedOffset_ = new Point(2, 2);
        private List<TouchKey> keys_ = new List<TouchKey>();
        private Rectangle textRect_ = new Rectangle(22, 25, 914, 44);
        private bool shiftPressed_ = false;
        private bool capsLockActive_ = Control.IsKeyLocked(Keys.CapsLock);
        private bool altGrPressed_ = false;
        private StringFormat defStringFormat_ = GetTextStringFormat();
        private Keys lastKeyData_ = Keys.None;
        private int caretPos_ = 0;

        private int CaretPosition { get { return caretPos_; } set { SetCaretPos( value ); } }
       
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
         Bindable(true)]
        public override string Text { get { return base.Text; } set { SetText(value); } }

        public TouchKeyboard()
        {
            InitializeComponent();

            DoubleBuffered = true;

            Font = ChangeFontSize( Font, 28 );

            CreateKeyboard();
        }

        public void RebuildKeyboard()
        {
            DestroyKeyboard();
            CreateKeyboard();
        }

        private void CreateKeyboard()
        {
            pressedKeyBmp_ = Properties.Resources.KeyPressed;

            releasedKeyBmp_ = Properties.Resources.KeyReleased;

            int HSpc = Math.Max(pressedKeyBmp_.Width, releasedKeyBmp_.Width) + hKeySpace_;
            int VSpc = Math.Max(pressedKeyBmp_.Height, releasedKeyBmp_.Height) + vKeySpace_;
            
            InputLanguage InLang = InputLanguage.CurrentInputLanguage;

            keys_.Add(new TouchKey(new Point(xOfs_[0] + 0 * HSpc, yOfs_ + 0 * VSpc), 41, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[0] + 1 * HSpc, yOfs_ + 0 * VSpc), 2, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[0] + 2 * HSpc, yOfs_ + 0 * VSpc), 3, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[0] + 3 * HSpc, yOfs_ + 0 * VSpc), 4, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[0] + 4 * HSpc, yOfs_ + 0 * VSpc), 5, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[0] + 5 * HSpc, yOfs_ + 0 * VSpc), 6, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[0] + 6 * HSpc, yOfs_ + 0 * VSpc), 7, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[0] + 7 * HSpc, yOfs_ + 0 * VSpc), 8, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[0] + 8 * HSpc, yOfs_ + 0 * VSpc), 9, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[0] + 9 * HSpc, yOfs_ + 0 * VSpc), 10, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[0] + 10 * HSpc, yOfs_ + 0 * VSpc), 11, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[0] + 11 * HSpc, yOfs_ + 0 * VSpc), 12, InLang)); 
            keys_.Add(new TouchKey(new Point(xOfs_[0] + 12 * HSpc, yOfs_ + 0 * VSpc), 13, InLang)); 
            keys_.Add(
                new GraphicTouchKey(
                    new Point(xOfs_[0] + 13 * HSpc, yOfs_ + 0 * VSpc),
                    14, InLang,
                    Properties.Resources.BackspaceKeyReleased,
                    Properties.Resources.BackspaceKeyPressed
                )
            );
            keys_.Last().KeyDownAction = OnBackKeyDown;

            keys_.Add(new TouchKey(new Point(xOfs_[1] + 1 * HSpc, yOfs_ + 1 * VSpc), 16, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[1] + 2 * HSpc, yOfs_ + 1 * VSpc), 17, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[1] + 3 * HSpc, yOfs_ + 1 * VSpc), 18, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[1] + 4 * HSpc, yOfs_ + 1 * VSpc), 19, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[1] + 5 * HSpc, yOfs_ + 1 * VSpc), 20, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[1] + 6 * HSpc, yOfs_ + 1 * VSpc), 21, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[1] + 7 * HSpc, yOfs_ + 1 * VSpc), 22, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[1] + 8 * HSpc, yOfs_ + 1 * VSpc), 23, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[1] + 9 * HSpc, yOfs_ + 1 * VSpc), 24, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[1] + 10 * HSpc, yOfs_ + 1 * VSpc), 25, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[1] + 11 * HSpc, yOfs_ + 1 * VSpc), 26, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[1] + 12  * HSpc, yOfs_ + 1 * VSpc), 27, InLang));

            keys_.Add(
                new LockableTouchKey(
                    new Point(xOfs_[2] + 0 * HSpc, yOfs_ + 2 * VSpc),
                    58, InLang, new Rectangle(3, 3, 114, 54), 
                    Properties.Resources.CapsLockKeyMedReleased,
                    Properties.Resources.CapsLockKeyMedPressed,
                    Properties.Resources.CapsLockActiveKeyMedReleased,
                    Properties.Resources.CapsLockActiveKeyMedPressed
                )
            );
            keys_.Last().KeyDownAction = OnCapsLockKeyDown;

            keys_.Add(new TouchKey(new Point(xOfs_[2] + 2 * HSpc, yOfs_ + 2 * VSpc), 30, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[2] + 3 * HSpc, yOfs_ + 2 * VSpc), 31, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[2] + 4 * HSpc, yOfs_ + 2 * VSpc), 32, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[2] + 5 * HSpc, yOfs_ + 2 * VSpc), 33, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[2] + 6 * HSpc, yOfs_ + 2 * VSpc), 34, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[2] + 7 * HSpc, yOfs_ + 2 * VSpc), 35, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[2] + 8 * HSpc, yOfs_ + 2 * VSpc), 36, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[2] + 9 * HSpc, yOfs_ + 2 * VSpc), 37, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[2] + 10 * HSpc, yOfs_ + 2 * VSpc), 38, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[2] + 11 * HSpc, yOfs_ + 2 * VSpc), 39, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[2] + 12 * HSpc, yOfs_ + 2 * VSpc), 40, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[2] + 13 * HSpc, yOfs_ + 2 * VSpc), 43, InLang));

            keys_.Add(
                new LockableTouchKey(
                    new Point(xOfs_[3] + 1 * HSpc, yOfs_ + 3 * VSpc), 
                    42, InLang,
                    Properties.Resources.ShiftKeyReleased,
                    Properties.Resources.ShiftKeyPressed,
                    Properties.Resources.ShiftActiveKeyReleased,
                    Properties.Resources.ShiftActiveKeyPressed
                )
            );
            keys_.Last().KeyDownAction = OnShiftKeyDown;

            keys_.Add(new TouchKey(new Point(xOfs_[3] +  2 * HSpc, yOfs_ + 3 * VSpc), 86, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[3] +  3 * HSpc, yOfs_ + 3 * VSpc), 44, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[3] +  4 * HSpc, yOfs_ + 3 * VSpc), 45, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[3] +  5 * HSpc, yOfs_ + 3 * VSpc), 46, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[3] +  6 * HSpc, yOfs_ + 3 * VSpc), 47, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[3] +  7 * HSpc, yOfs_ + 3 * VSpc), 48, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[3] +  8 * HSpc, yOfs_ + 3 * VSpc), 49, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[3] +  9 * HSpc, yOfs_ + 3 * VSpc), 50, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[3] + 10 * HSpc, yOfs_ + 3 * VSpc), 51, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[3] + 11 * HSpc, yOfs_ + 3 * VSpc), 52, InLang));
            keys_.Add(new TouchKey(new Point(xOfs_[3] + 12  * HSpc, yOfs_ + 3 * VSpc), 53, InLang));
            keys_.Add(
                new LockableTouchKey(
                    new Point(xOfs_[3] + 13 * HSpc, yOfs_ + 3 * VSpc),
                    42, InLang,
                    Properties.Resources.ShiftKeyReleased,
                    Properties.Resources.ShiftKeyPressed,
                    Properties.Resources.ShiftActiveKeyReleased,
                    Properties.Resources.ShiftActiveKeyPressed
                )
            );
            keys_.Last().KeyDownAction = OnShiftKeyDown;

            keys_.Add(
                new GraphicTouchKey(
                    new Point(258, yOfs_ + 4 * VSpc),
                    57, InLang, new Rectangle(3, 3, 389, 54),
                    Properties.Resources.SpacebarKeyReleased,
                    Properties.Resources.SpacebarKeyPressed
                )
            );

            const int MenuKeysXOfs = 676;
            keys_.Add(
                new LockableTouchKey(
                    new Point(MenuKeysXOfs + 0 * HSpc, yOfs_ + 4 * VSpc),
                    56, InLang, 
                    Properties.Resources.AltGrKeyReleased,
                    Properties.Resources.AltGrKeyPressed,
                    Properties.Resources.AltGrActiveKeyReleased,
                    Properties.Resources.AltGrActiveKeyPressed
                )
            );
            keys_.Last().KeyDownAction = OnAltGrKeyDown;

            keys_.Add(
                new GraphicTouchKey(
                    new Point(MenuKeysXOfs + 1 * HSpc, yOfs_ + 4 * VSpc),
                    75, InLang, 
                    Properties.Resources.LeftKeyReleased,
                    Properties.Resources.LeftKeyPressed
                )
            );
            keys_.Last().KeyDownAction = OnLeftKeyDown;

            keys_.Add(
                new GraphicTouchKey(
                    new Point(MenuKeysXOfs + 2 * HSpc, yOfs_ + 4 * VSpc),
                    77, InLang,
                    Properties.Resources.RightKeyReleased,
                    Properties.Resources.RightKeyPressed
                )
            );
            keys_.Last().KeyDownAction = OnRightKeyDown;
        }

        private void DestroyKeyboard()
        {
            keys_.Clear();
        }

        private static StringFormat GetTextStringFormat()
        {
            StringFormat Fmt = new StringFormat();
            Fmt.Alignment = StringAlignment.Near;
            Fmt.LineAlignment = StringAlignment.Center;
            return Fmt;
        }

        static public Font ChangeFontSize(Font font, float fontSize)
        {
            if (font != null)
            {
                float currentSize = font.Size;
                if (currentSize != fontSize)
                    font = 
                        new Font(
                            font.Name, fontSize,
                            font.Style, font.Unit,
                            font.GdiCharSet, font.GdiVerticalFont
                        );
            }
            return font;
        }

        private const int WM_INPUTLANGCHANGE = 0x51;

        protected override void WndProc(ref Message aMessage)
        {
            base.WndProc( ref aMessage );
            switch ( aMessage.Msg ) { 
                case WM_INPUTLANGCHANGE:
                    RebuildKeyboard();
                    Invalidate();
                    break;
            } 
        }

        protected override bool ProcessCmdKey( ref Message msg, Keys keyData )
        {
            //Debug.WriteLine("ProcessCmdKey");
            switch ( keyData ) {
                case Keys.Left:
                    --CaretPosition;
                    break;
                case Keys.Right:
                    ++CaretPosition;
                    break;
            }
            return base.ProcessCmdKey( ref msg, keyData);
        }

        protected virtual void SetText(string Val)
        {
            base.Text = Val;
            Invalidate(textRect_);
        }

        protected override void SetBoundsCore(int x, int y,
                                              int width, int height,
                                              BoundsSpecified specified)
        {
            Size Area = new Size(
                Math.Max( 966, BackgroundImage != null ? BackgroundImage.Width : 0 ),
                Math.Max( 468, BackgroundImage != null ? BackgroundImage.Height : 0  )
            );
            base.SetBoundsCore( x, y, Area.Width, Area.Height, specified );
        }

        private void DrawText(Graphics g, Brush FontBrush, string Txt)
        {
            g.DrawString(Txt, Font, FontBrush, textRect_, defStringFormat_);
        }

        private Rectangle GetCaretRectangle(Graphics g, Rectangle LayoutRect, int Position )
        {
            string Txt = Text + "_";
            int Pos = Math.Min(Math.Max(Position, 0), Text.Length);
            CharacterRange[] characterRanges = { new CharacterRange(Pos, 1) };
            RectangleF lr = new RectangleF(LayoutRect.Left, LayoutRect.Top, LayoutRect.Width, LayoutRect.Height);
            StringFormat stringFormat = defStringFormat_;
            stringFormat.SetMeasurableCharacterRanges(characterRanges);
            Region[] stringRegions = g.MeasureCharacterRanges(Txt, Font, lr, stringFormat );
            return Rectangle.Round( stringRegions[0].GetBounds(g) );
        }

        private void SetupGraphics( Graphics g )
        {
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        }

        private Rectangle GetButtonTextRect( TouchKey Key )
        {
            Rectangle TextRect = Key.TextRect;
            TextRect.Offset( Key.Position );
            return TextRect;
        }

        private void DrawButton(Graphics g, Brush FontBrush, TouchKey k)
        {
            Bitmap BtnBmp = GetKeyBitmap(k);
            g.DrawImage(BtnBmp, k.Position);
            string KeyChar = k.GetKeyText(shiftPressed_, altGrPressed_, capsLockActive_);
            Rectangle TextRect = GetButtonTextRect(k);
            if (k == pressedkey_)
                TextRect.Offset( pressedOffset_ );
            //g.DrawRectangle(Pens.Red, TextRect);
            StringFormat Fmt = new StringFormat();
            Fmt.Alignment = StringAlignment.Center;
            Fmt.LineAlignment = StringAlignment.Center;
            g.DrawString( KeyChar, Font, FontBrush, TextRect, Fmt );
        }

        private TouchKey HitButton(Point Location)
        {
            foreach (TouchKey k in keys_)
                if (GetButtonTextRect(k).Contains(Location))
                    return k;
            return null;
        }

        private bool IsKeyLocked(TouchKey Key)
        {
            switch (Key.Key)
            {
                case Keys.ShiftKey:
                    return shiftPressed_;
                case Keys.CapsLock:
                    return capsLockActive_;
                case Keys.Menu:
                    return altGrPressed_;
                default:
                    return false;
            }
        }

        private Bitmap GetKeyBitmap( TouchKey Key )
        {
            if (Key == pressedkey_)
            {
                Bitmap Bmp = Key.GetPressedBmp(IsKeyLocked(Key));
                return Bmp != null ? Bmp : pressedKeyBmp_;
            }
            else
            {
                Bitmap Bmp = Key.GetReleasedBmp(IsKeyLocked(Key));
                return Bmp != null ? Bmp : releasedKeyBmp_;
            }
        }

        private Rectangle GetKeyRectangle(TouchKey Key)
        {
            return new Rectangle(Key.Position, GetKeyBitmap(Key).Size);
        }

        private void UpdateKeyState(TouchKey PressedKey)
        {
            if (PressedKey != pressedkey_)
            {
                Brush FontBrush = new SolidBrush(ForeColor);
                TouchKey OldPressedKey = pressedkey_;
                pressedkey_ = null;
                if (OldPressedKey != null)
                {
                    Invalidate(GetKeyRectangle(OldPressedKey));
                    if (OldPressedKey.KeyUpAction != null)
                        OldPressedKey.KeyUpAction(OldPressedKey);
                }
                pressedkey_ = PressedKey;
                if (PressedKey != null)
                {
                    Invalidate(GetKeyRectangle(PressedKey));
                    if (PressedKey.KeyDownAction == null)
                    {
                        int OldCaretPos = CaretPosition;
                        string Text = GetKeyString(
                            PressedKey.Key, shiftPressed_, altGrPressed_, capsLockActive_, 
                            InputLanguage.CurrentInputLanguage
                        );
                        InsertKeys( Text, OldCaretPos );
                        CaretPosition += Text.Length;

                        if (shiftPressed_ )
                        {
                            shiftPressed_ = false;
                            Invalidate();
                        }
                        if (altGrPressed_)
                        {
                            altGrPressed_ = false;
                            Invalidate();
                        }

                    }
                    else
                        PressedKey.KeyDownAction(PressedKey);
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                TouchKey Key = HitButton(e.Location);
                UpdateKeyState(Key);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
                UpdateKeyState(null);
        }
   
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode,
           byte[] lpKeyState, UInt16[] pwszBuff,
           int cchBuff, uint wFlags, IntPtr dwhkl);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int GetKeyboardState(byte[] keystate);

        private string GetKeyString(Keys key, bool ShiftPressed, bool AltGrPressed, bool CapsLockPressed, InputLanguage InLng)
        {
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);
            keyboardState[(int)Keys.ShiftKey] ^= ShiftPressed ? (byte)0x80 : (byte)0;
            keyboardState[(int)Keys.CapsLock] ^= CapsLockPressed ? (byte)0x01 : (byte)0;
            keyboardState[(int)Keys.Menu] ^= AltGrPressed ? (byte)0x80 : (byte)0;
            keyboardState[(int)Keys.ControlKey] ^= AltGrPressed ? (byte)0x80 : (byte)0;
            UInt16[] b = new UInt16[4];
            return ArrayToStr(b, ToUnicodeEx((uint)key, 0, keyboardState, b, b.Length, 0, InLng.Handle));
        }

        private string ArrayToStr(UInt16[] Buffer, int Cnt )
        {
            string Ret = "";
            for (int Idx = 0; Idx < Cnt; ++Idx)
                Ret += Convert.ToString(Convert.ToChar(Buffer[Idx]));
            return Ret;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            //Debug.WriteLine(String.Format("Down: KeyCode={0:d}, KeyData={1:d}, KeyValue={2:d}, Last={3:d}", e.KeyCode, e.KeyData, e.KeyValue, lastKeyData_));
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    if (lastKeyData_ != 0)
                        return;
                    break;
                case Keys.Menu:
                case Keys.ControlKey:
                case Keys.CapsLock:
                    break;
                case Keys.Back:
                    Rubout();
                    return;
                default:
                    lastKeyData_ = e.KeyData;
                    return;
            }

            if (lastKeyData_ != e.KeyData) {
                lastKeyData_ = e.KeyData;
                Invalidate(this.ClientRectangle);
            }

        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            //Debug.WriteLine(String.Format("Press: KeyChar={0:c}", e.KeyChar ));
            base.OnKeyPress(e);
            if (char.IsLetterOrDigit(e.KeyChar) || char.IsPunctuation(e.KeyChar) ||
                char.IsSymbol(e.KeyChar) || char.IsSeparator(e.KeyChar)) 
            {
                int OldCaretPos = CaretPosition;
                InsertKey(e.KeyChar, CaretPosition);
                ++CaretPosition;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            //Debug.WriteLine(String.Format("Up: KeyCode={0:d}, KeyData={1:d}, KeyValue={2:d}, Last={3:d}", e.KeyCode, e.KeyData, e.KeyValue, lastKeyData_) );
            base.OnKeyUp(e);
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                case Keys.Menu:
                case Keys.ControlKey:
                case Keys.CapsLock:
                    break;
                /*
                case Keys.Left:
                    --CaretPosition;
                    return;
                case Keys.Right:
                    ++CaretPosition;
                    return;
                */
                default:
                    lastKeyData_ = 0;
                    return;
            }
            Invalidate();
            lastKeyData_ = 0;
        }

        private void OnShiftKeyDown( TouchKey Key )
        {
            shiftPressed_ = !shiftPressed_;
            Invalidate();
        }

        private void OnCapsLockKeyDown( TouchKey Key )
        {
            capsLockActive_ = !capsLockActive_;
            Invalidate();
        }

        private void OnAltGrKeyDown(TouchKey Key)
        {
            altGrPressed_ = !altGrPressed_;
            Invalidate();
        }

        private void OnBackKeyDown(TouchKey Key)
        {
            Rubout();
        }

        private void OnLeftKeyDown(TouchKey Key)
        {
            --CaretPosition;
        }

        private void OnRightKeyDown(TouchKey Key)
        {
            ++CaretPosition;
        }

        private void SetCaretPos( int Val )
        {
            Val = Math.Min( Math.Max(0, Val), Text.Length );
            if ( Val != caretPos_ ) {
                Graphics g = CreateGraphics();
                SetupGraphics( g );
                Invalidate( GetCaretRectangle( g, textRect_, caretPos_ ) );
                caretPos_ = Val;
                Invalidate( GetCaretRectangle( g, textRect_, caretPos_ ) );
            }
        }

        private string InsertKey(TouchKey Key, int Position)
        {
            string Txt = 
                GetKeyString(
                    Key.Key, 
                    shiftPressed_, 
                    altGrPressed_, 
                    capsLockActive_, 
                    InputLanguage.CurrentInputLanguage 
                );
            if (Key != null && Txt != "\0" && Txt != "")
                InsertKeys(Txt, Position);
            return Txt;
        }

        private string InsertKey(char Key, int Position)
        {
            InsertKeys(Key.ToString(), Position);
            return Convert.ToString(Key);
        }

        private string InsertKeys(string Keys, int Position)
        {
            Text = Text.Substring(0, Position) + Keys + 
                   Text.Substring(Position, Text.Length - Position);
            return Text;
        }

        private void Rubout()
        {
            RemoveKey(CaretPosition--);
            if (CaretPosition > Text.Length)
                CaretPosition = Text.Length;
        }

        private void RemoveKey( int AtPos )
        {
            Text = Text.Substring(0, Math.Max( AtPos - 1, 0 ) ) + 
                   Text.Substring(AtPos, Text.Length - AtPos);
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            base.OnPaint(e);

            SetupGraphics(e.Graphics);

            Rectangle CaretRect = GetCaretRectangle(e.Graphics, textRect_, CaretPosition);
            if (e.ClipRectangle.IntersectsWith(CaretRect))
                e.Graphics.FillRectangle(Brushes.Lime, CaretRect);

            if (e.ClipRectangle.IntersectsWith(textRect_))
                DrawText(e.Graphics, new SolidBrush(ForeColor), Text);

            if (keys_ != null)
                foreach (TouchKey k in keys_)
                    if (e.ClipRectangle.IntersectsWith(GetKeyRectangle(k)))
                        DrawButton(e.Graphics, new SolidBrush(ForeColor), k);

        }

    }
}
