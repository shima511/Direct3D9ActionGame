using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageRWTest
{
    class Program
    {
        static void AddData(ref StageRW.Objects objects)
        {
            objects.Items = new List<StageRW.Property.Item>();
            objects.Player = new StageRW.Property.Player();
            objects.Stage = new StageRW.Property.Stage();
            objects.Collisions = new List<StageRW.Property.Collision>();
            objects.Decolations = new List<StageRW.Property.Decolation>();
            objects.Enemies = new List<StageRW.Property.Enemy>();

            objects.Collisions.Add(new StageRW.Property.Collision()
            {
                StartingPoint = new SlimDX.Vector2(2.0f, 1.0f),
                TerminatePoint = new SlimDX.Vector2(),
                TypeId = 1
            });
        }

        static void PrintData(ref StageRW.Objects objects)
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
            StageRW.Writer writer = new StageRW.Writer();
            StageRW.Objects objects = new StageRW.Objects();

            AddData(ref objects);

            writer.Write("out.dat", objects);

            StageRW.Reader reader = new StageRW.Reader();

            reader.Read("out.dat", out objects);

            PrintData(ref objects);

            if (reader.Valid)
            {
                Console.WriteLine("正しいデータ");
            }
            else
            {
                Console.WriteLine("Invalidデータ");
                Console.WriteLine(reader.ErrorMessage);
            }
        }
    }
}
