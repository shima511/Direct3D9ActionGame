using System;
using System.Collections.Generic;
using SlimDX.Direct3D9;

namespace SlimDxGame.AssetFactory
{
    class TextureFactory
    {
        static public SlimDX.Direct3D9.Device device { private get; set; }

        static public Asset.Texture CreateTextureFromFile(string filename)
        {
            var tex = new Asset.Texture();
            try
            {
                ImageInformation info;
                tex.Resource = Texture.FromFile(device, filename, 0, 0, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0, out info);
                var description = tex.Resource.GetSurfaceLevel(0).Description;
                tex.Width = info.Width;
                tex.Height = info.Height;
            }catch(Direct3D9Exception ex){
                System.Diagnostics.Debug.Assert(false, ex.Message + "：テクスチャ生成時");
                tex = null;
            }
            return tex;
        }

        static public Asset.Texture CreateFromRawData(int width, int height, byte[] buffer)
        {
            var tex = new Asset.Texture();
            tex.Height = height;
            tex.Width = width;
            tex.Resource = new Texture(device, width, height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
            var rect = tex.Resource.LockRectangle(0, LockFlags.None);
            rect.Data.Write(buffer, 0, sizeof(byte) * buffer.Length);
            tex.Resource.UnlockRectangle(0);
            return tex;
        }

        static public Asset.Texture CreateFromMemory(int width, int height, byte[] buffer)
        {
            var tex = new Asset.Texture();
            try
            {
                tex.Resource = Texture.FromMemory(device, buffer);
                var description = tex.Resource.GetSurfaceLevel(0).Description;
                tex.Width = description.Width;
                tex.Height = description.Height;
            }
            catch (Direct3D9Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message + "：テクスチャ生成時");
                tex = null;
            }
            return tex;

        }
    }
}
