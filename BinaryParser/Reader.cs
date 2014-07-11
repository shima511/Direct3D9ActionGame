using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace BinaryParser
{
    public class Reader
    {
        BinaryReader reader;
        int sum;
        
        /// <summary>
        /// 読み込んだデータが正しいデータである場合、trueを返します。
        /// </summary>
        public bool Valid { get; private set; }

        void ReadSum()
        {
            byte[] bytes = reader.ReadBytes(sizeof(int));
            sum = BitConverter.ToInt32(bytes, 0);
        }

        void SetCapacity(ref Objects objects)
        {
            objects.Collisions = new List<Property.Collision>();
            objects.Items = new List<Property.Item>();
            objects.Decolations = new List<Property.Decolation>();
            objects.Enemies = new List<Property.Enemy>();
            byte[] bytes = reader.ReadBytes(sizeof(int) * 4);
            objects.Collisions.Capacity = BitConverter.ToInt32(bytes, 0);
            objects.Items.Capacity = BitConverter.ToInt32(bytes, sizeof(int));
            objects.Decolations.Capacity = BitConverter.ToInt32(bytes, sizeof(int) * 2);
            objects.Enemies.Capacity = BitConverter.ToInt32(bytes, sizeof(int) * 3);
        }

        void CheckSumData(ref Objects objects)
        {
            if (sum != (objects.Collisions.Capacity + objects.Items.Capacity + objects.Enemies.Capacity + objects.Decolations.Capacity))
            {
                throw new SystemException("チェックサムの値が異なっています。");
            }
        }

        void ReadPlayerData(ref Objects objects)
        {
            var player = new Property.Player();
            byte[] bytes = reader.ReadBytes(Marshal.SizeOf(player));
            player.Position = new SlimDX.Vector2(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, sizeof(float)));
            objects.Player = player;
        }

        void ReadStageData(ref Objects objects)
        {
            var stage = new Property.Stage();
            byte[] bytes = reader.ReadBytes(Marshal.SizeOf(stage));
            int top = BitConverter.ToInt32(bytes, 0);
            int right = BitConverter.ToInt32(bytes, sizeof(int));
            int bottom = BitConverter.ToInt32(bytes, sizeof(int) * 2);
            int left = BitConverter.ToInt32(bytes, sizeof(int) * 3);
            int limit_time = BitConverter.ToInt32(bytes, sizeof(int) * 4);
            stage.LimitLine = new System.Drawing.Rectangle() { 
                X = left,
                Y = top,
                Height = bottom - top,
                Width = right - left
            };
            stage.LimitTime = limit_time;
            objects.Stage = stage;
        }

        void ReadCollisionsData(ref Objects objects)
        {
            objects.Collisions = new List<Property.Collision>();
            for (int i = 0; i < objects.Collisions.Capacity; i++)
            {
                var new_collision = new Property.Collision();
                byte[] bytes = reader.ReadBytes(Marshal.SizeOf(new_collision));
                new_collision.StartingPoint = new SlimDX.Vector2(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, sizeof(float)));
                new_collision.TerminatePoint = new SlimDX.Vector2(BitConverter.ToSingle(bytes, sizeof(float) * 2), BitConverter.ToSingle(bytes, sizeof(float) * 3));
                new_collision.TypeId = BitConverter.ToInt32(bytes, sizeof(float) * 4);
                objects.Collisions[i] = new_collision;
            }
        }

        void ReadItemsData(ref Objects objects)
        {
            objects.Items = new List<Property.Item>();
            for (int i = 0; i < objects.Items.Capacity; i++)
            {
                var new_item = new Property.Item();
                byte[] bytes = reader.ReadBytes(Marshal.SizeOf(new_item));
                new_item.Position = new SlimDX.Vector2(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, sizeof(float)));
                new_item.TypeId = BitConverter.ToInt32(bytes, sizeof(float) * 2);
                objects.Items[i] = new_item;
            }
        }

        void ReadDecolationsData(ref Objects objects)
        {
            objects.Decolations = new List<Property.Decolation>();
            for (int i = 0; i < objects.Decolations.Capacity; i++)
            {
                var new_item = new Property.Decolation();
                byte[] bytes = reader.ReadBytes(Marshal.SizeOf(new_item));

                new_item.Position = new SlimDX.Vector3(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, sizeof(float)), BitConverter.ToSingle(bytes, sizeof(float) * 2));
                new_item.TypeId = BitConverter.ToInt32(bytes, sizeof(float) * 3);
                objects.Decolations[i] = new_item;
            }
        }

        void ReadEnemiesData(ref Objects objects)
        {
            objects.Enemies = new List<Property.Enemy>();
            for (int i = 0; i < objects.Enemies.Capacity; i++)
            {
                var new_item = new Property.Enemy();
                byte[] bytes = reader.ReadBytes(Marshal.SizeOf(new_item));
                new_item.Position = new SlimDX.Vector2(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, sizeof(float)));
                new_item.TypeId = BitConverter.ToInt32(bytes, sizeof(float) * 2);
                objects.Enemies[i] = new_item;
            }
        }

        public void Read(string filename, out Objects objects)
        {
            objects = new Objects();
            using (reader = new BinaryReader(File.OpenRead(filename)))
            {
                try
                {
                    ReadSum();
                    SetCapacity(ref objects);
                    CheckSumData(ref objects);
                    ReadPlayerData(ref objects);
                    ReadStageData(ref objects);
                    ReadCollisionsData(ref objects);
                    ReadItemsData(ref objects);
                    ReadDecolationsData(ref objects);
                    ReadEnemiesData(ref objects);
                    Valid = true;
                }catch(SystemException)
                {
                    Valid = false;
                }
            }
        }
    }
}
