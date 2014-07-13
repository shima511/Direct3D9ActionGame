using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.PropertyController
{
    sealed class Collisions : StageObjectController
    {
        public List<Object.ExProperty.Collision> CollisionList { get; set; }
        public override int CurrentSize
        {
            get
            {
                return CollisionList.Count;
            }
            set
            {
            }
        }
        public MaskedTextBox StartPointXAxis { private get; set; }
        public MaskedTextBox StartPointYAxis { private get; set; }
        public MaskedTextBox TerminatePointXAxis { private get; set; }
        public MaskedTextBox TerminatePointYAxis { private get; set; }
        public ComboBox TypeId { private get; set; }

        public override void Initialize()
        {
            StartPointXAxis.LostFocus += StartPointXAxis_LostFocus;
            StartPointYAxis.LostFocus += StartPointYAxis_LostFocus;
            TerminatePointXAxis.LostFocus += TerminatePointXAxis_LostFocus;
            TerminatePointYAxis.LostFocus += TerminatePointYAxis_LostFocus;
            TypeId.SelectedIndexChanged += TypeId_SelectedIndexChanged;
        }

        void TypeId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CollisionList.Count != 0)
            {
                var col = CollisionList[CurrentIndex].CollisionInfo;
                col.TypeId = TypeId.SelectedIndex;
                CollisionList[CurrentIndex].CollisionInfo = col;
            }
        }

        void TerminatePointYAxis_LostFocus(object sender, EventArgs e)
        {
            if (CollisionList.Count != 0)
            {
                var col = CollisionList[CurrentIndex].CollisionInfo;
                var pos = col.TerminatePoint;
                pos.Y = float.Parse(TerminatePointYAxis.Text);
                col.TerminatePoint = pos;
                CollisionList[CurrentIndex].CollisionInfo = col;
            }
        }

        void TerminatePointXAxis_LostFocus(object sender, EventArgs e)
        {
            if (CollisionList.Count != 0)
            {
                var col = CollisionList[CurrentIndex].CollisionInfo;
                var pos = col.TerminatePoint;
                pos.X = float.Parse(TerminatePointXAxis.Text);
                col.TerminatePoint = pos;
                CollisionList[CurrentIndex].CollisionInfo = col;
            }
        }

        void StartPointYAxis_LostFocus(object sender, EventArgs e)
        {
            if (CollisionList.Count != 0)
            {
                var col = CollisionList[CurrentIndex].CollisionInfo;
                var pos = col.StartingPoint;
                pos.Y = float.Parse(StartPointYAxis.Text);
                col.StartingPoint = pos;
                CollisionList[CurrentIndex].CollisionInfo = col;
            }
        }

        void StartPointXAxis_LostFocus(object sender, EventArgs e)
        {
            if (CollisionList.Count != 0)
            {
                var col = CollisionList[CurrentIndex].CollisionInfo;
                var pos = col.StartingPoint;
                pos.X = float.Parse(StartPointXAxis.Text);
                col.StartingPoint = pos;
                CollisionList[CurrentIndex].CollisionInfo = col;
            }
        }

        protected override void Add()
        {
            Object.ExProperty.Collision new_collision = new Object.ExProperty.Collision() 
            {
                CollisionInfo = new BinaryParser.Property.Collision() 
                {
                    StartingPoint = new SlimDX.Vector2(-1.0f, 0.0f),
                    TerminatePoint = new SlimDX.Vector2(1.0f, 0.0f),
                    TypeId = 0
                }
            };
            CollisionList.Add(new_collision);
            SetTextBoxValue();
        }

        void SetTextBoxValue()
        {
            if (CurrentSize != 0)
            {
                var col = CollisionList[CurrentIndex].CollisionInfo;
                StartPointXAxis.Text = col.StartingPoint.X.ToString();
                StartPointYAxis.Text = col.StartingPoint.Y.ToString();
                TerminatePointXAxis.Text = col.TerminatePoint.X.ToString();
                TerminatePointYAxis.Text = col.TerminatePoint.Y.ToString();
                TypeId.SelectedIndex = col.TypeId;
            }
        }

        protected override void Decliment()
        {
            base.Decliment();
            SetTextBoxValue();
        }

        protected override void Delete()
        {
            if (CollisionList.Count != 0)
            {
                CollisionList.RemoveAt(CurrentIndex);
            }
        }

        protected override void Incliment()
        {
            base.Incliment();
            SetTextBoxValue();
        }
    }
}
