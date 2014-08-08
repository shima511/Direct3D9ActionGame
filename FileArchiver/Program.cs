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
        static void Main(string[] args)
        {
            DataWriter writer = new DataWriter();
            DataReader reader = new DataReader();
            if (args.Length == 0)
            {
                args = new string[] { "Assets", "Out.dat"};
            }
            try
            {
                writer.Write(args[0], args[1]);
                Console.WriteLine(writer.HeaderSize);
                Console.WriteLine("ファイル作成に成功しました。");
            }
            catch(SystemException ex)
            {
                Console.WriteLine("ファイル作成に失敗しました。");
                Console.WriteLine(ex.Message);
            }
            try
            {
                reader.Open(args[1]);
                var info = reader.ReadFileInfo("S1.jpeg");
                Console.WriteLine(info.Name + ":" + info.Size + ":" + info.OffSet);
            }
            catch
            {
                Console.WriteLine("ロード失敗");
            }
            System.Threading.Thread.Sleep(1000);
        }
    }
}
