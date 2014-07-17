using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.Direct3D9;

namespace LevelCreator.Asset
{
    public class Model : IDisposable
    {
        public Mesh Mesh { get; set; }
        public List<SlimDX.Direct3D9.Texture> Textures { get; set; }
        public List<SlimDX.Direct3D9.ExtendedMaterial> Materials { get; set; }
        public List<SlimDX.Direct3D9.ExtendedMaterial> LoadedMaterials { get; set; }

        public void Dispose()
        {
            foreach (var tex in Textures)
            {
                tex.Dispose();
            }
            Mesh.Dispose();
        }
    }
}
