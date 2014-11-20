using System;
using SlimDX.Multimedia;
using System.IO;

namespace SlimDxGame.AssetFactory
{
    interface SoundLoader
    {
        /// <summary>
        /// 指定されたファイルからデータを読み込みます。
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="format"></param>
        /// <param name="filename">ファイル名</param>
        /// <returns>読み込みに成功した場合trueを返します。</returns>
        bool LoadFromFile(out MemoryStream stream, out WaveFormat format, string filename);

        /// <summary>
        /// バイト列からデータを読み込みます。
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="format"></param>
        /// <param name="data_array"></param>
        /// <returns>読み込みに成功した場合trueを返します。</returns>
        bool LoadFromMemory(out MemoryStream stream, out WaveFormat format, byte[] data_array);
    }

    class WavFileLoader : SoundLoader
    {
        void AllocateData(Stream s, out WaveFormat format, out MemoryStream stream)
        {
            using(var wavStream = new WaveStream(s))
            {
                format = wavStream.Format;
                byte[] wavData = new byte[wavStream.Length];
                wavStream.Read(wavData, 0, (int)wavStream.Length);
                stream = new MemoryStream(wavData);
            }
        }

        public bool LoadFromFile(out MemoryStream stream, out WaveFormat format, string filename)
        {
            bool ret_val = true;
            try
            {
                using (var s = System.IO.File.OpenRead(filename))
                {
                    AllocateData(s, out format, out stream);
                }
            }
            catch (SystemException)
            {
                stream = null;
                format = null;
                ret_val = false;
            }
            return ret_val;
        }

        public bool LoadFromMemory(out MemoryStream stream, out WaveFormat format, byte[] data_array)
        {
            bool ret_val = true;
            try
            {
                using (var s = new MemoryStream(data_array))
                {
                    AllocateData(s, out format, out stream);
                }
            }
            catch(SystemException)
            {
                stream = null;
                format = null;
                ret_val = false;
            }
            return ret_val;
        }
    }

}
