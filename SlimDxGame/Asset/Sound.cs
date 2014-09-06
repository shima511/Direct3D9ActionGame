using System;
using System.Collections.Generic;
using System.IO;
using SlimDX.XAudio2;
using SlimDX.Multimedia;

namespace SlimDxGame.Asset
{
    class Sound : IDisposable
    {
        public class VoiceInfo
        {
            private bool _is_playing = false;
            public bool Playing { get { return _is_playing; } set { _is_playing = value; } }
            public SourceVoice Resource { get; set; }
        }
        public AudioBuffer Buffer { private get; set; }
        public List<VoiceInfo> Voices { get; set; }
        public WaveFormat Format { get; set; }
        public MemoryStream Stream { private get; set; }
        float _volume = 1.0f;
        /// <summary>
        /// 音量
        /// </summary>
        public float Volume {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
                foreach (var item in Voices)
                {
                    item.Resource.Volume = _volume;
                }
            }
        }

        /// <summary>
        /// 音を作成します。
        /// </summary>
        public void Play()
        {
            for (int i = 0; i < Voices.Count; i++)
            {
                // バッファがなくなったらバッファを再設定
                if (Voices[i].Resource.State.BuffersQueued <= 0 && Voices[i].Playing)
                {
                    Buffer.AudioData.Seek(0, SeekOrigin.Begin);
                    Voices[i].Resource.FlushSourceBuffers();
                    Voices[i].Resource.SubmitSourceBuffer(Buffer);
                    Voices[i].Playing = false;
                }
                // 再生中でない場合音声を再生する
                if (!Voices[i].Playing)
                {
                    Voices[i].Resource.Start();
                    Voices[i].Playing = true;
                    break;
                }
            }
        }

        /// <summary>
        /// 音を停止させます。
        /// </summary>
        public void Stop()
        {
            Voices.ForEach(delegate(VoiceInfo voice)
            {
                voice.Resource.Stop();
            });
        }

        /// <summary>
        /// 未実装
        /// </summary>
        public void Pause()
        {
            
        }

        /// <summary>
        /// 未実装
        /// </summary>
        public void Resume()
        {

        }

        /// <summary>
        /// リソースの解放
        /// </summary>
        public void Dispose()
        {
            Buffer.Dispose();
            Voices.ForEach(delegate(VoiceInfo voice){
                voice.Resource.Dispose();
            });
            Stream.Close();
            Stream.Dispose();
        }
    }
}
