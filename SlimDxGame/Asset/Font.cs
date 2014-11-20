using System;

namespace SlimDxGame.Asset
{
    class Font : IDisposable
    {
        public System.Drawing.Font Info { get; set; }
        public SlimDX.Direct3D9.Font Resource { get; set; }
        /// <summary>
        /// リソースの解放
        /// </summary>
        public void Dispose()
        {
            Resource.Dispose();
        }
    }
}
