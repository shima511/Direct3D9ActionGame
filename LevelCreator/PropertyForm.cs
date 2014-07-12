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
    public partial class PropertyForm : Form
    {
        public PropertyForm()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.CollisionType.SelectedIndex = 0;
            this.tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
        }

        protected override void OnShown(EventArgs e)
        {
            LevelCreator form = this.Owner as LevelCreator;
            form.CurrentController = new PropertyController.Collisions()
            {
                CollisionList = form.StageObject.Collisions,
                StartPointXAxis = this.StartPointXAxisTextBox,
                StartPointYAxis = this.StartPointYAxisTextBox,
                TerminatePointXAxis = this.TerminatePointXAxisTextBox,
                TerminatePointYAxis = this.TerminatePointYAxisTextBox,
                TypeId = this.CollisionType
            };

            base.OnShown(e);
        }

        void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LevelCreator form = this.Owner as LevelCreator;
            switch (this.tabControl.SelectedIndex)
            {
                case 0:
                    form.CurrentController = new PropertyController.Collisions()
                    {
                        CollisionList = form.StageObject.Collisions,
                        StartPointXAxis = this.StartPointXAxisTextBox,
                        StartPointYAxis = this.StartPointYAxisTextBox,
                        TerminatePointXAxis = this.TerminatePointXAxisTextBox,
                        TerminatePointYAxis = this.TerminatePointYAxisTextBox,
                        TypeId = this.CollisionType,
                        ModelFactory = form.ModelFactory
                    };
                    break;
                case 1:
                    // TODO アイテム
                    break;
                case 2:
                    // TODO 装飾
                    break;
                case 3:
                    // TODO 敵
                    break;
                case 4:
                    // TODO プレイヤー
                    break;
                case 5:
                    // TODO ステージ
                    break;
            }
            form.CurrentController.Initialize();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }
    }
}
