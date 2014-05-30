using System;
using System.Collections.Generic;
using SlimDX;

namespace SlimDxGame.Object
{
    interface ICollisionObject
    {
        void Dispatch(ICollisionObject obj);
        void Hit(Player player);
        void Hit(Floor floor);
        void Hit(Ceiling ceiling);
    }

    class Floor : ICollisionObject
    {
        public Collision.Shape.Line Line { get; set; }
        public void Dispatch(ICollisionObject obj)
        {
            obj.Hit(this);
        }
        public void Hit(Player player)
        {
            foreach (var col in player.LegCollisions)
            {
                if (col.Hit(Line) && player.Speed.Y <= 0)
                {
                    var inclination = (Line.TerminalPoint.Y - Line.StartingPoint.Y) / (Line.TerminalPoint.X - Line.StartingPoint.X);
                    var intercept = Line.StartingPoint.Y;
                    var player_y = inclination * (player.Position.X - Line.StartingPoint.X) + intercept + Player.LegLength;
                    player.Position = new Vector3(player.Position.X, player_y, player.Position.Z);
                    player.IsOnTheGround = true;
                    player.IsInTheAir = false;
                }
            }
        }
        public void Hit(Floor floor)
        {

        }
        public void Hit(Ceiling ceiling)
        {

        }
    }

    class Ceiling : ICollisionObject
    {
        /// <summary>
        /// X,Y係数
        /// </summary>
        public Vector2 Coefficient { get; set; }
        /// <summary>
        /// 定数項
        /// </summary>
        public float Nape { get; set; }
        public void Dispatch(ICollisionObject obj)
        {
            obj.Hit(this);
        }
        public void Hit(Player player)
        {

        }
        public void Hit(Floor floor)
        {

        }
        public void Hit(Ceiling ceiling)
        {

        }
    }
    
    
}
