using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserTest
{
    class Program
    {
        static void AddData(ref BinaryParser.Objects objects)
        {
            objects.Items = new List<BinaryParser.Property.Item>();
            objects.Player = new BinaryParser.Property.Player();
            objects.Stage = new BinaryParser.Property.Stage();
            objects.Collisions = new List<BinaryParser.Property.Collision>();
            objects.Decolations = new List<BinaryParser.Property.Decolation>();
            objects.Enemies = new List<BinaryParser.Property.Enemy>();
        }

        static void PrintData(ref BinaryParser.Objects objects)
        {
            Console.WriteLine("プレイヤー位置：" + objects.Player.Position);
            Console.WriteLine("境界線:" + objects.Stage.LimitLine);
            Console.WriteLine("時間制限：" + objects.Stage.LimitTime);
            Console.WriteLine("地形数：" + objects.Collisions.Count);
            foreach (var item in objects.Collisions)
            {
                Console.WriteLine("始点：" + item.StartingPoint);
                Console.WriteLine("終点：" + item.TerminatePoint);
                Console.WriteLine("タイプ：" + item.TypeId);
            }
            Console.WriteLine("アイテム数：" + objects.Items.Count);
            foreach (var item in objects.Items)
            {
                Console.WriteLine("地点：" + item.Position);
                Console.WriteLine("タイプ：" + item.TypeId);
            }
            Console.WriteLine("飾りの数：" + objects.Decolations.Count);
            foreach (var item in objects.Decolations)
            {
                Console.WriteLine("地点：" + item.Position);
                Console.WriteLine("タイプ：" + item.TypeId);
            }
            Console.WriteLine("敵の数：" + objects.Enemies.Count);
            foreach (var item in objects.Enemies)
            {
                Console.WriteLine("地点：" + item.Position);
                Console.WriteLine("タイプ：" + item.TypeId);
            }
        }

        static void Main(string[] args)
        {
            BinaryParser.Writer writer = new BinaryParser.Writer();
            BinaryParser.Objects objects = new BinaryParser.Objects();

            AddData(ref objects);

            writer.Write("out.dat", objects);

            BinaryParser.Reader reader = new BinaryParser.Reader();

            reader.Read("out.dat", out objects);

            PrintData(ref objects);

            if (reader.Valid)
            {
                Console.WriteLine("正しいデータ");
            }
            else
            {
                Console.WriteLine("Invalidデータ");
            }
        }
    }
}
