using System;
using System.IO;
using System.Windows.Forms;

namespace SlimDxGame.AssetFactory
{
   public enum ModelType
    {
        Box,
        Sphere,
        Cylinder
    }

    class ModelFactory
    {
        static public SlimDX.Direct3D9.Device Device { private get; set; }

        public static Asset.Model CreateBasicModel(ModelType type, System.Drawing.Color color)
        {
            var new_model = new Asset.Model();
            switch (type)
            {
                case ModelType.Box:
                    new_model.Mesh = SlimDX.Direct3D9.Mesh.CreateBox(Device, 1.0f, 1.0f, 1.0f);
                    break;
                case ModelType.Sphere:
                    new_model.Mesh = SlimDX.Direct3D9.Mesh.CreateSphere(Device, 0.0f, 10, 10);
                    break;
                case ModelType.Cylinder:
                    new_model.Mesh = SlimDX.Direct3D9.Mesh.CreateCylinder(Device, 0.0f, 0.0f, 1.0f, 10, 10);
                    break;
                default:

                    break;
            }
            var new_mat = new SlimDX.Direct3D9.ExtendedMaterial()
            {
                MaterialD3D = new SlimDX.Direct3D9.Material()
                {
                    Diffuse = new SlimDX.Color4(color),
                    Emissive = new SlimDX.Color4(color)
                }
            };
            new_model.Materials.Add(new_mat);
            return new_model;
        }

        public static Asset.Model CreateModelFromFile(string filename)
        {
            var new_model = new Asset.Model();
            try
            {
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
            }catch(SlimDX.Direct3D9.Direct3D9Exception){
                new_model = null;
            }
            return new_model;
        }
    }
}
