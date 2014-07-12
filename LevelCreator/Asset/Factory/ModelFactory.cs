using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.Direct3D9;
using System.IO;
using System.Windows.Forms;

namespace LevelCreator.Asset.Factory
{
    public class ModelFactory : IDisposable
    {
        Device D3DDevice { get; set; }
        Dictionary<string, Asset.Model> models = new Dictionary<string,Model>();

        public ModelFactory(Device d3ddev)
        {
            D3DDevice = d3ddev;
            CreateStandardModel(ModelType.Box);
            CreateStandardModel(ModelType.Sphere);
        }

        public Asset.Model FindModel(string name)
        {
            Asset.Model model;
            models.TryGetValue(name, out model);
            return model;
        }

        void CreateStandardModel(ModelType type)
        {
            var new_model = new Asset.Model();
            new_model.Materials = new List<ExtendedMaterial>();
            new_model.Materials.Add(new ExtendedMaterial(){
                MaterialD3D = new Material()
                {
                    Diffuse = new SlimDX.Color4(1.0f, 0.0f, 0.0f, 0.0f),
                    Emissive = new SlimDX.Color4(1.0f, 0.0f, 0.0f, 0.0f),
                    Ambient = new SlimDX.Color4(1.0f, 0.0f, 0.0f, 0.0f)
                }
            });
            new_model.Textures = new List<Texture>();
            switch (type)
            {
                case ModelType.Box:
                    new_model.Mesh = Mesh.CreateBox(D3DDevice, 1.0f, 1.0f, 1.0f);
                    models.Add("Box", new_model);
                    break;
                case ModelType.Sphere:
                    new_model.Mesh = Mesh.CreateSphere(D3DDevice, 1.0f, 15, 15);
                    models.Add("Sphere", new_model);
                    break;
                default:
                    break;
            }
        }

        public void CreateFromFile(string filename, string name)
        {
            var new_model = new Asset.Model();
            new_model.Materials = new List<ExtendedMaterial>();
            new_model.Textures = new List<Texture>();
            try
            {
                new_model.Mesh = SlimDX.Direct3D9.Mesh.FromFile(D3DDevice, filename, SlimDX.Direct3D9.MeshFlags.Managed);
                // 属性を取得
                foreach (var material in new_model.Mesh.GetMaterials())
                {
                    new_model.Materials.Add(material);
                }

                for (int i = 0; i < new_model.Materials.Count; i++)
                {
                    string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                    var tex_filename = new_model.Materials[i].TextureFileName;
                    if (!string.IsNullOrEmpty(tex_filename))
                    {
                        var tex_path = Path.Combine(baseDir, Path.Combine("models", tex_filename));
                        new_model.Textures.Add(SlimDX.Direct3D9.Texture.FromFile(D3DDevice, tex_path));
                    }
                }
                models.Add(name, new_model);
            }
            catch (SlimDX.Direct3D9.Direct3D9Exception)
            {
                new_model = null;
            }
        }

        public void Dispose()
        {
            foreach (var item in models.Values)
            {
                item.Dispose();
            }
        }
    }
}
