﻿using System;
using SlimDX;
using SlimDX.Direct3D9;

namespace SlimDxGame.Object
{
    class Camera : Component.IDrawableObject, Component.IOperableObject
    {
        private bool _is_visible = true;
        public bool IsVisible { get { return _is_visible; } set { _is_visible = value; } }
        private float _range = (float)Math.PI / 4;
        private Vector3 _eye_position = new Vector3(0.0f, 1.0f, -20.0f);
        private Vector3 _at_position = new Vector3(0.0f, 0.0f, 0.0f);
        private Vector3 _up_direction = new Vector3(0.0f, 1.0f, 0.0f);
        public Vector3 EyePosition { get { return _eye_position; } set { _eye_position = value; } }
        public Vector3 AtPosition { get { return _at_position; } set { _at_position = value; } }
        public Vector3 UpDirection { get { return _up_direction; } set { _up_direction = value; } }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            var view_mat = Matrix.LookAtLH(EyePosition, AtPosition, UpDirection);
            var proj_mat = Matrix.PerspectiveFovLH(_range, (float)Core.Game.AppInfo.Width / Core.Game.AppInfo.Height, 0.1f, 20.0f);
            dev.SetTransform(TransformState.View, view_mat);
            dev.SetTransform(TransformState.Projection, proj_mat);
        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }

        private void ZoomIn()
        {
            if(_range < Math.PI / 2){
                _range += (float)Math.PI / 100;
            }
        }

        private void ZoomOut()
        {
            if (_range > 0.0f)
            {
                _range -= (float)Math.PI / 100;
            }
        }

        public void ControllerAction(SlimDxGame.Controller controller)
        {
            if (controller.UpButton.IsBeingPressed())
            {
                ZoomIn();
            }
            if (controller.DownButton.IsBeingPressed())
            {
                ZoomOut();
            }
        }
    }
}