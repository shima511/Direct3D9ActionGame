using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object.Ground
{
    abstract class Base : ICollisionObject
    {
        bool _is_visible;
        /// <summary>
        /// 衝突判定の存在する線分
        /// </summary>
        public Collision.Shape.Line CollisionLine { get; set; }
        public bool IsVisible { get { return _is_visible; } set { _is_visible = value; } }

        public Base()
        {
            CollisionLine = new Collision.Shape.Line();
#if DEBUG
            _is_visible = true;
#else
            _is_visible = false;
#endif
        }
        abstract public void Dispatch(ICollisionObject obj);
        abstract public void Hit(Player player);
        public void Hit(Floor floor)
        {

        }
        public void Hit(Ceiling ceiling)
        {

        }
        public void Hit(RightWall wall)
        {

        }
        public void Hit(LeftWall wall)
        {

        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }
        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            CollisionLine.Draw3D(dev);
        }
    }

    class Floor : Base
    {
        public override void Dispatch(ICollisionObject obj)
        {
            obj.Hit(this);
        }
        public override void Hit(Player player)
        {
            if (player.FeetCollision.Hit(CollisionLine) && player.Speed.Y < 0)
            {
                var intercept = CollisionLine.StartingPoint.Y;
                var player_y = CollisionLine.Coefficient * (player.Position.X - CollisionLine.StartingPoint.X) + intercept + Player.LegLength;
                player.Position = new SlimDX.Vector3(player.Position.X, player_y, player.Position.Z);
                player.IsOnTheGround = true;
                player.IsInTheAir = false;
            }
        }
    }

    class Ceiling : Base
    {
        public override void Dispatch(ICollisionObject obj)
        {
            obj.Hit(this);
        }
        public override void Hit(Player player)
        {
            if (player.HeadCollision.Hit(CollisionLine))
            {
                var player_spd = player.Speed;
                player_spd.Y -= 0.01f;
                player.Speed = player_spd;
            }
        }
    }

    class RightWall : Base
    {
        public override void Dispatch(ICollisionObject obj)
        {
            obj.Hit(this);
        }
        public override void Hit(Player player)
        {

        }
    }
    class LeftWall : Base
    {
        public override void Dispatch(ICollisionObject obj)
        {
            obj.Hit(this);
        }
        public override void Hit(Player player)
        {

        }
    }

}
