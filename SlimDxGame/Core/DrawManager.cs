using System;
using System.Collections.Generic;
using SlimDX.Direct3D9;

namespace SlimDxGame.Core
{
    class DrawManager
    {
        public void DrawBegin(SlimDX.Direct3D9.Device dev){
            try
            {
                dev.Clear(SlimDX.Direct3D9.ClearFlags.Target | SlimDX.Direct3D9.ClearFlags.ZBuffer, System.Drawing.Color.DarkBlue, 1.0F, 0);
                dev.BeginScene();
            }
            catch (SlimDX.Direct3D9.Direct3D9Exception ex)
            {
                if (ex.ResultCode == ResultCode.DeviceLost)
                {
                    MikuMikuDance.SlimDX.SlimMMDXCore.Instance.OnLostDevice();
                }
            }
        }

        public void DrawObjects(SlimDX.Direct3D9.Device d3d_dev, SlimDX.Direct3D9.Sprite sprite_dev, List<List<Component.IDrawableObject>> layers)
        {
            //Draw3D
            foreach (var layer in layers)
            {
                foreach (var obj in layer)
                {
                    if (obj.IsVisible)
                    {
                        obj.Draw3D(d3d_dev);
                    }
                }
            }

            //Draw2D
            sprite_dev.Begin(SpriteFlags.AlphaBlend);
            foreach(var layer in layers)
            {
                foreach (var obj in layer)
                {
                    if (obj.IsVisible)
                    {
                        obj.Draw2D(sprite_dev);
                    }
                }
            }
            sprite_dev.End();
        }

        public void DrawEnd(SlimDX.Direct3D9.Device dev)
        {
            dev.EndScene();
            dev.Present();
        }
    }
}
