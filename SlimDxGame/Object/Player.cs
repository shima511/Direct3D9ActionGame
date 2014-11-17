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
            Run = 1,
            Jump = 2,
            StickToRightWall = 4,
            InAir = 8,
        }

        public StateFrag State { get; set; }

        /// <summary>
        /// 終着点についたらtrueを返します。
        /// </summary>
        public bool ReachedRightBorder { get; set; }

        public bool IsActive { get; set; }
        /// <summary>
        /// ジャンプした瞬間に実行されるイベント
        /// </summary>
        public event Action<SlimDX.Vector3> OnJump;
        /// <summary>
        /// 2回目のジャンプで実行されるイベント
        /// </summary>
        public event Action<SlimDX.Vector3> OnSecondJump;
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
        public Collision.Shape.Line RightTopSideCollision { get; set; }
        /// <summary>
        /// プレイヤー左側の当たり判定
        /// </summary>
        public Collision.Shape.Line RightBottomSideCollision { get; set; }
        int fall_time = 0;
        readonly float MinimumRunSpeed = 0.1f;
        readonly float MaxRunSpeed = 1.5f;
        readonly float JumpSpeed = 0.4f;
        readonly float FallSpeed = 0.02f;
        readonly float MaxFallSpeed = 0.1f;
        readonly int DefaultLives = 1;
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
        /// 移動可能かどうかを設定・取得します
        /// </summary>
        public bool CanMove { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Status.Charactor Parameter { get; set; }

        /// <summary>
        /// プレイヤーの残機
        /// </summary>
        public int Life { get; set; }

        public MikuMikuDance.Core.Model.MMDModel MMDModel { get; set; }

        private class Run : ObjectState<Player>
        {
            int time = 0;
            readonly int RequiredTime = 80;
            readonly float SpeedDiff = 0.02f;

            public Run(Player parent)
            {
                parent.jumped_two_times = false;
                parent.State |= StateFrag.Run;
            }

            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                if (time == 0)
                {
                    parent.MMDModel.AnimationPlayer["Run"].Reset();
                    parent.MMDModel.AnimationPlayer["Run"].Start();
                }
                time++;
                time %= RequiredTime;
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if (controller.AButton.IsPressed())
                {
                    parent.State -= StateFrag.Run;
                    new_state = new Jump(parent);
                }
                if (controller.RightButton.IsPressed() && parent.Speed.X < parent.MaxRunSpeed)
                {
                    var spd = parent.Speed;
                    spd.X += SpeedDiff;
                    parent.Speed = spd;
                }
                if (controller.LeftButton.IsPressed() && parent.Speed.X > parent.MinimumRunSpeed)
                {
                    var spd = parent.Speed;
                    spd.X -= SpeedDiff;
                    parent.Speed = spd;
                }
            }
        }

        private class Jump : ObjectState<Player>
        {
            private int time = 0;
            readonly int RequiredFrame = 15;

            public Jump(Player parent)
            {
                var spd = parent.Speed;
                spd.Y = parent.JumpSpeed;
                parent.Speed = spd;
                parent.State |= StateFrag.Jump;
                parent.OnJump(parent.Position);
            }

            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                if (time >= RequiredFrame)
                {
                    parent.State -= StateFrag.Jump;
                    new_state = new Fall();
                }
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if (controller.DownButton.IsPressed())
                {
                    var spd = parent.Speed;
                    spd.Y = -parent.MaxFallSpeed;
                    parent.Speed = spd;
                }
                if (controller.AButton.IsPressed() && !parent.jumped_two_times)
                {
                    new_state = new TwiceJump(parent);
                }
            }
        }

        private class TwiceJump : ObjectState<Player>
        {
            int time = 0;
            readonly int RequiredFrame = 5;
            public TwiceJump(Player parent)
            {
                var spd = parent.Speed;
                spd.Y = parent.JumpSpeed;
                parent.Speed = spd;
                parent.jumped_two_times = true;
                parent.OnSecondJump(parent.Position);
            }

            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                if (time >= RequiredFrame)
                {
                    parent.State -= StateFrag.Jump;
                    new_state = new Fall();
                }
            }
        }

        private class Fall : ObjectState<Player>
        {
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                if ((parent.State & StateFrag.InAir) != StateFrag.InAir)
                {
                    new_state = new Run(parent);
                }
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if(controller.AButton.IsPressed() && !parent.jumped_two_times)
                {
                    new_state = new TwiceJump(parent);
                }
            }
        }

        public void Pause()
        {
            MMDModel.AnimationPlayer["Run"].Stop();
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
        public Player()
        {
            Life = DefaultLives;

            Parameter = new Status.Charactor()
            {
                HP = 1
            };
            Width = 1.0f;
            Height = 1.0f;
            FeetCollision = new Collision.Shape.Line();
            HeadCollision = new Collision.Shape.Line();
            RightTopSideCollision = new Collision.Shape.Line();
            RightBottomSideCollision = new Collision.Shape.Line();

            CurrentState = new Run(this);
        }

        private void UpdateSpeed()
        {
            if ((State & StateFrag.InAir) == StateFrag.InAir && _speed.Y >= -MaxFallSpeed)
            {
                _speed.Y -= FallSpeed;
            }
            if ((State & StateFrag.InAir) == StateFrag.InAir && _speed.X == 0.0f)
            {
                _speed.X = MinimumRunSpeed;
            }
            if ((State & StateFrag.StickToRightWall) == StateFrag.StickToRightWall)
            {
                _speed.X = 0.0f;
            }
        }

        private void UpdatePosition()
        {
            if (CanMove)
            {
                _position.X += _speed.X;
                _position.Y += _speed.Y;
            }
        }

        private void UpdateCollision()
        {
            // 頭の当たり判定を更新
            HeadCollision.StartingPoint = new Vector2(_position.X, _position.Y);
            HeadCollision.TerminalPoint = new Vector2(_position.X, _position.Y + 2.0f + Height / 2);

            // 足の当たり判定を更新
            FeetCollision.StartingPoint = new Vector2(_position.X, _position.Y + Height / 2);
            FeetCollision.TerminalPoint = new Vector2(_position.X, _position.Y - Height / 2);

            // 右側上の当たり判定を更新
            RightTopSideCollision.StartingPoint = new Vector2(_position.X - Width, _position.Y + 2 * Height);
            RightTopSideCollision.TerminalPoint = new Vector2(_position.X + Width, _position.Y + 2 * Height);

            // 右側下の当たり判定を更新
            RightBottomSideCollision.StartingPoint = new Vector2(_position.X - Width, _position.Y);
            RightBottomSideCollision.TerminalPoint = new Vector2(_position.X + Width, _position.Y);
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

        void MendMatrix()
        {
            var mat = CommonMatrix.WorldMatrix;
            mat.M42 -= 0.4f;
            CommonMatrix.WorldMatrix = mat;
        }

        public void Update()
        {
            UpdateState();
            UpdateSpeed();
            UpdatePosition();
            UpdateCollision();
            UpdateFallTime();
            UpdateMatrix();
            MendMatrix();
            MMDModel.Transform = CommonMatrix.WorldMatrix;
        }

        public void DrawHitRange(SlimDX.Direct3D9.Device dev)
        {
            FeetCollision.Draw3D(dev);
            HeadCollision.Draw3D(dev);
            RightTopSideCollision.Draw3D(dev);
            RightBottomSideCollision.Draw3D(dev);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        void ActionForDebug(SlimDxGame.Controller controller)
        {
            if (controller.DButton.IsPressed() && controller.UpButton.IsBeingPressed())
            {
                ReachedRightBorder = true;
            }
        }

        public void ControllerAction(SlimDxGame.Controller controller)
        {
            CurrentState.ControllerAction(this, controller, ref CurrentState);
            ActionForDebug(controller);
        }

        public override void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            base.Draw3D(dev);
            MMDModel.Draw();
        }

        public void ResetState()
        {
            ReachedRightBorder = false;
            fall_time = 0;
            Parameter.HP = 1;
            CurrentState = new Run(this);
            Scale = new Vector3(0.2f, 0.2f, 0.2f);
            Speed = new Vector2(MinimumRunSpeed, 0.0f);
            _position.Z = 20.0f;
            _rotation.Y = (float)Math.PI / 2;
        }
    }
}
