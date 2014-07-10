using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
            byte[] bytes = reader.ReadBytes(sizeof(float) * 2);
            float x = BitConverter.ToSingle(bytes, 0);
            float y = BitConverter.ToSingle(bytes, sizeof(float));
            objects.Player = new Property.Player() { Position = new SlimDX.Vector3(x, y, 0.0f)};
        }

        void ReadStageData(ref Objects objects)
        {
            byte[] bytes = reader.ReadBytes(sizeof(int) * 5);
            int top = BitConverter.ToInt32(bytes, 0);
            int right = BitConverter.ToInt32(bytes, sizeof(int));
            int bottom = BitConverter.ToInt32(bytes, sizeof(int) * 2);
            int left = BitConverter.ToInt32(bytes, sizeof(int) * 3);
            int limit_time = BitConverter.ToInt32(bytes, sizeof(int) * 4);
            objects.Stage = new Property.Stage() { 
                LimitLine = new System.Drawing.Rectangle() { X = left, Y = top, Height = bottom - top, Width = right - left },
                LimitTime = limit_time
            };
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
                    Valid = true;
                }catch(SystemException)
                {
                    Valid = false;
                }
            }
        }
    }
}
