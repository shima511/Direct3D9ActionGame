using System;
using System.Collections.Generic;
using SlimDX;

namespace SlimDxGame.Object
{
    class Player : Object.Base.Model, ICollisionObject, Component.IUpdateObject, Component.IOperableObject
    {
        public class Status
        {
            public int HP { get; set; }
            public int Score { get; set; }
        }

        /// <summary>
        /// 足の当たり判定
        /// </summary>
        public Collision.Shape.Line FeetCollision { get; set; }
        /// <summary>
        /// 頭の当たり判定
        /// </summary>
        public Collision.Shape.Line HeadCollision { get; set; }
        /// <summary>
        /// プレイヤー右側の当たり判定
        /// </summary>
        public Collision.Shape.Line RightSideCollision { get; set; }
        /// <summary>
        /// プレイヤー左側の当たり判定
        /// </summary>
        public Collision.Shape.Line LeftSideCollision { get; set; }
        public bool IsBesideOfRightWall { get; set; }
        public bool IsBesideOfLeftWall { get; set; }
        const float WalkSpeed = 0.05f;
        const float RunSpeed = 0.1f;
        const float JumpSpeed = 0.2f;
        const float FallSpeed = 0.01f;
        const float MaxFallSpeed = 0.1f;
        bool jumped_two_times = false;
        private ObjectState<Player> now_state = new Wait();
        private Vector2 _speed = new Vector2(0.0f, 0.0f);
        public float Width { get; private set; }
        public float Height { get; private set; }
        public Vector2 Speed { get { return _speed; } set { _speed = value; } }
        public bool IsInTheAir { get; set; }
        public bool FaceRight { get; set; }
        public bool IsTurning { get; set; }
        public Status State { get; set; }

        private class Wait : ObjectState<Player>
        {
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                parent._speed.X = 0.0f;
                parent.IsInTheAir = false;
                parent._rotation.Z = (float)Math.PI / 2;
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                // 左ボタンが押された場合
                if (controller.LeftButton.IsBeingPressed())
                {
                    // 右を向いている場合
                    if (parent.FaceRight)
                    {
                        parent.FaceRight = false;
                        parent.IsTurning = true;
                        new_state = new Turn();
                    }
                    else
                    {
                        new_state = new WalkStart();
                    }
                }
                // 右ボタンが押された場合
                if (controller.RightButton.IsBeingPressed())
                {
                    // 左を向いている場合
                    if (!parent.FaceRight)
                    {
                        parent.FaceRight = true;
                        parent.IsTurning = true;
                        new_state = new Turn();
                    }
                    else
                    {
                        new_state = new WalkStart();
                    }
                }
                if (controller.AButton.IsPressed())
                {
                    new_state = new JumpStart();
                }
            }
        }

        private class Turn : ObjectState<Player>
        {
            private int time = 0;
            private const int RequiredFrame = 10;
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                parent._rotation.Y += (float)Math.PI / RequiredFrame;
                if (time >= RequiredFrame)
                {
                    parent.IsTurning = false;
                    new_state = new WalkStart();
                }
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if (controller.RightButton.IsPressed() || controller.LeftButton.IsPressed())
                {
                    new_state = new Run();
                }
            }
        }

        private class QuickTurn : ObjectState<Player>
        {
            private int time = 0;
            private const int RequiredFrame = 15;
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                parent._rotation.Y += (float)Math.PI / RequiredFrame;
                if (time >= RequiredFrame)
                {
                    new_state = new Run();
                }
            }
        }

        private class WalkStart : ObjectState<Player>
        {
            private int time = 0;
            private const int RequiredFrame = 10;

            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                if (time >= RequiredFrame)
                {
                    new_state = new Walk();
                }
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if(controller.LeftButton.IsPressed() || controller.RightButton.IsPressed()){
                    new_state = new Run();
                }
                if(controller.AButton.IsPressed()){
                    new_state = new JumpStart();
                }
            }
        }

        private class Walk : ObjectState<Player>
        {
            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if (controller.RightButton.IsBeingPressed() && parent.FaceRight)
                {
                    parent._speed.X = WalkSpeed;
                    parent._rotation.Z = (float)Math.PI / 6;
                }
                else if (controller.LeftButton.IsBeingPressed() && !parent.FaceRight)
                {
                    parent._speed.X = -WalkSpeed;
                    parent._rotation.Z = (float)Math.PI / 6;
                }
                if (controller.RightButton.IsReleased() || controller.LeftButton.IsReleased())
                {
                    new_state = new Wait();
                }
                if(controller.AButton.IsPressed()){
                    new_state = new JumpStart();
                }
            }
        }

        private class WalkEnd : ObjectState<Player>
        {

        }

        private class Run : ObjectState<Player>
        {
            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if (controller.RightButton.IsBeingPressed() && parent.FaceRight)
                {
                    parent._speed.X = RunSpeed;
                    parent._rotation.Z = (float)Math.PI / 4;
                }
                else if (controller.LeftButton.IsBeingPressed() && !parent.FaceRight)
                {
                    parent._speed.X = -RunSpeed;
                    parent._rotation.Z = (float)Math.PI / 4;
                }
                if (controller.RightButton.IsReleased() || controller.LeftButton.IsReleased())
                {
                    new_state = new Break();
                }
                if (controller.AButton.IsPressed())
                {
                    new_state = new JumpStart();
                }
            }
        }

        private class Break : ObjectState<Player>
        {
            private int time = 0;
            private const int RequiredFrame = 5;
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                if (time >= RequiredFrame)
                {
                    new_state = new Wait();
                }
            }
        }

        private class JumpStart : ObjectState<Player>
        {
            private int time = 0;
            private const int RequiredFrame = 15;

            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                if (time <= RequiredFrame)
                {
                    // 初速度
                    parent._speed.Y = JumpSpeed;
                    parent.IsInTheAir = true;
                    new_state = new Jump();
                }
            }
        }

        private class Jump : ObjectState<Player>
        {
            private int time = 0;
            private const int RequiredFrame = 15;
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                if (time >= RequiredFrame)
                {
                    new_state = new Fall();
                }
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if (controller.AButton.IsPressed() && !parent.jumped_two_times)
                {
                    parent._speed.Y = JumpSpeed;
                    new_state = new TwiceJump();
                }
                if (controller.RightButton.IsBeingPressed() && !parent.IsBesideOfRightWall)
                {
                    parent._speed.X = WalkSpeed;
                }
                if (controller.LeftButton.IsBeingPressed() && !parent.IsBesideOfLeftWall)
                {
                    parent._speed.X = -WalkSpeed;
                }
            }
        }

        private class TwiceJump : ObjectState<Player>
        {
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                base.Update(parent, ref new_state);
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if (controller.RightButton.IsBeingPressed())
                {
                    parent._speed.X = WalkSpeed;
                }
                if (controller.LeftButton.IsBeingPressed())
                {
                    parent._speed.X = -WalkSpeed;
                }
            }
        }

        private class Fall : ObjectState<Player>
        {
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                if (!parent.IsInTheAir)
                {
                    new_state = new Land();
                }
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if(controller.AButton.IsPressed() && !parent.jumped_two_times)
                {
                    new_state = new TwiceJump();
                }
                if (controller.RightButton.IsBeingPressed())
                {
                    parent._speed.X = WalkSpeed;
                }
                if (controller.LeftButton.IsBeingPressed())
                {
                    parent._speed.X = -WalkSpeed;
                }
            }
        }

        private class Land : ObjectState<Player>
        {
            int time = 0;
            const int RequiredFrame = 5;
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                parent.Speed = new Vector2();
                if (time >= RequiredFrame)
                {
                    parent.jumped_two_times = false;
                    new_state = new Wait();
                }
            }
        }

        public void Dispatch(ICollisionObject obj)
        {
            obj.Hit(this);
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
        public Player()
        {
            IsBesideOfRightWall = false;
            IsTurning = false;
            IsInTheAir = true;
            IsBesideOfLeftWall = false;

            _rotation.Z = (float)Math.PI / 4;

            State = new Status()
            {
                HP = 10,
                Score = 0
            };

            Width = 1.0f;
            Height = 1.0f;
            FeetCollision = new Collision.Shape.Line();
            HeadCollision = new Collision.Shape.Line();
            RightSideCollision = new Collision.Shape.Line();
            LeftSideCollision = new Collision.Shape.Line();
        }

        private void UpdateSpeed()
        {
            if (this.IsInTheAir && _speed.Y >= -MaxFallSpeed)
            {
                _speed.Y -= FallSpeed;
            }

        }

        private void UpdatePosition()
        {
            if ((Speed.X < 0 && !IsBesideOfLeftWall) || (Speed.X > 0 && !IsBesideOfRightWall))
            {
                _position.X += _speed.X;
            }
            _position.Y += _speed.Y;
        }

        private void UpdateCollision()
        {
            // 頭の当たり判定を更新
            HeadCollision.StartingPoint = new Vector2(_position.X, _position.Y);
            HeadCollision.TerminalPoint = new Vector2(_position.X, _position.Y + Height / 2);

            // 足の当たり判定を更新
            FeetCollision.StartingPoint = new Vector2(_position.X, _position.Y);
            FeetCollision.TerminalPoint = new Vector2(_position.X, _position.Y - Height / 2);

            // 右側の当たり判定を更新
            RightSideCollision.StartingPoint = new Vector2(_position.X, _position.Y);
            RightSideCollision.TerminalPoint = new Vector2(_position.X + Width / 2, _position.Y);

            // 左側の当たり判定を更新
            LeftSideCollision.StartingPoint = new Vector2(_position.X, _position.Y);
            LeftSideCollision.TerminalPoint = new Vector2(_position.X - Width / 2, _position.Y);
        }

        public void Update()
        {
            now_state.Update(this, ref now_state);
            UpdateSpeed();
            UpdatePosition();
            UpdateCollision();
        }

        public override void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            FeetCollision.Draw3D(dev);
            HeadCollision.Draw3D(dev);
            RightSideCollision.Draw3D(dev);
            LeftSideCollision.Draw3D(dev);
            base.Draw3D(dev);
        }

        private void ControlSpeed(SlimDxGame.Controller controller)
        {

        }

        public void ControllerAction(SlimDxGame.Controller controller)
        {
            now_state.ControllerAction(this, controller, ref now_state);
            if (controller.SelectButton.IsPressed())
            {
                _position = new Vector3();
            }
        }
    }
}
