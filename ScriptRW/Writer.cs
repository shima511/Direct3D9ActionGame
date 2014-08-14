using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ScriptRW
{
    internal class Writer
    {
        void AddSizeInfo(List<byte> data_array, Properties properties)
        {
            data_array.AddRange(BitConverter.GetBytes(properties.Decolations.Count));
            data_array.AddRange(BitConverter.GetBytes(properties.Items.Count));
            data_array.AddRange(BitConverter.GetBytes(properties.Enemies.Count));
            data_array.AddRange(BitConverter.GetBytes(properties.Textures.Count));
        }

        void AddPropertyInfo(List<byte> data_array, List<ObjectProperty> obj_properties)
        {
            foreach (var item in obj_properties)
            {
                data_array.AddRange(BitConverter.GetBytes(item.Id));
                data_array.AddRange(BitConverter.GetBytes(Encoding.Unicode.GetByteCount(item.Name)));
                data_array.AddRange(Encoding.Unicode.GetBytes(item.Name));
                data_array.AddRange(BitConverter.GetBytes(Encoding.Unicode.GetByteCount(item.AssetPath)));
                data_array.AddRange(Encoding.Unicode.GetBytes(item.AssetPath));
            }
        }

        public void WriteAsBinary(string output_file, Properties properties)
        {
            List<byte> data_array = new List<byte>();

            AddSizeInfo(data_array, properties);
            AddPropertyInfo(data_array, properties.Decolations);
            AddPropertyInfo(data_array, properties.Items);
            AddPropertyInfo(data_array, properties.Enemies);
            AddPropertyInfo(data_array, properties.Textures);

            File.WriteAllBytes(output_file, data_array.ToArray());
        }
    }
}
