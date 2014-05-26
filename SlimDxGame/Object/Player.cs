﻿using System;
using System.Collections.Generic;
using SlimDX;

namespace SlimDxGame.Object
{
    class Player : Base.Model, Component.IUpdateObject, Component.IOperableObject, ICollisionObject
    {
        private List<Collision.Shape.Circle> _collision_shapes = new List<Collision.Shape.Circle>();
        public List<Collision.Shape.Circle> CollisionShapes { get { return _collision_shapes; } set { _collision_shapes = value; } }
        private const float WalkSpeed = 0.01f;
        private const float RunSpeed = 0.03f;
        private const float JumpSpeed = 0.2f;
        private ObjectState<Player> now_state = new Wait();
        private Vector2 _speed = new Vector2(0.0f, 0.0f);
        private bool _face_right = true;
        private bool _is_turning = false;
        private bool _in_the_air = false;
        private bool _is_on_the_ground = true;
        public bool IsInTheAir { get { return _in_the_air; } set { _in_the_air = value; } }
        public bool FaceRight { get { return _face_right; } set { _face_right = value; } }
        public bool FaceLeft { get { return !_face_right; } set { _face_right = !value; } }
        public bool IsTurning { get { return _is_turning; } set { _is_turning = value; } }
        public bool IsOnTheGround { get { return _is_on_the_ground; } set { _is_on_the_ground = value; } }

        private class Wait : ObjectState<Player>
        {
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                parent._speed = new Vector2(0.0f, 0.0f);
                parent._rotation.Z = 0.0f;
            }

            public override void ControllerAction(Player parent, Controller controller, ref ObjectState<Player> new_state)
            {
                if (controller.LeftButton.IsPressed() || controller.RightButton.IsPressed())
                {
                    new_state = new WalkStart();
                }
                // 右を向いている状態で左ボタンが押された場合
                if (parent.FaceRight && controller.LeftButton.IsPressed())
                {
                    parent.FaceRight = false;
                    parent._is_turning = true;
                    new_state = new Turn();
                }
                // 左を向いている状態で右ボタンが押された場合
                if (!parent.FaceRight && controller.RightButton.IsPressed())
                {
                    parent.FaceRight = true;
                    parent._is_turning = true;
                    new_state = new Turn();
                }
                if (controller.LeftButton.IsBeingPressed() || controller.RightButton.IsBeingPressed())
                {
                    new_state = new Walk();
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
            private const int RequiredFrame = 30;
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                parent._rotation.Y += (float)Math.PI / RequiredFrame;
                if (time >= RequiredFrame)
                {
                    parent._is_turning = false;
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
            }
        }

        private class Break : ObjectState<Player>
        {
            private int time = 0;
            private const int RequiredFrame = 15;
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
                    parent._in_the_air = true;
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
            private int time = 0;
            private const int RequiredFrame = 15;
            public override void Update(Player parent, ref ObjectState<Player> new_state)
            {
                time++;
                if (time >= RequiredFrame)
                {
                    parent._in_the_air = false;
                    new_state = new Wait();
                }
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

        private void UpdateSpeed()
        {
            if (this.IsInTheAir)
            {
                _speed.Y -= 0.01f;
            }
        }

        private void UpdatePosition()
        {
            _position.X += _speed.X;
            _position.Y += _speed.Y;
            foreach (var item in CollisionShapes)
            {
                item.Center = new Vector2(item.Center.X + _speed.X, item.Center.Y + _speed.Y);
            }
        }

        public void Update()
        {
            now_state.Update(this, ref now_state);
            UpdateSpeed();
            UpdatePosition();
        }

        public void ControllerAction(SlimDxGame.Controller controller)
        {
            now_state.ControllerAction(this, controller, ref now_state);
        }
    }
}