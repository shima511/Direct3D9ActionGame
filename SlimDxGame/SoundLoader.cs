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

}
