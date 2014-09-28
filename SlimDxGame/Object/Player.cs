using System;
using System.Collections.Generic;
using SlimDX;

namespace SlimDxGame.Object
{
    class Player : Object.Base.Model, ICollisionObject, Component.IUpdateObject, Component.IOperableObject
    {
        [Flags]
        public enum StateFrag
        {
            None = 0,
            Wait = 1,
            Walk = 2,
            Run = 4,
            Jump = 8,
            StickToRightWall = 16,
            StickToLeftWall = 32,
            InAir = 64,
            FaceRight = 128,
            Turn = 256
        }

        public StateFrag State { get; set; }

        public bool IsActive { get; set; }
        /// <summary>
        /// ジャンプした瞬間に実行されるメソッドです。
        /// </summary>
        public event Action<SlimDX.Vector3> OnJump;
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
        int fall_time = 0;
        readonly float MoveSpeed = 0.01f;
        readonly float WalkSpeed = 0.05f;
        readonly float RunSpeed = 0.1f;
        readonly float JumpSpeed = 0.2f;
        readonly float FallSpeed = 0.01f;
        readonly float MaxFallSpeed = 0.1f;
        bool jumped_two_times = false;
        /// <summary>
        /// 現在の状態
        /// </summary>
        private ObjectState<Player> CurrentState;
        private Vector2 _speed = new Vector2(0.0f, 0.0f);
        public float Width { get; private set; }
        public float Height { get; private set; }
        public Vector2 Speed { get { return _speed; } set { _speed = value; } }
        /// <summary>
        /// 
        /// </summary>
        public Status.Charactor Parameter { get; set; }

        /// <summary>
        /// プレイヤーの残機
        /// </summary>
        public int Life { get; set; }

        private class Wait : ObjectState<Player>
        {
            public Wait(Player parent)
            {
                parent.State |= StateFrag.Wait;
            }

            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                parent._speed.X = 0.0f;
                parent._rotation.Z = (float)Math.PI / 2;
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                // 左ボタンが押された場合
                if (controller.LeftButton.IsBeingPressed())
                {
                    parent.State -= StateFrag.Wait;
                    // 右を向いている場合
                    if ((parent.State & StateFrag.FaceRight) == StateFrag.FaceRight)
                    {
                        parent.State -= StateFrag.FaceRight;
                        new_state = new Turn(parent);
                    }
                    else
                    {
                        parent.State |= StateFrag.Turn;
                        new_state = new WalkStart();
                    }
                }
                // 右ボタンが押された場合
                if (controller.RightButton.IsBeingPressed())
                {
                    parent.State -= StateFrag.Wait;
                    // 左を向いている場合
                    if ((parent.State & StateFrag.FaceRight) != StateFrag.FaceRight)
                    {
                        parent.State |= StateFrag.FaceRight;
                        new_state = new Turn(parent);
                    }
                    else
                    {
                        parent.State |= StateFrag.Walk;
                        new_state = new WalkStart();
                    }
                }
                if (controller.AButton.IsPressed())
                {
                    parent.State -= StateFrag.Wait;
                    new_state = new JumpStart();
                }
                if(controller.DownButton.IsBeingPressed())
                {
                    parent.State -= StateFrag.Wait;
                    new_state = new CrouchStart();
                }
            }
        }

        private class Turn : ObjectState<Player>
        {
            private int time = 0;
            readonly int RequiredFrame = 10;

            public Turn(Player parent)
            {
                parent.State |= StateFrag.Turn;
            }

            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                parent._rotation.Y += (float)Math.PI / RequiredFrame;
                if (time >= RequiredFrame)
                {
                    parent.State -= StateFrag.Turn;
                    new_state = new WalkStart();
                }
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if (controller.RightButton.IsPressed() || controller.LeftButton.IsPressed())
                {
                    parent.State -= StateFrag.Turn;
                    new_state = new Run(parent);
                }
            }
        }

        private class QuickTurn : ObjectState<Player>
        {
            private int time = 0;
            readonly int RequiredFrame = 15;
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                parent._rotation.Y += (float)Math.PI / RequiredFrame;
                if (time >= RequiredFrame)
                {
                    new_state = new Run(parent);
                }
            }
        }

        private class WalkStart : ObjectState<Player>
        {
            private int time = 0;
            readonly int RequiredFrame = 10;

            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                if (time >= RequiredFrame)
                {
                    new_state = new Walk(parent);
                }
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if(controller.LeftButton.IsPressed() || controller.RightButton.IsPressed()){
                    new_state = new Run(parent);
                }
                if(controller.AButton.IsPressed()){
                    new_state = new JumpStart();
                }
            }
        }

        private class Walk : ObjectState<Player>
        {
            public Walk(Player parent)
            {
                parent.State |= StateFrag.Walk;
            }

            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if (controller.RightButton.IsBeingPressed() && (parent.State & StateFrag.FaceRight) == StateFrag.FaceRight)
                {
                    parent._speed.X = parent.WalkSpeed;
                    parent._rotation.Z = (float)Math.PI / 6;
                }
                else if (controller.LeftButton.IsBeingPressed() && (parent.State & StateFrag.FaceRight) != StateFrag.FaceRight)
                {
                    parent._speed.X = -parent.WalkSpeed;
                    parent._rotation.Z = (float)Math.PI / 6;
                }
                if (controller.RightButton.IsReleased() || controller.LeftButton.IsReleased())
                {
                    parent.State -= StateFrag.Walk;
                    new_state = new Wait(parent);
                }
                if(controller.AButton.IsPressed()){
                    parent.State -= StateFrag.Walk;
                    new_state = new JumpStart();
                }
            }
        }

        private class WalkEnd : ObjectState<Player>
        {

        }

        private class Run : ObjectState<Player>
        {
            public Run(Player parent)
            {
                parent.State |= StateFrag.Run;
            }

            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if (controller.RightButton.IsBeingPressed() && (parent.State & StateFrag.FaceRight) == StateFrag.FaceRight)
                {
                    parent._speed.X = parent.RunSpeed;
                    parent._rotation.Z = (float)Math.PI / 4;
                }
                else if (controller.LeftButton.IsBeingPressed() && (parent.State & StateFrag.FaceRight) != StateFrag.FaceRight)
                {
                    parent._speed.X = -parent.RunSpeed;
                    parent._rotation.Z = (float)Math.PI / 4;
                }
                if (controller.RightButton.IsReleased() || controller.LeftButton.IsReleased())
                {
                    parent.State -= StateFrag.Run;
                    new_state = new Break();
                }
                if (controller.AButton.IsPressed())
                {
                    parent.State -= StateFrag.Run;
                    new_state = new JumpStart();
                }
            }
        }

        private class Break : ObjectState<Player>
        {
            private int time = 0;
            readonly int RequiredFrame = 5;
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                if (time >= RequiredFrame)
                {
                    new_state = new Wait(parent);
                }
            }
        }

        private class JumpStart : ObjectState<Player>
        {
            private int time = 0;
            readonly int RequiredFrame = 15;

            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                if (time <= RequiredFrame)
                {
                    // 初速度
                    parent._speed.Y = parent.JumpSpeed;
                    parent.State |= StateFrag.InAir;
                    if (parent.OnJump != null)
                    {
                        parent.OnJump(parent.Position);
                    }
                    new_state = new Jump();
                }
            }
        }

        private class Jump : ObjectState<Player>
        {
            private int time = 0;
            readonly int RequiredFrame = 15;
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
                    new_state = new TwiceJump(parent);
                }
                if (controller.RightButton.IsBeingPressed() && parent.Speed.X < parent.RunSpeed)
                {
                    parent._speed.X += parent.MoveSpeed;
                }
                if (controller.LeftButton.IsBeingPressed() && parent.Speed.X > -parent.RunSpeed)
                {
                    parent._speed.X -= parent.MoveSpeed;
                }
            }
        }

        private class TwiceJump : ObjectState<Player>
        {
            int time = 0;
            readonly int RequiredFrame = 5;
            public TwiceJump(Player parent)
            {
                parent._speed.Y = parent.JumpSpeed;
                parent.jumped_two_times = true;
            }

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
                if (controller.RightButton.IsBeingPressed() && parent.Speed.X < parent.RunSpeed)
                {
                    parent._speed.X += parent.MoveSpeed;
                }
                if (controller.LeftButton.IsBeingPressed() && parent.Speed.X > -parent.RunSpeed)
                {
                    parent._speed.X -= parent.MoveSpeed;
                }
            }
        }

        private class Fall : ObjectState<Player>
        {
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                if ((parent.State & StateFrag.InAir) != StateFrag.InAir)
                {
                    new_state = new Land();
                }
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if(controller.AButton.IsPressed() && !parent.jumped_two_times)
                {
                    new_state = new TwiceJump(parent);
                }
                if (controller.RightButton.IsBeingPressed() && parent.Speed.X < parent.RunSpeed)
                {
                    parent._speed.X += parent.MoveSpeed;
                }
                if (controller.LeftButton.IsBeingPressed() && parent.Speed.X > -parent.RunSpeed)
                {
                    parent._speed.X -= parent.MoveSpeed;
                }
            }
        }

        private class Land : ObjectState<Player>
        {
            int time = 0;
            readonly int RequiredFrame = 5;
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                parent.Speed = new Vector2();
                if (time >= RequiredFrame)
                {
                    parent.jumped_two_times = false;
                    new_state = new Wait(parent);
                }
            }
        }

        private class CrouchStart : ObjectState<Player>
        {
            int time = 0;
            readonly int RequiredFrame = 5;
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                var rot = parent.Rotation;
                rot.Z += (float)(Math.PI) / (RequiredFrame * 2);
                parent.Rotation = rot;
                if (time >= RequiredFrame)
                {
                    new_state = new Crouching();
                }
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if (controller.DownButton.IsReleased())
                {
                    new_state = new CrouchEnd();
                }
            }
        }

        private class Crouching : ObjectState<Player>
        {
            int time = 0;
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                var rot = parent.Rotation;
                rot.Y = (float)(Math.PI * Math.Sin(time * 0.01) / 3);
                parent.Rotation = rot;
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if (controller.DownButton.IsReleased())
                {
                    new_state = new CrouchEnd();
                }
            }
        }

        private class CrouchEnd : ObjectState<Player>
        {
            int time = 0;
            readonly int RequiredFrame = 5;
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                var rot = parent.Rotation;
                rot.Z -= (float)(Math.PI) / (RequiredFrame * 2);
                parent.Rotation = rot;
                if (time >= RequiredFrame)
                {
                    new_state = new Wait(parent);
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
            Life = 3;

            _rotation.Z = (float)Math.PI / 4;

            Parameter = new Status.Charactor()
            {
                HP = 10
            };

            Width = 1.0f;
            Height = 1.0f;
            FeetCollision = new Collision.Shape.Line();
            HeadCollision = new Collision.Shape.Line();
            RightSideCollision = new Collision.Shape.Line();
            LeftSideCollision = new Collision.Shape.Line();

            CurrentState = new Wait(this);
        }

        private void UpdateSpeed()
        {
            if ((State & StateFrag.InAir) == StateFrag.InAir && _speed.Y >= -MaxFallSpeed)
            {
                _speed.Y -= FallSpeed;
            }

        }

        private void UpdatePosition()
        {
            if ((Speed.X < 0 && (State & StateFrag.StickToLeftWall) != StateFrag.StickToLeftWall) || (Speed.X > 0 && (State & StateFrag.StickToRightWall) != StateFrag.StickToRightWall))
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

        void UpdateFallTime()
        {
            if((State & StateFrag.InAir) == StateFrag.InAir){
                fall_time++;
            }
            else
            {
                fall_time = 0;
            }
        }

        void UpdateState()
        {
            if (fall_time == 10)
            {
                CurrentState = new Fall();
            }
            CurrentState.Update(this, ref CurrentState);
        }

        public void Update()
        {
            UpdateState();
            UpdateSpeed();
            UpdatePosition();
            UpdateCollision();
            UpdateFallTime();
        }

        public void DrawHitRange(SlimDX.Direct3D9.Device dev)
        {
            FeetCollision.Draw3D(dev);
            HeadCollision.Draw3D(dev);
            RightSideCollision.Draw3D(dev);
            LeftSideCollision.Draw3D(dev);
        }

        private void ControlSpeed(SlimDxGame.Controller controller)
        {

        }

        [System.Diagnostics.Conditional("DEBUG")]
        void ActionForDebug(SlimDxGame.Controller controller)
        {
            if (controller.DButton.IsPressed() && controller.UpButton.IsBeingPressed())
            {
                _position = new Vector3();
            }
        }

        public void ControllerAction(SlimDxGame.Controller controller)
        {
            CurrentState.ControllerAction(this, controller, ref CurrentState);
            ActionForDebug(controller);
        }

        public void ResetState()
        {
            State = StateFrag.None;
            Speed = new Vector2(0.0f, 0.0f);
            Rotation = new Vector3(0.0f, 0.0f, 0.0f);
            Scale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}
