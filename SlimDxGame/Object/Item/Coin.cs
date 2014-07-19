using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object.Item
{
    class Coin : Object.Base.Model, IBase
    {
        public SlimDX.Vector2 Position2D
        {
            get
            {
                return new SlimDX.Vector2(Position.X, Position.Y);
            }
            set
            {
                var pos = Position;
                pos.X = Position2D.X;
                pos.Y = Position2D.Y;
                Position = pos;
            }
        }
        public event OnHitAction OnHit;
        Collision.Shape.Point HitCollision;
        readonly int score = 10;

        public Coin()
        {
            HitCollision = new Collision.Shape.Point();
        }

        public void Update()
        {
            var rot = Rotation;
            rot.Y += (float)(Math.PI * 0.01);
            Rotation = rot;
        }

        public bool IsCatched { get; set; }
        public void Dispatch(ICollisionObject obj)
        {

        }
        public void Hit(Player player)
        {
            if (player.RightSideCollision.Hit(HitCollision))
            {
                OnHit(this);
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
