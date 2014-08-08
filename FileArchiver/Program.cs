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
            System.Threading.Thread.Sleep(1000);
        }
    }
}
