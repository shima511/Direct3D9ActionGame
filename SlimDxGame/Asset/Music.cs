using System;
using SlimDX.XAudio2;
using SlimDX.Multimedia;
using System.IO;

namespace SlimDxGame.Asset
{
    class Music : Base
    {
        public bool Playing { get; private set; }
        public AudioBuffer Buffer { private get; set; }
        public SourceVoice Resource { private get; set; }
        public MemoryStream Stream { private get; set; }
        public WaveFormat Format { get; set; }
        public float Volume { private get { return Resource.Volume; } set { Resource.Volume = value; } }

        /// <summary>
        /// 音楽を再生します。
        /// </summary>
        public void Play()
        {
            Resource.Start();
            Playing = true;
        }

        /// <summary>
        /// 音楽を停止します。
        /// </summary>
        public void Stop()
        {
            Resource.Stop();
            Playing = false;
        }

        /// <summary>
        /// 未対応
        /// </summary>
        public void Pause()
        {

        }

        /// <summary>
        /// 未対応
        /// </summary>
        public void Resume()
        {

        }

        /// <summary>
        /// リソースの解放
        /// </summary>
        public void Release()
        {
            Buffer.Dispose();
            Resource.Dispose();
            Stream.Close();
            Stream.Dispose();
        }
    }
}
