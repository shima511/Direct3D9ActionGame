using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BinaryParser
{
    public class Writer
    {
        List<byte> byteArray = new List<byte>();


        public void Write(string filename, Objects objects)
        {
            BinaryWriter writer = new BinaryWriter(File.OpenRead(filename));
            writer.Write(byteArray.ToArray());
        }
    }
}
