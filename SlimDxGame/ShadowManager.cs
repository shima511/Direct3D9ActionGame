using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame
{
    class ShadowManager : Component.IUpdateObject
    {
        public Object.Shadow Shadow { private get; set; }
        public List<Object.Ground.Base> Grounds { private get; set; }

        public void Update()
        {
        }
    }
}
