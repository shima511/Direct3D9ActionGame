﻿using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

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
        static Dictionary<string, Asset.Model> meshes = new Dictionary<string,Asset.Model>();

        [System.Diagnostics.Conditional("DEBUG")]
        public static void InitBasicModels(ModelType type)
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
                    Diffuse = new SlimDX.Color4(System.Drawing.Color.White.ToArgb()),
                    Emissive = new SlimDX.Color4(System.Drawing.Color.White.ToArgb())
                }
            };
            new_model.Materials.Add(new_mat);
            meshes.Add(type.ToString(), new_model);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void Terminate()
        {
            foreach (var item in meshes)
            {
                item.Value.Dispose();
            }
        }


        public static Asset.Model CreateBasicModel(ModelType type)
        {
            return meshes[type.ToString()];
        }

        public static Asset.Model CreateModelFromMemory(byte[] bytes, FileArchiver.DataReader reader)
        {
            var new_model = new Asset.Model();
            try
            {
                new_model.Mesh = SlimDX.Direct3D9.Mesh.FromMemory(Device, bytes, SlimDX.Direct3D9.MeshFlags.Managed);
                // 属性を取得
                foreach (var material in new_model.Mesh.GetMaterials())
                {
                    new_model.Materials.Add(material);
                }

                if (new_model.Materials.Count >= 1)
                {
                    for (int i = 0; i < new_model.Materials.Count; i++)
                    {
                        var tex_filename = new_model.Materials[i].TextureFileName;
                        if (!string.IsNullOrEmpty(tex_filename))
                        {
                            new_model.Textures.Add(SlimDX.Direct3D9.Texture.FromMemory(Device, reader.GetBytes(tex_filename)));
                        }
                    }
                }
            }
            catch (SlimDX.Direct3D9.Direct3D9Exception ex)
            {
                new_model = null;
            }
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
