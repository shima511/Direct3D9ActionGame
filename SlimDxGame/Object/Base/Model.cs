using System;
using SlimDX;
using SlimDX.Direct3D9;

namespace SlimDxGame.Object.Base
{
    public class Model : Component.IDrawableObject
    {
        bool _is_visible = true;
        public bool IsVisible { get { return _is_visible; } set { _is_visible = value; } }
        public Asset.Model ModelAsset { protected get; set; }
        protected Vector3 _position = new Vector3();
        protected Vector3 _scale = new Vector3(1.0f, 1.0f, 1.0f);
        protected Vector3 _rotation = new Vector3();
        public Vector3 Position { get { return _position; } set { _position = value; } }
        public Vector3 Scale { get { return _scale; } set { _scale = value; } }
        public Vector3 Rotation { get { return _rotation; } set { _rotation = value; } }

        private void UpdateMatrix()
        {
            var world_mat = Matrix.Scaling(_scale.X, _scale.Y, _scale.Z) * Matrix.RotationYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z);
            world_mat.M41 = _position.X;
            world_mat.M42 = _position.Y;
            world_mat.M43 = _position.Z;
            CommonMatrix.WorldMatrix = world_mat;
        }

        virtual public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            UpdateMatrix();
            dev.SetTransform(TransformState.World, CommonMatrix.WorldMatrix);
            for (int i = 0; i < ModelAsset.Materials.Count; i++)
            {
                dev.Material = ModelAsset.Materials[i].MaterialD3D;
                if (i < ModelAsset.Textures.Count)
                {
                    dev.SetTexture(0, ModelAsset.Textures[i]);
                }
                ModelAsset.Mesh.DrawSubset(i);
            }
        }

        virtual public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }
    }
}
