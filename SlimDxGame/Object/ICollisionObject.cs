using System;
using System.Collections.Generic;
using SlimDX;

namespace SlimDxGame.Object
{
    interface ICollisionObject
    {
        void DrawHitRange(SlimDX.Direct3D9.Device dev);
        void Dispatch(ICollisionObject obj);
        void Hit(Player player);
        void Hit(Ground.Floor floor);
        void Hit(Ground.Ceiling ceiling);
        void Hit(Ground.RightWall wall);
    }
}
