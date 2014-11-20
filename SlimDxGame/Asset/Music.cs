using System;
using SlimDX.XAudio2;
using SlimDX.Multimedia;
using System.IO;

namespace SlimDxGame.Asset
{
    class Music : IDisposable
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
            if(!Playing) Resource.Start();
            Playing = true;
        }

        /// <summary>
        /// 音楽を停止させます。
        /// </summary>
        public void Stop()
        {
            if (Playing)
            {
                Resource.Stop();
                Buffer.AudioData.Seek(0, SeekOrigin.Begin);
                Resource.FlushSourceBuffers();
                Resource.SubmitSourceBuffer(Buffer);
                Playing = false;
            }
        }

        /// <summary>
        /// 音楽を一時停止します。
        /// </summary>
        public void Pause()
        {
            if (Playing)
            {
                Resource.Stop();
                Playing = false;
            }
        }

        /// <summary>
        /// リソースの解放
        /// </summary>
        public void Dispose()
        {
            Buffer.Dispose();
            Resource.Dispose();
            Stream.Close();
            Stream.Dispose();
        }
    }
}
