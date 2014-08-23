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
            scale.X = 5.0f - diff;
            scale.Y = 5.0f - diff;
            if (scale.X < 0)
            {
                scale.X = 0.0f;
            }
            if (scale.Y < 0)
            {
                scale.Y = 0.0f;
            }
            Scale = scale;
        }

        void UpdateRotation()
        {
            var rot = Rotation;
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
