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
        static List<SoundLoader> loader = new List<SoundLoader>();

        static AudioMediaFactory()
        {
            loader.Add(new WavFileLoader());
        }

        static void LoadDataFromMemory(out MemoryStream stream, out WaveFormat format, byte[] data_array)
        {
            foreach (var item in loader)
            {
                if (item.LoadFromMemory(out stream, out format, data_array)) return;
            }
            stream = null;
            format = null;
        }

        static void LoadDataFromFile(out MemoryStream stream, out WaveFormat format, string filename){
            foreach (var item in loader)
            {
                if (item.LoadFromFile(out stream, out format, filename)) return;
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
            buffer.AudioData.Seek(0, SeekOrigin.Begin);
            return buffer;
        }

        static private SourceVoice CreateSourceVoice(MemoryStream stream, WaveFormat format, AudioBuffer buffer)
        {
            var sourceVoice = new SourceVoice(Device, format);
            buffer.AudioData.Seek(0, SeekOrigin.Begin);
            sourceVoice.FlushSourceBuffers();
            sourceVoice.SubmitSourceBuffer(buffer);
            return sourceVoice;
        }

        /// <summary>
        /// バイト列から音声を作成します。
        /// </summary>
        /// <param name="data_array">バイトのデータ列</param>
        /// <param name="num_of_sound">必要なボイス数</param>
        /// <returns>生成した音声オブジェクト</returns>
        static public Asset.Sound CreateSoundFromMemory(byte[] data_array, int num_of_sound = 10)
        {
            var new_sound = new Asset.Sound();
            try
            {
                // 必要な数だけソースボイスを作成
                new_sound.Voices = new List<SlimDxGame.Asset.Sound.VoiceInfo>();
                for (int i = 0; i < num_of_sound; i++)
                {
                    MemoryStream stream;
                    WaveFormat format;
                    // データをロードする
                    LoadDataFromMemory(out stream, out format, data_array);
                    stream.Seek(0, SeekOrigin.Begin);

                    var buffer = CreateAudioBuffer(stream);
                    // サウンドに設定
                    new_sound.Buffer = buffer;
                    new_sound.Stream = stream;
                    new_sound.Format = format;

                    var new_voice = new Asset.Sound.VoiceInfo();
                    new_voice.Resource = CreateSourceVoice(stream, format, buffer);
                    new_sound.Voices.Add(new_voice);
                }

            }
            catch (SystemException)
            {
                new_sound = null;
            }

            return new_sound;
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
        /// バイト列から音楽を作成します。
        /// </summary>
        /// <param name="data_array">データ列</param>
        /// <param name="loop_begin"></param>
        /// <param name="loop_length"></param>
        /// <returns></returns>
        static public Asset.Music CreateMusicFromMemory(byte[] data_array, int loop_begin, int loop_length)
        {
            var new_music = new Asset.Music();
            try
            {
                MemoryStream stream;
                WaveFormat format;
                AudioBuffer buffer = new AudioBuffer();
                // データをロードする
                LoadDataFromMemory(out stream, out format, data_array);

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
