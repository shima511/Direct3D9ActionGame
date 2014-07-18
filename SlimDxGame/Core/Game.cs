using System;
using System.Collections.Generic;
using MikuMikuDance.SlimDX;
using System.IO;
using System.Windows.Forms;

namespace SlimDxGame.Core
{
    class Game : System.Windows.Forms.Form
    {
        static public System.Windows.Forms.Form AppInfo { get; private set; }
        private FPSManager fps_mgr = new FPSManager();
        private GameRootObjects root_objects = new GameRootObjects();
        private DrawManager draw_manager = new DrawManager();
        private Scene.Base now_scene;
        private Device.Graphic graphic_dev;
        private Device.Input input_dev;
        private Device.Audio audio_dev;
        // FPS表示用のフォント
        SlimDxGame.Asset.Font default_font;

        public Game(Scene.Base first_scene)
        {
            now_scene = first_scene;
            this.Text = "サンプルプログラム";
            this.Width = 900;
            this.Height = 600;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            AppInfo = this;
        }

        private bool InitializeDevices(){
            try
            {
                graphic_dev.Initialize();
                input_dev.Initialize();
                audio_dev.Initialize();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "DirectX Initialization Failed", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void TerminateDevices()
        {
            default_font.Release();
            SlimMMDXCore.Instance.Dispose();
            graphic_dev.Terminate();
            input_dev.Terminate();
            audio_dev.Terminate();
        }

        private void MakeInstances()
        {
            graphic_dev = new Device.Graphic(this);
            input_dev = new Device.Input(this);
            audio_dev = new Device.Audio();
        }

        [System.Diagnostics.Conditional("DEBUG")]
        void DrawFPS(SlimDX.Direct3D9.Sprite sprite_dev)
        {
            sprite_dev.Begin(SlimDX.Direct3D9.SpriteFlags.AlphaBlend);
            sprite_dev.Transform = SlimDX.Matrix.Identity;
            fps_mgr.Draw(graphic_dev.D3DSprite, default_font.Resource);
            sprite_dev.End();
        }

        void Update()
        {
            if (now_scene.Update(root_objects, ref now_scene) != 0) this.Close();
            foreach (var item in root_objects.update_list)
            {
                item.Update();
            }
            root_objects.input_manager.Update();
        }

        void Draw(SlimDX.Direct3D9.Device d3d_dev, SlimDX.Direct3D9.Sprite sprite_dev)
        {
            draw_manager.DrawBegin(d3d_dev);
            draw_manager.DrawObjects(d3d_dev, sprite_dev, root_objects.layers);
        }

        private void MainLoop()
        {
            fps_mgr.Begin();
            Update();

            //Draw
            var d3d_dev = graphic_dev.D3DDevice;
            var sprite_dev = graphic_dev.D3DSprite;
            Draw(d3d_dev, sprite_dev);
            DrawFPS(sprite_dev);
            draw_manager.DrawEnd(d3d_dev);

            fps_mgr.End();
        }

        private void InitMMDX()
        {
            string[] toonTexPath = new string[10];
            string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
            for (int i = 1; i <= 10; ++i)
            {
                toonTexPath[i - 1] = Path.Combine(baseDir, Path.Combine("toons", "toon" + i.ToString("00") + ".bmp"));
            }
            try
            {
                SlimMMDXCore.Setup(graphic_dev.D3DDevice, toonTexPath);
                SlimMMDXCore.Instance.UsePhysics = false;
            }
            catch (SlimDX.Direct3D9.Direct3D9Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message + "MMDX失敗");
            }
        }

        private void SetDevice()
        {
            AssetFactory.TextureFactory.device = graphic_dev.D3DDevice;
            AssetFactory.FontFactory.Device = graphic_dev.D3DDevice;
            AssetFactory.ModelFactory.Device = graphic_dev.D3DDevice;
            AssetFactory.AudioMediaFactory.Device = audio_dev.XAudioDevice;
            default_font = AssetFactory.FontFactory.CreateFont(new System.Drawing.Font("Arial", 20));
        }

        private void FreeAllResources()
        {
            root_objects.font_container.DeleteAllObject();
            root_objects.tex_container.DeleteAllObject();
            root_objects.sound_container.DeleteAllObject();
            root_objects.model_container.DeleteAllObject();
        }


        public void Run()
        {
            if(System.Diagnostics.Process.GetProcessesByName(
                System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                MessageBox.Show("既にアプリを起動しています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            MakeInstances();
            if(!InitializeDevices()) return;
            SetDevice();
            InitMMDX();
            SlimDX.Windows.MessagePump.Run(this, MainLoop);
            FreeAllResources();
            TerminateDevices();
        }
    }
}
