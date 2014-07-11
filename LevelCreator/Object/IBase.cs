using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.Object
{
    abstract class IBase
    {
        public abstract void Update();
        public virtual void InputAction(KeyEventArgs e)
        {

        }
        public abstract void Draw(SlimDX.Direct3D9.Device dev);
    }
}
