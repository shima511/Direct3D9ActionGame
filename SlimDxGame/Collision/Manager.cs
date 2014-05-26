using System;
using System.Collections.Generic;

namespace SlimDxGame.Collision
{
    class Manager : List<Object.ICollisionObject>, Component.IUpdateObject
    {
        public Object.Player Player { private get; set; }
        public void Update()
        {
            Player.IsOnTheGround = false;
            Player.IsInTheAir = true;
            foreach (var obj in this)
            {
                obj.Hit(Player);
            }
        }
    }
}
