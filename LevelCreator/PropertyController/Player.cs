using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.PropertyController
{
    class Player : StageObjectController
    {
        public Object.ExProperty.Player PlayerInfo { get; set; }
        public MaskedTextBox PositionXAxis { private get; set; }
        public MaskedTextBox PositionYAxis { private get; set; }

        public override void Clean()
        {
            PositionXAxis.LostFocus -= PositionXAxis_LostFocus;
            PositionXAxis.LostFocus -= GrobalLostFocusEvent;
            PositionYAxis.LostFocus -= PositionYAxis_LostFocus;
            PositionYAxis.LostFocus -= GrobalLostFocusEvent;
        }

        public override void Initialize()
        {
            PositionXAxis.LostFocus += PositionXAxis_LostFocus;
            PositionXAxis.LostFocus += GrobalLostFocusEvent;
            PositionYAxis.LostFocus += PositionYAxis_LostFocus;
            PositionYAxis.LostFocus += GrobalLostFocusEvent;
            SetTextBoxValue();
        }

        void PositionYAxis_LostFocus(object sender, EventArgs e)
        {
            var info = PlayerInfo.PlayerInfo;
            var pos = info.Position;
            pos.Y = TextBoxParser.ToSingle(PositionYAxis.Text);
            info.Position = pos;
            PlayerInfo.PlayerInfo = info;
        }

        void PositionXAxis_LostFocus(object sender, EventArgs e)
        {
            var info = PlayerInfo.PlayerInfo;
            var pos = info.Position;
            pos.X = TextBoxParser.ToSingle(PositionXAxis.Text);
            info.Position = pos;
            PlayerInfo.PlayerInfo = info;
        }

        void SetTextBoxValue()
        {
            var info = PlayerInfo.PlayerInfo;
            PositionXAxis.Text = info.Position.X.ToString();
            PositionYAxis.Text = info.Position.Y.ToString();
        }
    }
}
