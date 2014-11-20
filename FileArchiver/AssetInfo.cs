using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileArchiver
{
    public struct AssetInfo
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int OffSet { get; set; }
    }
}
