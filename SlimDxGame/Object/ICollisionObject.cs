using System;
using System.Collections.Generic;
using SlimDX;

namespace SlimDxGame.Object
{
    interface ICollisionObject : Component.IDrawableObject
    {
        void Dispatch(ICollisionObject obj);
        void Hit(Player player);
        void Hit(Floor floor);
        void Hit(Ceiling ceiling);
        void Hit(RightWall wall);
        void Hit(LeftWall wall);
    }

    class Floor : ICollisionObject
    {
        bool _is_visible = true;
        public bool IsVisible { get { return _is_visible; } set { _is_visible = value; } }
        public Floor()
        {
        }
        public Collision.Shape.Line Line { get; set; }
        public void Dispatch(ICollisionObject obj)
        {
            obj.Hit(this);
        }
        public void Hit(Player player)
        {
            if (player.FeetCollision.Hit(Line) && player.Speed.Y < 0)
            {
                var intercept = Line.StartingPoint.Y;
                var player_y = Line.Coefficient * (player.Position.X - Line.StartingPoint.X) + intercept + Player.LegLength;
                player.Position = new Vector3(player.Position.X, player_y, player.Position.Z);
                player.IsOnTheGround = true;
                player.IsInTheAir = false;
            }
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
        public void Hit(LeftWall wall)
        {

        }
        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }
        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            Line.Draw3D(dev);
        }
    }

    class Ceiling : ICollisionObject
    {
        bool _is_visible = true;
        public bool IsVisible { get { return _is_visible; } set { _is_visible = value; } }
        public Collision.Shape.Line Line { get; set; }
        public void Dispatch(ICollisionObject obj)
        {
            obj.Hit(this);
        }
        public void Hit(Player player)
        {
            if (player.HeadCollision.Hit(Line))
            {
                var player_spd = player.Speed;
                player_spd.Y -= 0.01f;
                player.Speed = player_spd;
            }
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
        public void Hit(LeftWall wall)
        {

        }
        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }
        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            Line.Draw3D(dev);
        }
    }

    class RightWall : ICollisionObject
    {
        bool _is_visible = true;
        public bool IsVisible { get { return _is_visible; } set { _is_visible = value; } }

        public Collision.Shape.Line Line { get; set; }
        public void Dispatch(ICollisionObject obj)
        {
            obj.Hit(this);
        }
        public void Hit(Player player)
        {
            if (player.HeadCollision.Hit(Line))
            {
                var player_spd = player.Speed;
                player_spd.Y -= 0.01f;
                player.Speed = player_spd;
            }
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
        public void Hit(LeftWall wall)
        {

        }
        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }
        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            Line.Draw3D(dev);
        }
    }
    class LeftWall : ICollisionObject
    {
        bool _is_visible = true;
        public bool IsVisible { get { return _is_visible; } set { _is_visible = value; } }

        public Collision.Shape.Line Line { get; set; }
        public void Dispatch(ICollisionObject obj)
        {
            obj.Hit(this);
        }
        public void Hit(Player player)
        {
            if (player.HeadCollision.Hit(Line))
            {
                var player_spd = player.Speed;
                player_spd.Y -= 0.01f;
                player.Speed = player_spd;
            }
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
        public void Hit(LeftWall wall)
        {

        }
        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }
        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            Line.Draw3D(dev);
        }
    }
}
