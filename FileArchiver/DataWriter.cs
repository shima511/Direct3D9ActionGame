using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileArchiver
{
    class DataWriter
    {
        /// <summary>
        /// ヘッダサイズ
        /// </summary>
        public int HeaderSize { get; private set; }
        List<AssetInfo> assets_info = new List<AssetInfo>();
        List<byte> byte_array = new List<byte>();

        /// <summary>
        /// ファイルの情報を読み込む
        /// </summary>
        /// <param name="dir">ディレクトリ</param>
        /// <param name="files_info">ファイルの情報</param>
        void ReadFilesInfo(string dir, List<AssetInfo> assets_info)
        {
            string[] files = Directory.GetFiles(dir);
            foreach (var item in files)
            {
                FileInfo f = new FileInfo(item);
                assets_info.Add(new AssetInfo()
                {
                    FullName = Path.Combine(dir, f.Name),
                    Name = f.Name,
                    Size = (int)f.Length,
                    OffSet = 0
                });
            }

            string[] dirs = Directory.GetDirectories(dir);
            foreach (var item in dirs)
            {
                ReadFilesInfo(item, assets_info);
            }
        }

        /// <summary>
        /// ヘッダサイズを計算します。
        /// </summary>
        void CountHeaderSize()
        {
            HeaderSize = 0;
            HeaderSize += sizeof(int);
            foreach (var item in assets_info)
            {
                HeaderSize += sizeof(int);
                HeaderSize += sizeof(int);
                HeaderSize += System.Text.Encoding.Unicode.GetByteCount(item.Name);
            }
        }

        /// <summary>
        /// オフセット情報を追加する
        /// </summary>
        void SetOffSetInfo()
        {
            var tail = HeaderSize;
            for (int i = 0; i < assets_info.Count; i++)
            {
                var tmp = assets_info[i];
                tmp.OffSet = tail;
                tail += tmp.Size;
                assets_info[i] = tmp;
            }
        }

        /// <summary>
        /// ヘッダの情報を格納する
        /// </summary>
        void SetHeaderInfo()
        {
            // ヘッダサイズ
            byte_array.AddRange(BitConverter.GetBytes(HeaderSize));
            // Asset情報
            foreach (var item in assets_info)
            {
                byte_array.AddRange(System.Text.Encoding.Unicode.GetBytes(item.Name));
                byte_array.AddRange(BitConverter.GetBytes(item.OffSet));
                byte_array.AddRange(BitConverter.GetBytes(item.Size));
            }
        }

        /// <summary>
        /// 実データを格納する
        /// </summary>
        void SetDataInfo()
        {
            foreach (var item in assets_info)
            {
                byte_array.AddRange(File.ReadAllBytes(item.FullName));
            }
        }

        /// <summary>
        /// ディレクトリ以下のファイルを全てバイナリとして書き込みます。
        /// </summary>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"></exception>
        /// <param name="base_dir">ベースディレクトリ名</param>
        /// <param name="out_filename">出力ファイル名</param>
        public void Write(string base_dir, string out_filename)
        {
            ReadFilesInfo(base_dir, assets_info);

            CountHeaderSize();

            SetOffSetInfo();

            SetHeaderInfo();

            SetDataInfo();

            File.WriteAllBytes(out_filename, byte_array.ToArray());
        }
    }
}
