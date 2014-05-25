using System;
using SlimDX.Multimedia;
using System.IO;

namespace SlimDxGame
{
    interface SoundLoader
    {
        bool LoadFromFile(out MemoryStream stream, out WaveFormat format,string filename);
    }

    class WavFileLoader : SoundLoader
    {
        public bool LoadFromFile(out MemoryStream stream, out WaveFormat format, string filename)
        {
            bool ret_val = true;
            var s = System.IO.File.OpenRead(filename);
            try
            {
                using (var wavStream = new WaveStream(s))
                {
                    format = wavStream.Format;
                    byte[] wavData = new byte[wavStream.Length];
                    wavStream.Read(wavData, 0, (int)wavStream.Length);
                    stream = new MemoryStream(wavData);
                }                
            }
            catch (SystemException)
            {
                stream = null;
                format = null;
                ret_val = false;
            }
            s.Close();
            return ret_val;
        }
    }

    class OggFileLoader : SoundLoader
    {
        private void ConvertFormat(ref WaveFormat wav_format, Tsukikage.Audio.OggDecodeStream oggStream){
            wav_format.AverageBytesPerSecond = wav_format.SamplesPerSecond * ((int)wav_format.BitsPerSample / 8);
            wav_format.SamplesPerSecond = oggStream.SamplesPerSecond;
            wav_format.Channels = (short)oggStream.Channels;
            wav_format.BitsPerSample = (short)oggStream.BitsPerSample;
            wav_format.FormatTag = WaveFormatTag.Pcm;
            wav_format.BlockAlignment = 2;
        }

        public bool LoadFromFile(out MemoryStream stream, out WaveFormat format, string filename)
        {
            bool ret_val = true;
            var s = System.IO.File.OpenRead(filename);
            try
            {
                Tsukikage.Audio.OggDecodeStream oggStream = new Tsukikage.Audio.OggDecodeStream(s);
                // フォーマットを取得
                WaveFormat f = new WaveFormat();
                ConvertFormat(ref f, oggStream);
                format = f;

                byte[] wmaData = new byte[oggStream.Length];
                oggStream.Read(wmaData, 0, (int)oggStream.Length);
                stream = new MemoryStream(wmaData);
            }
            catch (SystemException)
            {
                stream = null;
                format = null;
                ret_val = false;
            }
            s.Close();
            return ret_val;
        }
    }
}
