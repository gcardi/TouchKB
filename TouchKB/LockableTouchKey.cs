using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace TouchKB
{
    class LockableTouchKey : GraphicTouchKey
    {
        private Bitmap lockedReleasedBmp_ = null;
        private Bitmap lockedPressedBmp_ = null;

        public LockableTouchKey(Point Position, int ScanCode, InputLanguage InLang,
                                Bitmap ReleasedBmp, Bitmap PressedBmp,
                                Bitmap LockedReleasedBmp, Bitmap LockedPressedBmp)
            : base(Position, ScanCode, InLang, ReleasedBmp, PressedBmp)
        {
            lockedReleasedBmp_ = LockedReleasedBmp;
            lockedPressedBmp_ = LockedPressedBmp;
        }

        public LockableTouchKey(Point Position, int ScanCode,
                                InputLanguage InLang, Rectangle TxtRect,
                                Bitmap ReleasedBmp, Bitmap PressedBmp,
                                Bitmap LockedReleasedBmp, Bitmap LockedPressedBmp)
            : base(Position, ScanCode, InLang, TxtRect, ReleasedBmp, PressedBmp)
        {
            lockedReleasedBmp_ = LockedReleasedBmp;
            lockedPressedBmp_ = LockedPressedBmp;
        }

        protected override Bitmap DoGetReleasedBmp(bool Locked)
        {
            
            return Locked && lockedReleasedBmp_ != null ? lockedReleasedBmp_ : base.DoGetReleasedBmp(Locked);
        }

        protected override Bitmap DoGetPressedBmp(bool Locked)
        {
            return Locked && lockedPressedBmp_ != null ? lockedPressedBmp_ : base.DoGetPressedBmp(Locked);
        }
    }
}
