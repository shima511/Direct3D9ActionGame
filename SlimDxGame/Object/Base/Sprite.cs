using System;
using SlimDX.Direct3D9;
using SlimDX;
using System.Runtime.InteropServices;

namespace SlimDxGame.Object.Base
{
    class Sprite : PlaneObject
    {
        public Asset.Texture Texture { private get; set; }

        public Sprite(float width = 1.0f, float height = 1.0f)
        {
            _scale.X = width;
            _scale.Y = height;
        }

        private void UpdateMatrix()
        {
            var world_mat = Matrix.Scaling(_scale.X, _scale.Y, 0) * Matrix.RotationZ(Angle);
            world_mat.M41 = _position.X;
            world_mat.M42 = _position.Y;
            CommonMatrix.WorldMatrix = world_mat;
        }

        public override void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {
            UpdateMatrix();
            dev.Transform = CommonMatrix.WorldMatrix;
            dev.Draw(Texture.Resource, new Vector3((float)Texture.Width / 2, (float)Texture.Height / 2, 0.0F), new Vector3(), _color);
        }
    }
}
