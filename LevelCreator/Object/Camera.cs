using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D9;

namespace LevelCreator.Object
{
    class Camera : IBase
    {
        Form form;
        readonly float MoveDistance = 0.5f;
        private float _range = (float)Math.PI / 4;
        private Vector3 _eye_position = new Vector3(0.0f, 1.0f, -20.0f);
        private Vector3 _at_position = new Vector3(0.0f, 0.0f, 0.0f);
        private Vector3 _up_direction = new Vector3(0.0f, 1.0f, 0.0f);
        public Vector3 EyePosition { get { return _eye_position; } set { _eye_position = value; } }
        public Vector3 AtPosition { get { return _at_position; } set { _at_position = value; } }
        public Vector3 UpDirection { get { return _up_direction; } set { _up_direction = value; } }
        double rad = - Math.PI / 2;

        public Camera(Form f)
        {
            form = f;
        }

        public void Update()
        {

        }

        public void InputAction(KeyEventArgs e)
        {
            int strength = 1;
            if (e.Control)
            {
                strength = 5;
            }
            if (!e.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        _eye_position.X -= MoveDistance * strength;
                        _at_position.X -= MoveDistance * strength;
                        break;
                    case Keys.Right:
                        _eye_position.X += MoveDistance * strength;
                        _at_position.X += MoveDistance * strength;
                        break;
                    case Keys.Up:
                        _eye_position.Y += MoveDistance * strength;
                        _at_position.Y += MoveDistance * strength;
                        break;
                    case Keys.Down:
                        _eye_position.Y -= MoveDistance * strength;
                        _at_position.Y -= MoveDistance * strength;
                        break;
                }
            }
            else
            {
                var r = Math.Sqrt(Math.Pow(_at_position.Z - _eye_position.Z, 2) + Math.Pow(_at_position.X - _eye_position.X, 2));
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        rad -= 0.02;
                        _eye_position.Z = _at_position.Z + (float)(r * Math.Sin(rad));
                        _eye_position.X = _at_position.X + (float)(r * Math.Cos(rad));
                        break;
                    case Keys.Right:
                        rad += 0.02;
                        _eye_position.Z = _at_position.Z + (float)(r * Math.Sin(rad));
                        _eye_position.X = _at_position.X + (float)(r * Math.Cos(rad));
                        break;
                    case Keys.Up:
                        _eye_position.Z += MoveDistance * strength;
                        _at_position.Z += MoveDistance * strength;
                        break;
                    case Keys.Down:
                        _eye_position.Z -= MoveDistance * strength;
                        _at_position.Z -= MoveDistance * strength;
                        break;
                }
            }
        }

        public void MouseAction(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

            }
        }

        public void Draw(SlimDX.Direct3D9.Device dev)
        {
            var view_mat = Matrix.LookAtLH(EyePosition, AtPosition, UpDirection);
            var proj_mat = Matrix.PerspectiveFovLH(_range, (float)form.Width / form.Height, 5.0f, 100.0f);
            dev.SetTransform(TransformState.View, view_mat);
            dev.SetTransform(TransformState.Projection, proj_mat);
        }
    }
}
