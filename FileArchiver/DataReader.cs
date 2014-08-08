using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileArchiver
{
    class DataReader : IDisposable
    {
        FileStream fstream;
        byte[] header;

        /// <summary>
        /// アーカイブされたファイルを開きます
        /// </summary>
        /// <param name="filename">ファイル名</param>
        public void Open(string filename)
        {
            if (fstream == null)
            {
                fstream = new FileStream(filename, FileMode.Open);
                byte[] header_size = new byte[sizeof(int)];
                fstream.Read(header_size, 0, sizeof(int));
                header = new byte[BitConverter.ToUInt32(header_size, 0)];
                fstream.Seek(0, SeekOrigin.Begin);
                fstream.Read(header, 0, BitConverter.ToInt32(header_size, 0));
            }
        }

        public AssetInfo ReadFileInfo(string filename)
        {
            AssetInfo info = new AssetInfo();

            byte[] target_byte = System.Text.Encoding.Unicode.GetBytes(filename);
            int byte_index;
            int start_index = 0;
            bool hit_frag;

            while(true)
            {
                byte_index = Array.IndexOf(header, target_byte[0], start_index);
                if (byte_index < 0 || start_index + target_byte.Length - 1 >= header.Length)
                {
                    byte_index = -1;
                    hit_frag = false;
                    break;
                }
                else
                {
                    hit_frag = true;
                }

                for (int i = 1; i < target_byte.Length - 1; i++)
                {
                    if (header[byte_index + i] != target_byte[i])
                    {
                        hit_frag = false;
                        break;
                    }
                }

                if (hit_frag == true)
                {
                    break;
                }
                else
                {
                    start_index = byte_index + 1;
                }
            }

            byte[] info_datas = new byte[System.Text.Encoding.Unicode.GetByteCount(filename) + sizeof(int) + sizeof(int)];
            for (int i = 0; i < info_datas.Length; i++)
            {
                info_datas[i] = header[i + byte_index];
            }
            info.Name = System.Text.Encoding.Unicode.GetString(info_datas, 0, System.Text.Encoding.Unicode.GetByteCount(filename));
            info.OffSet = BitConverter.ToInt32(info_datas, System.Text.Encoding.Unicode.GetByteCount(filename));
            info.Size = BitConverter.ToInt32(info_datas, System.Text.Encoding.Unicode.GetByteCount(filename) + sizeof(int));

            return info;
        }

        /// <summary>
        /// 指定されたファイル名のバイナリデータを取得します。
        /// </summary>
        /// <param name="name">ファイル名</param>
        /// <returns>バイト配列</returns>
        public byte[] GetBytes(string name)
        {
            var info = ReadFileInfo(name);
            byte[] ret_val = new byte[info.Size];
            fstream.Seek(0, SeekOrigin.Begin);
            fstream.Read(ret_val, (int)info.OffSet, (int)info.Size);
            return ret_val;
        }

        public void Dispose()
        {
            if (fstream != null)
            {
                fstream.Dispose();
            }
        }
    }
}
