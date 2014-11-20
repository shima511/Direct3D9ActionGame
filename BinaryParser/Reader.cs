using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace StageRW
{
    public class Reader
    {
        int sum = 0;
        int tail = 0;
        
        /// <summary>
        /// 読み込んだデータが正しいデータである場合、trueを返します。
        /// </summary>
        public bool Valid { get; private set; }

        /// <summary>
        /// エラーの原因をメッセージで表示します。
        /// </summary>
        public string ErrorMessage { get; private set; }

        void ReadSum(byte[] data_array)
        {
            sum = GetValueFromByteInt32(data_array, ref tail);
        }

        void SetCapacity(byte[] data_array, ref Objects objects)
        {
            objects.Collisions = new List<Property.Collision>();
            objects.Items = new List<Property.Item>();
            objects.Decolations = new List<Property.Decolation>();
            objects.Enemies = new List<Property.Enemy>();

            objects.Collisions.Capacity = GetValueFromByteInt32(data_array, ref tail);
            objects.Items.Capacity = GetValueFromByteInt32(data_array, ref tail);
            objects.Decolations.Capacity = GetValueFromByteInt32(data_array, ref tail);
            objects.Enemies.Capacity = GetValueFromByteInt32(data_array, ref tail);
        }

        void CheckSumData(ref Objects objects)
        {
            if (sum != (objects.Collisions.Capacity + objects.Items.Capacity + objects.Enemies.Capacity + objects.Decolations.Capacity))
            {
                throw new SystemException("チェックサムの値が異なっています。");
            }
        }

        void ReadPlayerData(byte[] data_array, ref Objects objects)
        {
            var player = new Property.Player();
            player.Position = new SlimDX.Vector2(GetValueFromByteFloat(data_array, ref tail), GetValueFromByteFloat(data_array, ref tail));
            objects.Player = player;
        }

        void ReadStageData(byte[] data_array, ref Objects objects)
        {
            var stage = new Property.Stage();
            int top = GetValueFromByteInt32(data_array, ref tail);
            int right = GetValueFromByteInt32(data_array, ref tail);
            int bottom = GetValueFromByteInt32(data_array, ref tail);
            int left = GetValueFromByteInt32(data_array, ref tail);
            int limit_time = GetValueFromByteInt32(data_array, ref tail);
            stage.LimitLine = new System.Drawing.Rectangle() { 
                X = left,
                Y = top,
                Height = bottom - top,
                Width = right - left
            };
            stage.LimitTime = limit_time;
            objects.Stage = stage;
        }

        void ReadCollisionsData(byte[] data_array, ref Objects objects)
        {
            for (int i = 0; i < objects.Collisions.Capacity; i++)
            {
                var new_collision = new Property.Collision();
                new_collision.StartingPoint = new SlimDX.Vector2(GetValueFromByteFloat(data_array, ref tail), GetValueFromByteFloat(data_array, ref tail));
                new_collision.TerminatePoint = new SlimDX.Vector2(GetValueFromByteFloat(data_array, ref tail), GetValueFromByteFloat(data_array, ref tail));
                new_collision.TypeId = GetValueFromByteInt32(data_array, ref tail);
                objects.Collisions.Add(new_collision);
            }
        }

        void ReadItemsData(byte[] data_array, ref Objects objects)
        {
            for (int i = 0; i < objects.Items.Capacity; i++)
            {
                var new_item = new Property.Item();
                new_item.Position = new SlimDX.Vector2(GetValueFromByteFloat(data_array, ref tail), GetValueFromByteFloat(data_array, ref tail));
                new_item.TypeId = GetValueFromByteInt32(data_array, ref tail);
                objects.Items.Add(new_item);
            }
        }

        void ReadDecolationsData(byte[] data_array, ref Objects objects)
        {
            for (int i = 0; i < objects.Decolations.Capacity; i++)
            {
                var new_item = new Property.Decolation();
                new_item.Position = new SlimDX.Vector3(GetValueFromByteFloat(data_array, ref tail), GetValueFromByteFloat(data_array, ref tail), GetValueFromByteFloat(data_array, ref tail));
                new_item.TypeId = GetValueFromByteInt32(data_array, ref tail);
                objects.Decolations.Add(new_item);
            }
        }

        void ReadEnemiesData(byte[] data_array, ref Objects objects)
        {
            for (int i = 0; i < objects.Enemies.Capacity; i++)
            {
                var new_item = new Property.Enemy();
                new_item.Position = new SlimDX.Vector2(GetValueFromByteFloat(data_array, ref tail), GetValueFromByteFloat(data_array, ref tail));
                new_item.TypeId = GetValueFromByteInt32(data_array, ref tail);
                objects.Enemies.Add(new_item);
            }
        }

        /// <summary>
        /// ファイルからステージデータを読み込みます
        /// </summary>
        /// <param name="filename">ファイル名</param>
        /// <param name="objects">ステージオブジェクトの情報</param>
        public void Read(string filename, out Objects objects)
        {
            objects = new Objects();
            byte[] data_array = File.ReadAllBytes(filename);
            try
            {
                ReadSum(data_array);
                SetCapacity(data_array, ref objects);
                CheckSumData(ref objects);
                ReadPlayerData(data_array, ref objects);
                ReadStageData(data_array, ref objects);
                ReadCollisionsData(data_array, ref objects);
                ReadItemsData(data_array, ref objects);
                ReadDecolationsData(data_array, ref objects);
                ReadEnemiesData(data_array, ref objects);
                Valid = true;
            }catch(SystemException ex)
            {
                Valid = false;
                ErrorMessage = ex.Message;
            }

        }

        int GetValueFromByteInt32(byte[] byte_array, ref int tail_index)
        {
            int ret_val = BitConverter.ToInt32(byte_array, tail_index);
            tail_index += sizeof(int);
            return ret_val;
        }

        float GetValueFromByteFloat(byte[] byte_array, ref int tail_index)
        {
            float ret_val = BitConverter.ToSingle(byte_array, tail_index);
            tail_index += sizeof(float);
            return ret_val;
        }

        /// <summary>
        /// バイナリデータからステージデータを読み込みます
        /// </summary>
        /// <param name="byte_data">バイナリのデータ配列</param>
        /// <param name="objects">ステージオブジェクトの情報</param>
        public void Read(byte[] data_array, out Objects objects)
        {
            objects = new Objects();
            try
            {
                ReadSum(data_array);
                SetCapacity(data_array, ref objects);
                CheckSumData(ref objects);
                ReadPlayerData(data_array, ref objects);
                ReadStageData(data_array, ref objects);
                ReadCollisionsData(data_array, ref objects);
                ReadItemsData(data_array, ref objects);
                ReadDecolationsData(data_array, ref objects);
                ReadEnemiesData(data_array, ref objects);
                Valid = true;
            }
            catch (SystemException ex)
            {
                Valid = false;
                ErrorMessage = ex.Message;
            }
        }
    }
}
