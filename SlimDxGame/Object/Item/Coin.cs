using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object.Item
{
    class Coin : Object.Base.Model, Object.ICollisionObject, IBase
    {
        Collision.Shape.Point HitCollision;
        readonly int score = 10;

        public Coin()
        {
            HitCollision = new Collision.Shape.Point();
            IsCatched = false;
        }

        public bool IsCatched { get; set; }
        public void Dispatch(ICollisionObject obj)
        {

        }
        public void Hit(Player player)
        {
            if (player.RightSideCollision.Hit(HitCollision))
            {
            }
        }
        public void Hit(Ground.Floor floor)
        {

        }
        public void Hit(Ground.Ceiling ceiling)
        {

        }
        public void Hit(Ground.RightWall wall)
        {

        }
        public void Hit(Ground.LeftWall wall)
        {

        }
    }
}
