using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.Direct3D9;

namespace SlimDxGame.Effect
{
    class Light : Component.IDrawableObject
    {
        /// <summary>
        /// ライトのインデックス番号を取得・設定します。
        /// </summary>
        public int Index { get; set; }
        public bool IsVisible { get { return true; } set { } }
        /// <summary>
        /// ライトのオン・オフを行います。
        /// </summary>
        public bool EnableLight { get; set; }

        /// <summary>
        /// ライトの情報を取得・設定します。
        /// </summary>
        public SlimDX.Direct3D9.Light Property { get; set; }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            dev.SetRenderState(SlimDX.Direct3D9.RenderState.Lighting, true);
            dev.SetLight(Index, Property);
            dev.EnableLight(Index, EnableLight);
        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }
    }
}
