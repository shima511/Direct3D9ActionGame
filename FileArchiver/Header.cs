using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileArchiver
{
    struct Header
    {
        public List<AssetInfo> Files { get; set; }
    }
}
