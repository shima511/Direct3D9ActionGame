using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator
{
    public partial class LevelCreator : Form
    {
        PropertyForm propertyForm = new PropertyForm();
        List<Object.IBase> objects = new List<Object.IBase>();
        Object.Camera camera;
        Object.Player player = new Object.Player();
        string current_filename;
        GraphicDevice graphic_device;
        public Asset.Factory.ModelFactory ModelFactory { get; private set; }
        public StageObjectController CurrentController { get; set; }
        BinaryParser.Objects _stage_object = new BinaryParser.Objects()
        {
            Collisions = new List<BinaryParser.Property.Collision>(),
            Items = new List<BinaryParser.Property.Item>(),
            Decolations = new List<BinaryParser.Property.Decolation>(),
            Enemies = new List<BinaryParser.Property.Enemy>(),
            Player = new BinaryParser.Property.Player(),
            Stage = new BinaryParser.Property.Stage()
        };
        public BinaryParser.Objects StageObject { get { return _stage_object; } set { _stage_object = value; } }

        public LevelCreator()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Text = "無題";
        }

        protected override void OnShown(EventArgs e)
        {
            graphic_device = new GraphicDevice(this);
            graphic_device.Initialize();

            ModelFactory = new Asset.Factory.ModelFactory(graphic_device.D3DDevice);
            propertyForm.Owner = this;
            propertyForm.Show();

            player.ModelAsset = ModelFactory.FindModel("Sphere");
            player.PlayerInfo = StageObject.Player;

            camera = new Object.Camera(this);
            objects.Add(camera);
            objects.Add(player);

            base.OnShown(e);
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OpenFileStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    BinaryParser.Reader reader = new BinaryParser.Reader();
                    BinaryParser.Objects stage_objects;
                    reader.Read(diag.FileName, out stage_objects);
                    StageObject = stage_objects;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            camera.MouseAction(e);
            base.OnMouseMove(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Tab)
            {
                propertyForm.Activate();
            }
            foreach (var item in objects)
            {
                item.InputAction(e);
            }
            CurrentController.KeyAction(e);
            this.Invalidate();
            base.OnKeyDown(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ModelFactory.Dispose();
            graphic_device.Dispose();
            base.OnClosing(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                graphic_device.D3DDevice.Clear(SlimDX.Direct3D9.ClearFlags.Target | SlimDX.Direct3D9.ClearFlags.ZBuffer, System.Drawing.Color.DarkBlue, 1.0f, 0);
                graphic_device.D3DDevice.BeginScene();

                foreach (var item in objects)
                {
                    item.Update();
                    item.Draw(graphic_device.D3DDevice);
                }

                graphic_device.D3DDevice.EndScene();
                graphic_device.D3DDevice.Present();
            }
            catch (SlimDX.Direct3D9.Direct3D9Exception ex)
            {
                if(ex.ResultCode == SlimDX.Direct3D9.ResultCode.DeviceLost)
                {
                    graphic_device.D3DDevice.Reset(graphic_device.SettingParam);
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
            finally
            {
                base.OnPaint(e);
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void OverSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (current_filename == null)
            {
                NewSaveToolStripMenuItem_Click(sender, e);
            }
        }

        private void NewSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.Filter = "datファイル(.dat)| *.dat";
            if(diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                current_filename = System.IO.Path.GetFileName(diag.FileName);
                this.Text = current_filename;
            }
        }
    }
}
