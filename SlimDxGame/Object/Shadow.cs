using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object
{
    class Shadow : Base.SquarePolygon, Component.IUpdateObject
    {
        public Player Owner { private get; set; }
        public Collision.Shape.Line Line { private get; set; }

        public Shadow()
        {
            IsVisible = true;
        }

        void UpdatePosition()
        {
            var pos = Position;
            pos.X = Owner.Position.X + 0.5f;
            pos.Y = Line.GetYAxisFromXAxis(pos.X);
            Position = pos;
        }

        void UpdateScale()
        {
            // 床からの差分
            var diff = Owner.Position.Y - Line.GetYAxisFromXAxis(Owner.Position.X);
            var scale = Scale;
            scale.X = diff;
            scale.Y = diff;
            Scale = scale;
        }

        void UpdateRotation()
        {
            var rot = Rotation;
            rot.X = (float)Math.PI / 2;
            rot.Z = Line.Slope;
            Rotation = rot;
        }

        public void Update()
        {
            UpdateScale();
            UpdatePosition();
            UpdateRotation();
        }
    }
}
