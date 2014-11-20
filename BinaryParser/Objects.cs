using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageRW
{
    public struct Objects
    {
        public List<Property.Collision> Collisions { get; set; }
        public List<Property.Item> Items { get; set; }
        public List<Property.Decolation> Decolations { get; set; }
        public List<Property.Enemy> Enemies { get; set; }
        public Property.Stage Stage { get; set; }
        public Property.Player Player { get; set; }
    }
}
