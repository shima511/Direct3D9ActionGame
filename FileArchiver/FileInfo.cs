using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileArchiver
{
    struct FileInfo
    {
        public string Name { get; set; }
        public uint Size { get; set; }
        public uint OffSet { get; set; }
    }
}
