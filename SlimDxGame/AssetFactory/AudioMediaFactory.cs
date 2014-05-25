using System;
using SlimDX.XAudio2;
using SlimDX.Multimedia;
using System.Collections.Generic;
using System.IO;

namespace SlimDxGame.AssetFactory
{
    class AudioMediaFactory
    {
        static public XAudio2 Device { private get; set; }

        static private void LoadDataFromFile(out MemoryStream stream, out WaveFormat format,string filename){
            List<SoundLoader> loader = new List<SoundLoader>();
            loader.Add(new WavFileLoader());
            // oggローダがうまくいかない
//            loader.Add(new OggFileLoader());

            for (int i = 0; i < loader.Count; i++)
            {
                if (loader[i].LoadFromFile(out stream, out format, filename))
                {
                    return;
                }
            }
            stream = null;
            format = null;
        }

        static private AudioBuffer CreateAudioBuffer(MemoryStream stream)
        {
            AudioBuffer buffer = new AudioBuffer();
            buffer.AudioData = stream;
            buffer.AudioBytes = (int)stream.Length;
            buffer.Flags = BufferFlags.EndOfStream;
            return buffer;
        }

        static private SourceVoice CreateSourceVoice(MemoryStream stream, WaveFormat format, AudioBuffer buffer)
        {
            var souceVoice = new SourceVoice(Device, format);
            souceVoice.SubmitSourceBuffer(buffer);
            return souceVoice;
        }

        /// <summary>
        /// ファイルから音声を作成します。
        /// </summary>
        /// <param name="filename">ファイル名</param>
        /// <param name="num_of_sound">必要なボイス数</param>
        /// <returns>生成した音声オブジェクト</returns>
        static public Asset.Sound CreateSoundFromFile(string filename, int num_of_sound = 10)
        {
            var new_sound = new Asset.Sound();
            try
            {
                MemoryStream stream;
                WaveFormat format;
                // データをロードする
                LoadDataFromFile(out stream, out format, filename);
                var buffer = CreateAudioBuffer(stream);
                // サウンドに設定
                new_sound.Buffer = buffer;
                new_sound.Stream = stream;
                new_sound.Format = format;

                // 必要な数だけソースボイスを作成
                new_sound.Voices = new List<SlimDxGame.Asset.Sound.VoiceInfo>();
                for (int i = 0; i < num_of_sound; i++)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    var new_voice = new Asset.Sound.VoiceInfo();
                    new_voice.Resource = CreateSourceVoice(stream, format, buffer);
                    new_sound.Voices.Add(new_voice);
                }
                
            }catch(SystemException){
                new_sound = null;
            }
            return new_sound;
        }

        /// <summary>
        /// 音楽をファイルから作成します。
        /// </summary>
        /// <param name="filename">ファイル名</param>
        /// <param name="loop_begin">ループ開始地点()</param>
        /// <param name="loop_length">ループの長さ()</param>
        /// <returns>生成した音楽オブジェクト</returns>
        static public Asset.Music CreateMusicFromFile(string filename, int loop_begin, int loop_length)
        {
            var new_music = new Asset.Music();
            try
            {
                MemoryStream stream;
                WaveFormat format;
                AudioBuffer buffer = new AudioBuffer();
                // データをロードする
                LoadDataFromFile(out stream, out format, filename);
                
                // サウンドに設定
                buffer.AudioData = stream;
                buffer.AudioBytes = (int)stream.Length;
                buffer.Flags = BufferFlags.EndOfStream;
                buffer.LoopBegin = loop_begin;
                buffer.LoopLength = loop_length;
                buffer.LoopCount = XAudio2.LoopInfinite;

                new_music.Resource = CreateSourceVoice(stream, format, buffer);
                new_music.Buffer = buffer;
                new_music.Stream = stream;
                new_music.Format = format;
            }
            catch (SystemException)
            {
                new_music = null;
            }

            return new_music;
        }
    }
}
