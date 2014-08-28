using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame
{
    class ShadowManager : Component.IUpdateObject, Component.IDrawableObject
    {
        public bool IsVisible { get { return true; } set { } }
        public List<Object.Shadow> Shadows { private get; set; }
        public List<Object.Ground.Base> Grounds { private get; set; }

        public void Update()
        {
            foreach (var shadow in Shadows)
            {
                foreach (var item in Grounds)
                {
                    var bet_start_to_owner = item.CollisionLine.StartingPoint.X - shadow.Owner.Position.X;
                    var bet_terminal_to_owner = item.CollisionLine.TerminalPoint.X - shadow.Owner.Position.X;
                    if (bet_start_to_owner * bet_terminal_to_owner < 0)
                    {
                        shadow.ProjectionLine = item.CollisionLine;
                        break;
                    }
                    shadow.ProjectionLine = null;
                }
                shadow.Update();
            }
        }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            foreach (var item in Shadows)
            {
                item.Draw3D(dev);
            }
        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }

    }
}
