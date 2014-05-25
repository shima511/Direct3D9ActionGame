using System;


namespace SlimDxGame.Asset
{
    class Texture : Base
    {
        public int Width { set; get; }
        public int Height { set; get; }
        public SlimDX.Direct3D9.Texture Resource { set; get; }
        public void Release()
        {
            Resource.Dispose();
        }
    }
}
