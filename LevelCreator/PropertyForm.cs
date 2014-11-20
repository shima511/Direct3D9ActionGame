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
            MainForm form = this.Owner as MainForm;
            form.CurrentController = new PropertyController.Collisions()
            {
                CollisionList = form.StageObjects.Collisions,
                StartPointXAxis = this.GroundStartPointX,
                StartPointYAxis = this.GroundStartPointY,
                TerminatePointXAxis = this.GroundTerminalPointX,
                TerminatePointYAxis = this.GroundTerminalPointY,
                TypeId = this.CollisionType
            };
            form.CurrentController.Initialize();
            form.CurrentController.ModelFactory = form.ModelFactory;
            form.CurrentController.OnLostFocus += CurrentController_OnLostFocus;

            var dec_list = from p in form.ObjectMaps.Decolations
                           orderby p.Id ascending
                           select p.Name;
            this.DecolationType.Items.AddRange(dec_list.ToArray<string>());
            var item_list = from p in form.ObjectMaps.Items
                            orderby p.Id ascending
                            select p.Name;
            this.ItemType.Items.AddRange(item_list.ToArray<string>());
            var enemy_list = from p in form.ObjectMaps.Enemies
                             orderby p.Id ascending
                             select p.Name;
            this.EnemyType.Items.AddRange(enemy_list.ToArray<string>());
            
            base.OnShown(e);
        }

        void CurrentController_OnLostFocus(object sender, EventArgs e)
        {
            if (this.Owner != null)
            {
                this.Owner.Invalidate();
            }
        }

        void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainForm form = this.Owner as MainForm;
            form.CurrentController.Clean();
            switch (this.tabControl.SelectedIndex)
            {
                case 0:
                    form.CurrentController = new PropertyController.Collisions()
                    {
                        CollisionList = form.StageObjects.Collisions,
                        StartPointXAxis = this.GroundStartPointX,
                        StartPointYAxis = this.GroundStartPointY,
                        TerminatePointXAxis = this.GroundTerminalPointX,
                        TerminatePointYAxis = this.GroundTerminalPointY,
                        TypeId = this.CollisionType,
                    };
                    break;
                case 1:
                    form.CurrentController = new PropertyController.Items()
                    {
                        ItemList = form.StageObjects.Items,
                        PositionXAxis = this.ItemPositionX,
                        PositionYAxis = this.ItemPositionY,
                        TypeId = this.ItemType
                    };
                    break;
                case 2:
                    form.CurrentController = new PropertyController.Decolations()
                    {
                        DecolationList = form.StageObjects.Decolations,
                        PositionXAxis = this.DecolationPositionX,
                        PositionYAxis = this.DecolationPositionY,
                        PositionZAxis = this.DecolationPositionZ,
                        TypeId = this.DecolationType
                    };
                    break;
                case 3:
                    form.CurrentController = new PropertyController.Enemies()
                    {
                        EnemyList = form.StageObjects.Enemies,
                        PositionXAxis = this.EnemyPositionX,
                        PositionYAxis = this.EnemyPositionY,
                        TypeId = this.EnemyType
                    };
                    break;
                case 4:
                    form.CurrentController = new PropertyController.Player() 
                    { 
                        PlayerInfo = form.StageObjects.PlayerInfo,
                        PositionXAxis = this.PlayerPositionX,
                        PositionYAxis = this.PlayerPositionY
                    };
                    break;
                case 5:
                    form.CurrentController = new PropertyController.Stage()
                    {
                        StageInfo = form.StageObjects.StageInfo,
                        LimitLineLeft = this.LimitLineLeft,
                        LimitLineRight = this.LimitLineRight,
                        LimitLineBottom = this.LimitLineBottom,
                        LimitLineTop = this.LimitLineTop,
                        LimitTime = this.LimitTime
                    };
                    break;
            }
            form.CurrentController.ModelFactory = form.ModelFactory;
            form.CurrentController.OnLostFocus += CurrentController_OnLostFocus;
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
