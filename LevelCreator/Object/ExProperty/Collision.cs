using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.Object.ExProperty
{
    public class Collision : Object.IBase
    {
        public StageRW.Property.Collision CollisionInfo { get; set; }
        public Line Line { get; set; }

        public void Update()
        {
            Line.StartPoint = CollisionInfo.StartingPoint;
            Line.TerminatePoint = CollisionInfo.TerminatePoint;
            Line.Update();
        }

        public void InputAction(KeyEventArgs e)
        {

        }

        public void Draw(SlimDX.Direct3D9.Device dev)
        {
            Line.Draw(dev);
        }
    }
}
