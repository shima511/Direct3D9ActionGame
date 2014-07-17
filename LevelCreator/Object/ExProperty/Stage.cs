using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.Object.ExProperty
{
    public class Stage : Object.IBase
    {
        public Asset.Model ModelAsset { private get; set; }
        List<Line> lines = new List<Line>();
        public BinaryParser.Property.Stage StageInfo { get; set; }

        public Stage()
        {
            lines.AddRange(new Line[]{new Line(), new Line(), new Line(), new Line()});
        }

        public void Update()
        {
            foreach (var item in lines)
            {
                item.ModelAsset = ModelAsset;
            }
            // 左上から右上に向かう線
            lines[0].StartPoint = new SlimDX.Vector2(StageInfo.LimitLine.Left, StageInfo.LimitLine.Top);
            lines[0].TerminatePoint = new SlimDX.Vector2(StageInfo.LimitLine.Right, StageInfo.LimitLine.Top);

            // 左上から左下に向かう線
            lines[1].StartPoint = new SlimDX.Vector2(StageInfo.LimitLine.Left, StageInfo.LimitLine.Top);
            lines[1].TerminatePoint = new SlimDX.Vector2(StageInfo.LimitLine.Left, StageInfo.LimitLine.Bottom);

            // 右下から左下に向かう線
            lines[2].StartPoint = new SlimDX.Vector2(StageInfo.LimitLine.Right, StageInfo.LimitLine.Bottom);
            lines[2].TerminatePoint = new SlimDX.Vector2(StageInfo.LimitLine.Left, StageInfo.LimitLine.Bottom);

            // 右下から右上に向かう線
            lines[3].StartPoint = new SlimDX.Vector2(StageInfo.LimitLine.Right, StageInfo.LimitLine.Bottom);
            lines[3].TerminatePoint = new SlimDX.Vector2(StageInfo.LimitLine.Right, StageInfo.LimitLine.Top);
            foreach (var item in lines)
            {
                item.Update();
            }
        }

        public void InputAction(KeyEventArgs e)
        {

        }

        public void Draw(SlimDX.Direct3D9.Device dev)
        {
            foreach (var item in lines)
            {
                item.Draw(dev);
            }
        }
    }
}
