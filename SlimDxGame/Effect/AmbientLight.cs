using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.Direct3D9;

namespace SlimDxGame.Effect
{
    class AmbientLight : Component.IDrawableObject
    {
        public bool IsVisible { get { return true; } set { } }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            dev.EnableLight(0, true);
            dev.SetRenderState(SlimDX.Direct3D9.RenderState.Lighting, true);

            dev.SetRenderState(SlimDX.Direct3D9.RenderState.Ambient, System.Drawing.Color.White.ToArgb());
        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }
    }
}
