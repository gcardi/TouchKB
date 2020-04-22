using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace TouchKB
{
    class GraphicTouchKey : TouchKey
    {
        private Bitmap releasedBmp_ = null;
        private Bitmap pressedBmp_ = null;
        private Rectangle? textRect_;

        public GraphicTouchKey( Point Position, int ScanCode, InputLanguage InLang,
                                Bitmap ReleasedBmp, Bitmap PressedBmp )
            : base( Position, ScanCode, InLang )
        {
            releasedBmp_ = ReleasedBmp;
            pressedBmp_ = PressedBmp;
        }

        public GraphicTouchKey( Point Position, int ScanCode, InputLanguage InLang, 
                                Rectangle TxtRect, Bitmap ReleasedBmp, Bitmap PressedBmp )
            : this(Position, ScanCode, InLang, ReleasedBmp, PressedBmp )
        {
            textRect_ = TxtRect;
        }

        protected override Rectangle GetTextRect()
        {
            return textRect_.HasValue ? textRect_.Value : base.GetTextRect();
        }

        protected override Bitmap DoGetReleasedBmp( bool Locked )
        {
            return releasedBmp_;
        }

        protected override Bitmap DoGetPressedBmp(bool Locked )
        {
            return pressedBmp_;
        }
    
    }
}
