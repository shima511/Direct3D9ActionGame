using System;

namespace SlimDxGame.Asset
{
    class Texture : IDisposable
    {
        public int Width { set; get; }
        public int Height { set; get; }
        public SlimDX.Direct3D9.Texture Resource { set; get; }
        public void Dispose()
        {
            Resource.Dispose();
        }
    }
}
