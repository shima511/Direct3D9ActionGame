using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlimDX.Direct3D9;
using SlimDX;

namespace LevelCreator.Object
{
    class Model : IBase
    {
        public bool Selected { get; set; }
        public Asset.Model ModelAsset { private get; set; }
        protected Vector3 _position = new Vector3();
        protected Vector3 _scale = new Vector3(1.0f, 1.0f, 1.0f);
        protected Vector3 _rotation = new Vector3();
        public Vector3 Position { get { return _position; } set { _position = value; } }
        public Vector3 Scale { get { return _scale; } set { _scale = value; } }
        public Vector3 Rotation { get { return _rotation; } set { _rotation = value; } }

        public virtual void Update()
        {

        }

        Matrix GetMatrix()
        {
            var world_mat = Matrix.Scaling(Scale.X, Scale.Y, Scale.Z) * Matrix.RotationYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
            world_mat.M41 = Position.X;
            world_mat.M42 = Position.Y;
            world_mat.M43 = Position.Z;
            return world_mat;
        }

        public void InputAction(KeyEventArgs e)
        {

        }

        public void Draw(SlimDX.Direct3D9.Device dev)
        {
            dev.SetTransform(TransformState.World, GetMatrix());
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
    }
}
