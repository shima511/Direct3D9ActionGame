﻿using System;
using System.Collections.Generic;
using SlimDX;

namespace SlimDxGame.Object
{
    abstract class ICollisionObject : Object.Base.Model
    {
        abstract public void Dispatch(ICollisionObject obj);
        abstract public void Hit(Player player);
        abstract public void Hit(Floor floor);
        abstract public void Hit(Ceiling ceiling);
    }

    class Floor : ICollisionObject
    {
        public Floor()
        {
        }
        public Collision.Shape.Line Line { get; set; }
        public override void Dispatch(ICollisionObject obj)
        {
            obj.Hit(this);
        }
        public override void Hit(Player player)
        {
            if (player.FeetCollision.Hit(Line))
            {
                var intercept = Line.StartingPoint.Y;
                var player_y = Line.Coefficient * (player.Position.X - Line.StartingPoint.X) + intercept + Player.LegLength;
                player.Position = new Vector3(player.Position.X, player_y, player.Position.Z);
                player.IsOnTheGround = true;
                player.IsInTheAir = false;
            }
        }
        public override void Hit(Floor floor)
        {

        }
        public override void Hit(Ceiling ceiling)
        {

        }
        public override void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            Line.Draw3D(dev);
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
        public override void Dispatch(ICollisionObject obj)
        {
            obj.Hit(this);
        }
        public override void Hit(Player player)
        {

        }
        public override void Hit(Floor floor)
        {

        }
        public override void Hit(Ceiling ceiling)
        {

        }
    }
    
    
}
