namespace SlimDxGame.Asset
{
    class Font : Base
    {
        public System.Drawing.Font Info { get; set; }
        public SlimDX.Direct3D9.Font Resource { get; set; }
        /// <summary>
        /// リソースの解放
        /// </summary>
        public void Release()
        {
            Resource.Dispose();
        }
    }
}
