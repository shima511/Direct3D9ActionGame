using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object.Item
{
    class Coin : Object.Base.Model, Object.ICollisionObject, IBase
    {
        int score;

        public Coin()
        {
            score = 10;
        }

        public bool IsCatched { get; set; }
        public void Dispatch(ICollisionObject obj)
        {

        }
        public void Hit(Player player)
        {

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
