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
        private float _range = (float)Math.PI / 4;
        private Vector3 _eye_position = new Vector3(0.0f, 1.0f, -20.0f);
        private Vector3 _at_position = new Vector3(0.0f, 0.0f, 0.0f);
        private Vector3 _up_direction = new Vector3(0.0f, 1.0f, 0.0f);
        public Vector3 EyePosition { get { return _eye_position; } set { _eye_position = value; } }
        public Vector3 AtPosition { get { return _at_position; } set { _at_position = value; } }
        public Vector3 UpDirection { get { return _up_direction; } set { _up_direction = value; } }

        public Camera(Form f)
        {
            form = f;
        }

        public override void Update()
        {

        }
        public override void InputAction(KeyEventArgs e)
        {

        }
        public override void Draw(SlimDX.Direct3D9.Device dev)
        {
            var view_mat = Matrix.LookAtLH(EyePosition, AtPosition, UpDirection);
            var proj_mat = Matrix.PerspectiveFovLH(_range, (float)form.Width / form.Height, 0.1f, 20.0f);
            dev.SetTransform(TransformState.View, view_mat);
            dev.SetTransform(TransformState.Projection, proj_mat);
        }
    }
}
