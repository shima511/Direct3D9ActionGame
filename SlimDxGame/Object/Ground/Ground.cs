using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object.Ground
{
    abstract class Base : IFieldObject
    {
        /// <summary>
        /// 衝突判定の存在する線分
        /// </summary>
        public Collision.Shape.Line CollisionLine { get; set; }
        public bool IsVisible { get; set; }
        public bool IsActive { get; set; }
        public bool Spawnable { get; set; }
        public SlimDX.Vector3 Position { get; set; }

        [System.Diagnostics.Conditional("DEBUG")]
        void DebugDrawMode()
        {
            IsVisible = true;
        }

        public Base()
        {
            Spawnable = true;
            CollisionLine = new Collision.Shape.Line();
            DebugDrawMode();
        }

        public void Update()
        {

        }

        abstract public void Dispatch(ICollisionObject obj);

        abstract public void Hit(Player player);

        public void DrawHitRange(SlimDX.Direct3D9.Device dev)
        {
            CollisionLine.Draw3D(dev);
        }

        public void Hit(Floor floor)
        {

        }
        public void Hit(Ceiling ceiling)
        {

        }
        public void Hit(RightWall wall)
        {

        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }
        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            CollisionLine.Draw3D(dev);
        }

        public static Base CreateGround(StageRW.Property.Collision col)
        {
            Base new_collision = null;
            switch (col.TypeId)
            {
                case 0:
                    new_collision = new Floor();
                    break;
                case 1:
                    new_collision = new RightWall();
                    break;
                case 3:
                    new_collision = new Ceiling();
                    break;
            }
            new_collision.CollisionLine.StartingPoint = col.StartingPoint;
            new_collision.CollisionLine.TerminalPoint = col.TerminatePoint;
            return new_collision;
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
                var player_y = CollisionLine.Coefficient * (player.Position.X - CollisionLine.StartingPoint.X) + CollisionLine.Intercept + player.Height / 2;
                var p_pos = player.Position;
                p_pos.Y = player_y;
                player.Position = p_pos;
                if ((player.State & Player.StateFrag.InAir) == Player.StateFrag.InAir)
                {
                    player.State -= Object.Player.StateFrag.InAir;
                }
            }
        }
    }

    class Ceiling : Base
    {
        readonly float HitEffect = 0.1f;
        public override void Dispatch(ICollisionObject obj)
        {
            obj.Hit(this);
        }
        public override void Hit(Player player)
        {
            if (player.HeadCollision.Hit(CollisionLine))
            {
                var player_spd = player.Speed;
                player_spd.Y -= HitEffect;
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
            if (player.RightTopSideCollision.Hit(CollisionLine) || player.RightBottomSideCollision.Hit(CollisionLine))
            {
                player.State |= Player.StateFrag.StickToRightWall;
                var pos = player.Position;
                pos.X = CollisionLine.StartingPoint.X - 1.0f;
                player.Position = pos;
            }
        }
    }
}
