using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.PropertyController
{
    class Stage : StageObjectController
    {
        public Object.ExProperty.Stage StageInfo { get; set; }
        public MaskedTextBox LimitLineLeft { private get; set; }
        public MaskedTextBox LimitLineTop { private get; set; }
        public MaskedTextBox LimitLineRight { private get; set; }
        public MaskedTextBox LimitLineBottom { private get; set; }
        public MaskedTextBox LimitTime { private get; set; }

        public override void Clean()
        {
            LimitLineLeft.LostFocus -= LimitLineLeft_LostFocus;
            LimitLineLeft.LostFocus -= GrobalLostFocusEvent;
            LimitLineTop.LostFocus -= LimitLineTop_LostFocus;
            LimitLineTop.LostFocus -= GrobalLostFocusEvent;
            LimitLineRight.LostFocus -= LimitLineRight_LostFocus;
            LimitLineRight.LostFocus -= GrobalLostFocusEvent;
            LimitLineBottom.LostFocus -= LimitLineBottom_LostFocus;
            LimitLineBottom.LostFocus -= GrobalLostFocusEvent;
            LimitTime.LostFocus -= LimitTime_LostFocus;
        }

        public override void Initialize()
        {
            LimitLineLeft.LostFocus += LimitLineLeft_LostFocus;
            LimitLineLeft.LostFocus += GrobalLostFocusEvent;
            LimitLineTop.LostFocus += LimitLineTop_LostFocus;
            LimitLineTop.LostFocus += GrobalLostFocusEvent;
            LimitLineRight.LostFocus += LimitLineRight_LostFocus;
            LimitLineRight.LostFocus += GrobalLostFocusEvent;
            LimitLineBottom.LostFocus += LimitLineBottom_LostFocus;
            LimitLineBottom.LostFocus += GrobalLostFocusEvent;
            LimitTime.LostFocus += LimitTime_LostFocus;
            SetTextBoxValue();
        }

        void LimitTime_LostFocus(object sender, EventArgs e)
        {
            var info = StageInfo.StageInfo;
            info.LimitTime = MaskedTextParser.ToInt32(LimitTime.Text);
            StageInfo.StageInfo = info;
        }

        void LimitLineBottom_LostFocus(object sender, EventArgs e)
        {
            var info = StageInfo.StageInfo;
            var line = info.LimitLine;
            line.Height = -MaskedTextParser.ToInt32(LimitLineTop.Text) + MaskedTextParser.ToInt32(LimitLineBottom.Text);
            info.LimitLine = line;
            StageInfo.StageInfo = info;
        }

        void LimitLineRight_LostFocus(object sender, EventArgs e)
        {
            var info = StageInfo.StageInfo;
            var line = info.LimitLine;
            line.Width = MaskedTextParser.ToInt32(LimitLineRight.Text) - MaskedTextParser.ToInt32(LimitLineLeft.Text);
            info.LimitLine = line;
            StageInfo.StageInfo = info;
        }

        void LimitLineTop_LostFocus(object sender, EventArgs e)
        {
            var info = StageInfo.StageInfo;
            var line = info.LimitLine;
            line.Y = MaskedTextParser.ToInt32(LimitLineTop.Text);
            info.LimitLine = line;
            StageInfo.StageInfo = info;
            LimitLineBottom_LostFocus(sender, e);
        }

        void LimitLineLeft_LostFocus(object sender, EventArgs e)
        {
            var info = StageInfo.StageInfo;
            var line = info.LimitLine;
            line.X = MaskedTextParser.ToInt32(LimitLineLeft.Text);
            info.LimitLine = line;
            StageInfo.StageInfo = info;
            LimitLineRight_LostFocus(sender, e);
        }

        void SetTextBoxValue()
        {
            var info = StageInfo.StageInfo;
            LimitLineBottom.Text = info.LimitLine.Bottom.ToString();
            LimitLineLeft.Text = info.LimitLine.X.ToString();
            LimitLineRight.Text = info.LimitLine.Right.ToString();
            LimitLineTop.Text = info.LimitLine.Y.ToString();
        }
    }
}
