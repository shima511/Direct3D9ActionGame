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
        GraphicDevice graphic_device;

        public LevelCreator()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            graphic_device = new GraphicDevice(this);
            graphic_device.Initialize();
            propertyForm.Owner = this;
            propertyForm.Show();
        }

        private void 編集ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 終了XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ファイルを開くStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            foreach (var item in objects)
            {
                item.InputAction(e);
            }
            this.Invalidate();
            base.OnKeyDown(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            graphic_device.Dispose();
            base.OnClosing(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                graphic_device.D3DDevice.Clear(SlimDX.Direct3D9.ClearFlags.Target | SlimDX.Direct3D9.ClearFlags.ZBuffer, System.Drawing.Color.AliceBlue, 1.0f, 0);
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
    }
}
