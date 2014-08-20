using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptRW;

namespace ScriptConverter
{
    class Program
    {
        static void PrintPropertyList(List<ObjectProperty> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine("{0} {1} {2}", item.Id, item.Name, item.AssetPath);
            }
        }

        static void PrintProperties(Properties p)
        {
            Console.WriteLine("Decolation:");
            PrintPropertyList(p.Decolations);
            Console.WriteLine("Item:");
            PrintPropertyList(p.Items);
            Console.WriteLine("Enemy:");
            PrintPropertyList(p.Enemies);
            Console.WriteLine("Texture:");
            PrintPropertyList(p.Textures);
        }

        static void ConvertData(string input_file = null, string output_file = null)
        {
            if (input_file == null)
            {
                Console.WriteLine("入力ファイル名を入力して下さい。");
                input_file = Console.ReadLine();
            }
            if (output_file == null)
            {
                Console.WriteLine("出力ファイル名を入力して下さい。");
                output_file = Console.ReadLine();
            }
            Properties p;
            Reader reader = new Reader();
            reader.Read(out p, input_file);

            PrintProperties(p);

            Writer writer = new Writer();
            writer.WriteAsBinary(output_file, p);
        }

        static void Main(string[] args)
        {
            try
            {
                switch (args.Length)
                {
                    case 0:
                        ConvertData();
                        break;
                    case 1:
                        ConvertData(args[0]);
                        break;
                    default:
                        ConvertData(args[0], args[1]);
                        break;
                }
            }
            catch (SystemException ex)
            {
                Console.WriteLine("コンバートに失敗しました。");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return;
            }
            Console.WriteLine("コンバートに成功しました。");
            Console.ReadKey();
        }
    }
}
