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
        public SlimDX.Vector3 Position { get; set; }

        [System.Diagnostics.Conditional("DEBUG")]
        void DebugDrawMode()
        {
            IsVisible = true;
        }

        public Base()
        {
            CollisionLine = new Collision.Shape.Line();
            DebugDrawMode();
        }

        public void Update()
        {

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
                case 2:
                    new_collision = new LeftWall();
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
                player.IsInTheAir = false;
            }
        }
    }

    class Ceiling : Base
    {
        const float HitEffect = 0.05f;
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
            if (player.RightSideCollision.Hit(CollisionLine))
            {
                var player_vector = player.Speed;
                float common_denomi = (float)Math.Sqrt(Math.Pow(CollisionLine.TerminalPoint.X - CollisionLine.StartingPoint.X, 2.0) + Math.Pow(CollisionLine.TerminalPoint.Y - CollisionLine.StartingPoint.Y, 2.0));
                var n_normal = new SlimDX.Vector2((CollisionLine.StartingPoint.Y - CollisionLine.TerminalPoint.Y) / common_denomi, (CollisionLine.TerminalPoint.X - CollisionLine.StartingPoint.X) / common_denomi);
                var length = -SlimDX.Vector2.Dot(player_vector, n_normal);
                var w_vec = player_vector + length * n_normal;

                // プレイヤーのスピードを調整
                var player_spd = player.Speed;
                player_spd.X = w_vec.X;
                player_spd.Y = w_vec.Y;
                player.Speed = player_spd;
                player.IsBesideOfRightWall = true;
            }
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
            if (player.LeftSideCollision.Hit(CollisionLine))
            {
                var player_vector = player.Speed;
                float common_denomi = (float)Math.Sqrt(Math.Pow(CollisionLine.TerminalPoint.X - CollisionLine.StartingPoint.X, 2.0) + Math.Pow(CollisionLine.TerminalPoint.Y - CollisionLine.StartingPoint.Y, 2.0));
                var n_normal = new SlimDX.Vector2((CollisionLine.StartingPoint.Y - CollisionLine.TerminalPoint.Y) / common_denomi, (CollisionLine.TerminalPoint.X - CollisionLine.StartingPoint.X) / common_denomi);
                var length = -SlimDX.Vector2.Dot(player_vector, n_normal);
                var w_vec = player_vector + length * n_normal;

                // プレイヤーのスピードを調整
                var player_spd = player.Speed;
                player_spd.X = w_vec.X;
                player_spd.Y = w_vec.Y;
                player.Speed = player_spd;
                player.IsBesideOfLeftWall = true;
            }
        }
    }

}
