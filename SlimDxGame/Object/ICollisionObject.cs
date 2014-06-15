using System;
using System.Collections.Generic;
using SlimDX;

namespace SlimDxGame.Object
{
    interface ICollisionObject : Component.IDrawableObject
    {
        void Dispatch(ICollisionObject obj);
        void Hit(Player player);
        void Hit(Ground.Floor floor);
        void Hit(Ground.Ceiling ceiling);
        void Hit(Ground.RightWall wall);
        void Hit(Ground.LeftWall wall);
    }
}
