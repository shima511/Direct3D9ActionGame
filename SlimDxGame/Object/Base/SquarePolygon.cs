using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SlimDX.Direct3D9;
using SlimDX;

namespace SlimDxGame.Object.Base
{
    class SquarePolygon : Component.IDrawableObject, IDisposable
    {
        public Vertex Vertex { private get; set; }
        public Asset.Texture Texture { private get; set; }
        public bool IsVisible { get; set; }
        public Vector3 Position { get; set; }
        public Vector2 Scale { get; set; }
        public Vector3 Rotation { get; set; }

        void UpdateWorldMatrix(SlimDX.Direct3D9.Device dev)
        {
            var mat = CommonMatrix.WorldMatrix;
            mat = Matrix.Scaling(Scale.X, Scale.Y, 1.0f) * Matrix.RotationYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
            mat.M41 = Position.X;
            mat.M42 = Position.Y;
            mat.M43 = Position.Z;
            dev.SetTransform(TransformState.World, mat);
        }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            UpdateWorldMatrix(dev);
            dev.SetStreamSource(0, Vertex.Buffer, 0, Vertex.Size);
            dev.VertexDeclaration = Vertex.Declaration;
            dev.VertexFormat = Vertex.Format;
            dev.Material = new Material() { 
                Ambient = System.Drawing.Color.White, 
                Diffuse = System.Drawing.Color.White,
                Emissive = System.Drawing.Color.White
                
            };
            dev.SetTexture(0, Texture.Resource);
            dev.DrawPrimitives(PrimitiveType.TriangleList, 0, Vertex.TriangleCount);
        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }

        public void Dispose()
        {
            Vertex.Dispose();
        }
    }
}
