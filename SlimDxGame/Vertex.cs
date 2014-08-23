using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.Direct3D9;
using SlimDX;

namespace SlimDxGame
{
    class Vertex : IDisposable
    {
        /// <summary>
        /// バッファ
        /// </summary>
        public VertexBuffer Buffer { get; set; }
        /// <summary>
        /// 頂点の宣言
        /// </summary>
        public VertexDeclaration Declaration { get; set; }
        /// <summary>
        /// 頂点フォーマット
        /// </summary>
        public VertexFormat Format { get; set; }
        /// <summary>
        /// 頂点1つ当たりのサイズ
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 3角形の数
        /// </summary>
        public int TriangleCount { get; set; }

        public void Dispose()
        {
            Buffer.Dispose();
            Declaration.Dispose();
        }
    }
}
