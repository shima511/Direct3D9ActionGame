using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.Object
{
    public interface IBase
    {
        void Update();
        void InputAction(KeyEventArgs e);
        void Draw(SlimDX.Direct3D9.Device dev);
    }
}
