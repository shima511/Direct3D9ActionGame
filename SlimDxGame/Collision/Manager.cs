using System;
using System.Collections.Generic;

namespace SlimDxGame.Collision
{
    class Manager : List<Object.IFieldObject>, Component.IUpdateObject, Component.IDrawableObject
    {
        bool _is_visible = true;
        public bool IsActive { get; set; }
        public bool IsVisible { get { return _is_visible; } set { _is_visible = value; } }
        public Object.Player Player { private get; set; }
        public void Update()
        {
            Player.IsInTheAir = true;
            Player.IsBesideOfRightWall = false;
            Player.IsBesideOfLeftWall = false;
            foreach (var obj in this)
            {
                if(obj.Spawnable) obj.Hit(Player);
            }
        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }

        [System.Diagnostics.Conditional("DEBUG")]
        void DebugDraw(SlimDX.Direct3D9.Device dev)
        {
            foreach (var item in this)
            {
                item.DrawHitRange(dev);
            }
        }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            DebugDraw(dev);
        }
    }
}
