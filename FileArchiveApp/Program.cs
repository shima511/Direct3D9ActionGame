using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileArchiver;

namespace FileArchiveApp
{
    class Program
    {
        static void AskNames(string[] args, string folder_name, string file_name)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("入力するルートフォルダ名を入力して下さい。");
                folder_name = Console.ReadLine();
            }
            Console.WriteLine("出力するファイル名を入力して下さい。");
            file_name = Console.ReadLine();
        }

        static void Main(string[] args)
        {
            string folder_name = "";
            string file_name = "";
            DataWriter writer = new DataWriter();
            DataReader reader = new DataReader();
            if (args.Length < 2)
            {
                AskNames(args, folder_name, file_name);
            }
            else
            {
                folder_name = args[0];
                file_name = args[1];
            }
            try
            {
                writer.Write(folder_name, file_name);
                Console.WriteLine(writer.HeaderSize);
                Console.WriteLine("ファイル作成に成功しました。");
            }
            catch (SystemException ex)
            {
                Console.WriteLine("ファイル作成に失敗しました。");
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("キー入力で終了");
            Console.ReadLine();
        }
    }
}
