using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ScriptRW
{
    public class Reader
    {
        /// <summary>
        /// スクリプトファイルからプロパティを読み込みます。
        /// </summary>
        /// <param name="properties">プロパティズオブジェクト</param>
        /// <param name="filename">スクリプトファイル名</param>
        public void Read(out Properties properties, string filename)
        {
            properties = new Properties()
            {
                Decolations = new List<ObjectProperty>(),
                Enemies = new List<ObjectProperty>(),
                Items = new List<ObjectProperty>(),
                Textures = new List<ObjectProperty>()
            };
            using (StreamReader s_reader = new StreamReader(filename))
            {
                LineNode l_node = new LineNode()
                {
                    Properties = properties
                };
                while (!s_reader.EndOfStream)
                {
                    l_node.Parse(s_reader.ReadLine(), null);
                }
            }
        }

        int GetIntValueFromByteArray(byte[] data_array, ref int tail)
        {
            var ret_val = BitConverter.ToInt32(data_array, tail);
            tail += sizeof(int);
            return ret_val;
        }

        void AnalyzePropertiesValue(byte[] data_array, List<ObjectProperty> list, ref int tail)
        {
            for (int i = 0; i < list.Capacity; i++)
            {
                var new_prop = new ObjectProperty();
                new_prop.Id = GetIntValueFromByteArray(data_array, ref tail);
                var name_byte_length = GetIntValueFromByteArray(data_array, ref tail);
                new_prop.Name = Encoding.Unicode.GetString(data_array, tail, name_byte_length);
                tail += name_byte_length;
                var asset_path_byte_length = GetIntValueFromByteArray(data_array, ref tail);
                new_prop.AssetPath = Encoding.Unicode.GetString(data_array, tail, asset_path_byte_length);
                tail += asset_path_byte_length;
                list.Add(new_prop);
            }
        }

        /// <summary>
        /// バイナリからプロパティを読み込みます。
        /// </summary>
        /// <param name="properties">プロパティズオブジェクト</param>
        /// <param name="data_array">バイナリデータ</param>
        public void Read(out Properties properties, byte[] data_array)
        {
            properties = new Properties()
            {
                Decolations = new List<ObjectProperty>(),
                Enemies = new List<ObjectProperty>(),
                Items = new List<ObjectProperty>(),
                Textures = new List<ObjectProperty>()
            };
            int tail = 0;
            properties.Decolations.Capacity = GetIntValueFromByteArray(data_array, ref tail);
            properties.Items.Capacity = GetIntValueFromByteArray(data_array, ref tail);
            properties.Enemies.Capacity = GetIntValueFromByteArray(data_array, ref tail);
            properties.Textures.Capacity = GetIntValueFromByteArray(data_array, ref tail);

            AnalyzePropertiesValue(data_array, properties.Decolations, ref tail);
            AnalyzePropertiesValue(data_array, properties.Items, ref tail);
            AnalyzePropertiesValue(data_array, properties.Enemies, ref tail);
            AnalyzePropertiesValue(data_array, properties.Textures, ref tail);
        }
    }
}
