using System;
using SlimDX;

namespace SlimDxGame.Object.Base
{
    class String : PlaneObject
    {
        public Asset.Font Font { get; set; }
        public string Text { get; set; }

        private void UpdateMatrix()
        {
            var world_mat = CommonMatrix.WorldMatrix;
            world_mat = Matrix.Identity;
            world_mat = Matrix.Scaling(_scale.X, _scale.Y, 0) * Matrix.RotationZ(Angle);
            world_mat.M41 = _position.X;
            world_mat.M42 = _position.Y;
            CommonMatrix.WorldMatrix = world_mat;
        }

        public override void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {
            UpdateMatrix();
            dev.Transform = CommonMatrix.WorldMatrix;
            Font.Resource.DrawString(dev, Text, 0, 0, _color);
        }
    }
}
