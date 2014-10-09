using System;
using SlimDX;
using SlimDX.Direct3D9;
using MikuMikuDance.SlimDX;

namespace SlimDxGame.Object
{
    class Camera : Component.IUpdateObject, Component.IDrawableObject, Component.IOperableObject
    {
        private bool _is_visible = true;
        public bool IsActive { get; set; }
        public bool IsVisible { get { return _is_visible; } set { _is_visible = value; } }
        private float _range = (float)Math.PI / 4;
        /// <summary>
        /// カメラが注視する対象
        /// </summary>
        public Base.Model Subject { private get; set; }
        private Vector3 _eye_position = new Vector3(0.0f, 8.0f, -25.0f);
        private Vector3 _at_position = new Vector3(0.0f, 8.0f, -5.0f);
        private Vector3 _up_direction = new Vector3(0.0f, 1.0f, 0.0f);
        public Vector3 EyePosition { get { return _eye_position; } set { _eye_position = value; } }
        public Vector3 AtPosition { get { return _at_position; } set { _at_position = value; } }
        public Vector3 UpDirection { get { return _up_direction; } set { _up_direction = value; } }
        /// <summary>
        /// 注視点との差
        /// </summary>
        readonly Vector3 Diff = new Vector3(10.0f, 0.0f, 0.0f);
        MMDXCamera mmd_camera = new MMDXCamera();

        public void Update()
        {
            _eye_position.X = Subject.Position.X + Diff.X;
            _at_position.X = Subject.Position.X + Diff.X;
        }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            var view_mat = Matrix.LookAtLH(EyePosition, AtPosition, UpDirection);
            var proj_mat = Matrix.PerspectiveFovLH(_range, (float)Core.Game.AppInfo.Width / Core.Game.AppInfo.Height, 0.1f, 50.0f);
            mmd_camera.ViewMatrix = view_mat;
            mmd_camera.ProjMatrix = proj_mat;
            SlimMMDXCore.Instance.Camera = mmd_camera;
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

        [System.Diagnostics.Conditional("DEBUG")]
        void OperateCamera(SlimDxGame.Controller controller)
        {
            if (controller.UpButton.IsBeingPressed() && controller.SelectButton.IsBeingPressed())
            {
                ZoomIn();
            }
            if (controller.DownButton.IsBeingPressed() && controller.SelectButton.IsBeingPressed())
            {
                ZoomOut();
            }
            if (controller.RightButton.IsBeingPressed() && controller.SelectButton.IsBeingPressed())
            {
                _eye_position.X += 50.0f;
            }
        }

        public void ControllerAction(SlimDxGame.Controller controller)
        {
            OperateCamera(controller);
        }
    }

    public class MMDXCamera : MikuMikuDance.Core.Stages.IMMDXCamera
    {
        /// <summary>
        /// カメラ位置
        /// </summary>
        public Vector3 CameraPos;
        /// <summary>
        /// カメラ方向と距離
        /// </summary>
        public Vector3 CameraVector;
        /// <summary>
        /// カメラの上方向ベクトル
        /// </summary>
        public Vector3 CameraUpVector = Vector3.UnitY;
        /// <summary>
        /// 回転
        /// </summary>
        public Quaternion Rotation = Quaternion.Identity;
        /// <summary>
        /// Near面
        /// </summary>
        public float Near { get; set; }
        /// <summary>
        /// Far面
        /// </summary>
        public float Far { get; set; }
        /// <summary>
        /// 視野角
        /// </summary>
        public float FieldOfView { get; set; }
        /// <summary>
        /// カメラ位置
        /// </summary>
        public Vector3 Position { get { return CameraPos; } set { CameraPos = value; } }
        /// <summary>
        /// ビュー行列
        /// </summary>
        public Matrix ViewMatrix { get; set; }
        /// <summary>
        /// 射影行列
        /// </summary>
        public Matrix ProjMatrix { get; set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MMDXCamera()
        {
            CameraPos = new Vector3(0, 10, 35);
            CameraVector = new Vector3(0, 0, -35);
            Near = 1;
            Far = 50;
        }

        /// <summary>
        /// カメラ情報
        /// </summary>
        /// <param name="aspectRatio">アスペクト比</param>
        /// <param name="view">ビュー情報</param>
        /// <param name="proj">プロジェクション情報</param>
        public void GetCameraParam(float aspectRatio, out  Matrix view, out Matrix proj)
        {
            view = ViewMatrix;
            proj = ProjMatrix;
        }
        /// <summary>
        /// カメラベクトルの設定
        /// </summary>
        /// <param name="newVector">カメラベクトル</param>
        public void SetVector(Vector3 newVector)
        {
            CameraVector = newVector;
        }

        /// <summary>
        /// 視野角の設定/取得
        /// </summary>
        public void SetRotation(Quaternion rot)
        {
            Rotation = rot;
        }

    }
}
