using System.Collections.Generic;

namespace SlimDxGame.Asset
{
    public class Model : Base
    {
        public SlimDX.Direct3D9.Mesh Mesh { get; set; }
        public List<SlimDX.Direct3D9.Texture> Textures { get; set; }
        public List<SlimDX.Direct3D9.ExtendedMaterial> Materials { get; set; }

        public Model()
        {
            Textures = new List<SlimDX.Direct3D9.Texture>();
            Materials = new List<SlimDX.Direct3D9.ExtendedMaterial>();
        }

        /// <summary>
        /// リソースの解放
        /// </summary>
        public void Release()
        {
            foreach (var tex in Textures)
            {
                tex.Dispose();
            }
            Mesh.Dispose();
        }
    }
}
