using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileArchiver
{
    class Program
    {
        static void ReadFilesInfo(string dir, List<AssetInfo> files_info)
        {
            string[] files = Directory.GetFiles(dir);
            foreach (var item in files)
            {
                FileInfo f = new FileInfo(item);
                files_info.Add(new AssetInfo()
                {
                    Name = f.Name,
                    Size = f.Length
                });
            }

            string[] dirs = Directory.GetDirectories(dir);
            foreach (var item in dirs)
            {
                ReadFilesInfo(item, files_info);
            }
        }



        static void Main(string[] args)
        {
            List<AssetInfo> assets_info = new List<AssetInfo>();
            if (args.Length == 0)
            {
                args = new string[] { "Assets" };
            }
            
            // ファイルの情報を読み込む
            try
            {
                ReadFilesInfo(args[0], assets_info);
            }catch(DirectoryNotFoundException)
            {
                Console.WriteLine("存在しないディレクトリです。");
                return;
            }

            // オフセット値・実データを追加していく


            // 結果を表示
            foreach (var item in assets_info)
            {
                Console.WriteLine(item.Name + ":" + item.Size);
            }
        }
    }
}
