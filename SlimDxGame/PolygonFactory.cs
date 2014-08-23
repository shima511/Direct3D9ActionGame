using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.Direct3D9;

namespace SlimDxGame
{
    class PolygonFactory
    {
        struct PolygonVertex
        {
            public PolygonVertex(Vector3 pos, Vector2 uv)
            {
                Position = pos;
                UV = uv;
            }
            Vector3 Position;
            Vector2 UV;
        }

        public static SlimDX.Direct3D9.Device Device { private get; set; }

        public static void CreateSquarePolygon(out Vertex vertex)
        {
            vertex = new Vertex();
            vertex.Size = Marshal.SizeOf(typeof(PolygonVertex));
            vertex.TriangleCount = 2;
            vertex.Format = VertexFormat.Position | VertexFormat.Texture1;
            vertex.Buffer = new VertexBuffer(
                Device,
                vertex.TriangleCount * 3 * vertex.Size,
                Usage.WriteOnly,
                VertexFormat.None,
                Pool.Managed
                );

            var stream = vertex.Buffer.Lock(0, 0, LockFlags.None);
            stream.WriteRange(new []{
                new PolygonVertex(new Vector3(-0.5f, 0.0f, 0.5f), new Vector2(0.0f, 0.0f)),
                new PolygonVertex(new Vector3(0.5f, 0.0f, 0.5f), new Vector2(1.0f, 0.0f)),
                new PolygonVertex(new Vector3(-0.5f, 0.0f, -0.5f), new Vector2(0.0f, 1.0f)),
                new PolygonVertex(new Vector3(0.5f, 0.0f, -0.5f), new Vector2(1.0f, 1.0f)),
                new PolygonVertex(new Vector3(-0.5f, 0.0f, -0.5f), new Vector2(0.0f, 1.0f)),
                new PolygonVertex(new Vector3(0.5f, 0.0f, 0.5f), new Vector2(1.0f, 0.0f))
            });
            vertex.Buffer.Unlock();

            vertex.Declaration = new VertexDeclaration(Device, new[]{
                                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                                new VertexElement(0, 12, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
                                VertexElement.VertexDeclarationEnd
            });
        }
    }
}
