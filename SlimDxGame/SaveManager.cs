using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlimDxGame
{
    struct SaveData
    {
        public int CollectedCoinNum { get; set; }
        public int LeftTime { get; set; }
        public int Score { get; set; }
    }

    class SaveManager
    {
        static public void Load(out SaveData data, string filename)
        {
            data = new SaveData();
            try
            {
                var array = File.ReadAllBytes(filename);
                var coin_num = BitConverter.ToInt32(array, 0);
                var left_time = BitConverter.ToInt32(array, 4);
                var score = BitConverter.ToInt32(array, 8);
                if (coin_num * 100 + left_time * 10 == score)
                {
                    data.CollectedCoinNum = coin_num;
                    data.LeftTime = left_time;
                    data.Score = score;
                }
            }catch(SystemException)
            {

            }
        }

        static public void Save(SaveData data, string filename)
        {
            List<byte> byte_data = new List<byte>();
            byte_data.AddRange(BitConverter.GetBytes(data.CollectedCoinNum));
            byte_data.AddRange(BitConverter.GetBytes(data.LeftTime));
            byte_data.AddRange(BitConverter.GetBytes(data.Score));
            File.WriteAllBytes(filename, byte_data.ToArray());
        }
    }
}
