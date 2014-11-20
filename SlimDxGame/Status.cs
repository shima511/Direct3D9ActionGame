using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Status
{
    public class Charactor
    {
        public Charactor()
        {
            HP = 0;
        }
        public int HP { get; set; }
    }

    public class Stage
    {
        public uint Score { get; set; }
        public uint Time { get; set; }
    }
}
