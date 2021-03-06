﻿using System;
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
    public partial class MainForm : Form
    {
        PropertyForm propertyForm = new PropertyForm();
        List<Object.IBase> objects = new List<Object.IBase>();
        Object.Camera camera;
        string current_filename;
        GraphicDevice graphic_device;
        public List<Object.IBase> Objects { get { return objects; } set { } }
        public Asset.Factory.ModelFactory ModelFactory { get; private set; }
        public StageObjectController CurrentController { get; set; }
        Object.ExProperty.Property _stage_objects = new Object.ExProperty.Property(); 
        public Object.ExProperty.Property StageObjects { get { return _stage_objects; } set { _stage_objects = value; } }
        public ScriptRW.Properties ObjectMaps { get; private set; }

        public MainForm()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Text = "無題";
        }

        void LoadModelsFromPropertyList(List<ScriptRW.ObjectProperty> list)
        {
            foreach (var item in list)
            {
                ModelFactory.CreateFromFile(item.AssetPath, item.Name);
            }
        }

        void LoadObjectMaps()
        {
            ScriptRW.Reader reader = new ScriptRW.Reader();
            ScriptRW.Properties properties;
            reader.Read(out properties, "obj_list.txt");
            ObjectMaps = properties;

            LoadModelsFromPropertyList(properties.Decolations);
            LoadModelsFromPropertyList(properties.Items);
            LoadModelsFromPropertyList(properties.Enemies);
        }

        void OnSetAssets()
        {
            StageObjects.PlayerInfo.ModelAsset = ModelFactory.FindModel("Sphere");

            foreach (var item in StageObjects.Collisions)
            {
                item.Line.ModelAsset = ModelFactory.FindModel("Box");
            }
            foreach (var item in StageObjects.Decolations)
            {
                string model_name = "";
                foreach (var obj in ObjectMaps.Decolations)
                {
                    if (obj.Id == item.DecolationInfo.TypeId) model_name = obj.Name;
                }
                item.ModelAsset = ModelFactory.FindModel(model_name);
            }
            foreach (var item in StageObjects.Items)
            {
                string model_name = "";
                foreach (var obj in ObjectMaps.Items)
                {
                    if (obj.Id == item.ItemInfo.TypeId) model_name = obj.Name;
                }
                item.ModelAsset = ModelFactory.FindModel(model_name);
            }
            foreach (var item in StageObjects.Enemies)
            {
                string model_name = "";
                foreach (var obj in ObjectMaps.Enemies)
                {
                    if (obj.Id == item.EnemyInfo.TypeId) model_name = obj.Name;
                }
                item.ModelAsset = ModelFactory.FindModel(model_name);
            }

            StageObjects.StageInfo.ModelAsset = ModelFactory.FindModel("Box");
        }

        protected override void OnShown(EventArgs e)
        {
            try
            {
                graphic_device = new GraphicDevice(this);
                graphic_device.Initialize();

                ModelFactory = new Asset.Factory.ModelFactory(graphic_device.D3DDevice);
                propertyForm.Owner = this;
                propertyForm.Show();

                StageObjects.PlayerInfo.PlayerInfo = StageObjects.PlayerInfo.PlayerInfo;
                StageObjects.StageInfo = new Object.ExProperty.Stage()
                {
                    StageInfo = new StageRW.Property.Stage()
                    {
                        LimitLine = new Rectangle()
                        {
                            X = -6,
                            Y = 5,
                            Width = 12,
                            Height = -10
                        },
                        LimitTime = 100
                    }
                };

                camera = new Object.Camera(this);
                objects.Add(camera);
                objects.Add(StageObjects.PlayerInfo);
                objects.Add(StageObjects.StageInfo);

                OnSetAssets();
                LoadObjectMaps();
            }catch(SlimDX.Direct3D9.Direct3D9Exception ex){
                MessageBox.Show("DirectXの初期化に失敗しました。\n" + ex.Message);
            }catch(System.IO.IOException ex){
                MessageBox.Show("読み込みエラーです。\n" + ex.Message);
            }
            finally{
                base.OnShown(e);
            }
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
            using (OpenFileDialog diag = new OpenFileDialog() { Filter = "datファイル(.dat)| *.dat"})
            {
                if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StageRW.Reader reader = new StageRW.Reader();
                    StageRW.Objects stage_objects;
                    reader.Read(diag.FileName, out stage_objects);
                    StageObjects = new Object.ExProperty.Property(stage_objects);
                    
                    OnSetAssets();
                    current_filename = diag.FileName;
                    this.Text = System.IO.Path.GetFileName(current_filename);
                    this.Invalidate();
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
                graphic_device.D3DDevice.EnableLight(0, true);

                graphic_device.D3DDevice.SetRenderState(SlimDX.Direct3D9.RenderState.Lighting, true);
                graphic_device.D3DDevice.SetRenderState(SlimDX.Direct3D9.RenderState.Ambient, System.Drawing.Color.White.ToArgb());

                camera.Update();
                camera.Draw(graphic_device.D3DDevice);

                StageObjects.PlayerInfo.Update();
                StageObjects.PlayerInfo.Draw(graphic_device.D3DDevice);

                StageObjects.StageInfo.Update();
                StageObjects.StageInfo.Draw(graphic_device.D3DDevice);
                foreach (var item in StageObjects.Collisions)
                {
                    item.Update();
                    item.Draw(graphic_device.D3DDevice);
                }
                foreach (var item in StageObjects.Decolations)
                {
                    item.Update();
                    item.Draw(graphic_device.D3DDevice);
                }
                foreach (var item in StageObjects.Enemies)
                {
                    item.Update();
                    item.Draw(graphic_device.D3DDevice);
                }
                foreach (var item in StageObjects.Items)
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
            else
            {
                SaveData();
            }
        }

        void SaveData()
        {
            StageRW.Writer writer = new StageRW.Writer();
            writer.Write(current_filename, StageObjects.ToStructObjects());
        }

        private void NewSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.Filter = "datファイル(.dat)| *.dat";
            if(diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                current_filename = diag.FileName;
                this.Text = System.IO.Path.GetFileName(diag.FileName);
                SaveData();
            }
        }

        private void NewProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
