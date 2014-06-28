using System;
using System.Collections.Generic;

namespace SlimDxGame.Collision
{
    class Manager : List<Object.ICollisionObject>, Component.IUpdateObject, Component.IDrawableObject
    {
        bool _is_visible = true;
        public bool IsVisible { get { return _is_visible; } set { _is_visible = value; } }
        public Object.Player Player { private get; set; }
        public void Update()
        {
            Player.IsOnTheGround = false;
            Player.IsInTheAir = true;
            Player.IsBesideOfRightWall = false;
            Player.IsBesideOfLeftWall = false;
            foreach (var obj in this)
            {
                obj.Hit(Player);
            }
        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
#if DEBUG
            foreach (var item in this)
            {
                item.Draw3D(dev);
            }
#endif
        }
    }
}
