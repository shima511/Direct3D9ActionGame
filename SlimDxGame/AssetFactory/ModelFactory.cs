using System;
using System.IO;
using System.Windows.Forms;

namespace SlimDxGame.AssetFactory
{
    class ModelFactory
    {
        static public SlimDX.Direct3D9.Device Device { private get; set; }

        public static Asset.Model CreateModelFromFile(string filename)
        {
            var new_model = new Asset.Model();
            new_model.Mesh = SlimDX.Direct3D9.Mesh.FromFile(Device, filename, SlimDX.Direct3D9.MeshFlags.Managed);
            // 属性を取得
            foreach (var material in new_model.Mesh.GetMaterials())
            {
                new_model.Materials.Add(material);
            }

            if (new_model.Materials.Count >= 1)
            {
                for (int i = 0; i < new_model.Materials.Count; i++)
                {
                    string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                    var tex_filename = new_model.Materials[i].TextureFileName;
                    if (!string.IsNullOrEmpty(tex_filename))
                    {
                        var tex_path = Path.Combine(baseDir, Path.Combine("models", tex_filename));
                        new_model.Textures.Add(SlimDX.Direct3D9.Texture.FromFile(Device, tex_path));
                    }
                }
            }

            return new_model;
        }
    }
}
