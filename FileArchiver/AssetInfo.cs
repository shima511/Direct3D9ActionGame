using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileArchiver
{
    struct AssetInfo
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public uint OffSet { get; set; }
    }
}
