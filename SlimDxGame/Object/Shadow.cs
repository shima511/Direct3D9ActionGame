using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object
{
    class Shadow : Base.SquarePolygon, Component.IUpdateObject
    {
        /// <summary>
        /// 影の所有者
        /// </summary>
        public Base.Model Owner { get; set; }
        /// <summary>
        /// 影の投影先の線分
        /// </summary>
        public Collision.Shape.Line ProjectionLine { private get; set; }
        readonly float MaxScale;
        public bool IsActive { get; set; }
        public Shadow(float max_scale = 3.0f)
        {
            MaxScale = max_scale;
            IsVisible = true;
        }

        void UpdatePosition()
        {
            var pos = Position;
            pos.X = Owner.Position.X + 0.5f;
            pos.Y = ProjectionLine.GetYAxisFromXAxis(pos.X) + 0.1f;
            Position = pos;
        }

        void UpdateScale()
        {
            // 床からの差分
            var diff = Owner.Position.Y - ProjectionLine.GetYAxisFromXAxis(Owner.Position.X);
            var scale = Scale;
            scale.X = MaxScale - diff;
            scale.Y = MaxScale - diff;
            if (diff < 0)
            {
                scale.X = 0.0f; scale.Y = 0.0f;
            }
            Scale = scale;
        }

        void UpdateRotation()
        {
            var rot = Rotation;
            rot.Z = ProjectionLine.Slope;
            Rotation = rot;
        }

        public void Update()
        {
            if (ProjectionLine == null)
            {
                var scale = Scale;
                scale.X = 0;
                scale.Y = 0;
                Scale = scale;
            }
            else
            {
                UpdateScale();
                UpdatePosition();
                UpdateRotation();
            }
        }
    }
}
