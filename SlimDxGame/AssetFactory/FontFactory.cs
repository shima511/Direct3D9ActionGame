using System;
using System.Collections.Generic;
using SlimDX.Direct3D9;

namespace SlimDxGame.AssetFactory
{
    class FontFactory
    {
        static public SlimDX.Direct3D9.Device Device { private get; set; }

        static public Asset.Font CreateFont(System.Drawing.Font info){
            var new_font = new Asset.Font();
            var font_resource = new Font(Device, info);

            new_font.Info = info;
            new_font.Resource = font_resource;

            return new_font;
        }
    }
}
