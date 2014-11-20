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
        public TextBox StartPointXAxis { private get; set; }
        public TextBox StartPointYAxis { private get; set; }
        public TextBox TerminatePointXAxis { private get; set; }
        public TextBox TerminatePointYAxis { private get; set; }
        public ComboBox TypeId { private get; set; }


        public override void Initialize()
        {
            SetTextBoxValue();

            StartPointXAxis.LostFocus += StartPointXAxis_LostFocus;
            StartPointXAxis.LostFocus += GrobalLostFocusEvent;
            StartPointYAxis.LostFocus += StartPointYAxis_LostFocus;
            StartPointYAxis.LostFocus += GrobalLostFocusEvent;
            TerminatePointXAxis.LostFocus += TerminatePointXAxis_LostFocus;
            TerminatePointXAxis.LostFocus += GrobalLostFocusEvent;
            TerminatePointYAxis.LostFocus += TerminatePointYAxis_LostFocus;
            TerminatePointYAxis.LostFocus += GrobalLostFocusEvent;
            TypeId.SelectedIndexChanged += TypeId_SelectedIndexChanged;
        }

        public override void Clean()
        {
            StartPointXAxis.LostFocus -= StartPointXAxis_LostFocus;
            StartPointXAxis.LostFocus -= GrobalLostFocusEvent;
            StartPointYAxis.LostFocus -= StartPointYAxis_LostFocus;
            StartPointYAxis.LostFocus -= GrobalLostFocusEvent;
            TerminatePointXAxis.LostFocus -= TerminatePointXAxis_LostFocus;
            TerminatePointXAxis.LostFocus -= GrobalLostFocusEvent;
            TerminatePointYAxis.LostFocus -= TerminatePointYAxis_LostFocus;
            TerminatePointYAxis.LostFocus -= GrobalLostFocusEvent;
            TypeId.SelectedIndexChanged -= TypeId_SelectedIndexChanged;
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
                pos.Y = TextBoxParser.ToSingle(TerminatePointYAxis.Text);
                TerminatePointYAxis.Text = pos.Y.ToString();
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
                pos.X = TextBoxParser.ToSingle(TerminatePointXAxis.Text);
                TerminatePointXAxis.Text = pos.X.ToString();
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
                pos.Y = TextBoxParser.ToSingle(StartPointYAxis.Text);
                StartPointYAxis.Text = pos.Y.ToString();
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
                pos.X = TextBoxParser.ToSingle(StartPointXAxis.Text);
                StartPointXAxis.Text = pos.X.ToString();
                col.StartingPoint = pos;
                CollisionList[CurrentIndex].CollisionInfo = col;
            }
        }

        protected override void Add()
        {
            if (CollisionList.Count != 0)
            {
                CollisionList[CurrentIndex].Line.Selected = false;
            }
            Object.ExProperty.Collision new_collision = new Object.ExProperty.Collision() 
            {
                CollisionInfo = new StageRW.Property.Collision() 
                {
                    StartingPoint = new SlimDX.Vector2(-1.0f, 0.0f),
                    TerminatePoint = new SlimDX.Vector2(1.0f, 0.0f),
                    TypeId = 0
                },
                Line = new Object.Line()
                {
                    ModelAsset = ModelFactory.FindModel("Box")
                }
            };
            new_collision.Line.Selected = true;
            CollisionList.Add(new_collision);
            CurrentIndex = CollisionList.IndexOf(new_collision);
            SetTextBoxValue();
        }

        void SetTextBoxValue()
        {
            if (CurrentSize != 0)
            {
                var col = CollisionList[CurrentIndex].CollisionInfo;
                StartPointXAxis.Text = TextBoxParser.ToString(col.StartingPoint.X);
                StartPointYAxis.Text = col.StartingPoint.Y.ToString();
                TerminatePointXAxis.Text = col.TerminatePoint.X.ToString();
                TerminatePointYAxis.Text = col.TerminatePoint.Y.ToString();
                TypeId.SelectedIndex = col.TypeId;
            }
        }

        protected override void Decliment()
        {
            CollisionList[CurrentIndex].Line.Selected = false;
            base.Decliment();
            SetTextBoxValue();
            CollisionList[CurrentIndex].Line.Selected = true;
        }

        protected override void Delete()
        {
            CollisionList.RemoveAt(CurrentIndex);
            CurrentIndex = 0;
        }

        protected override void Incliment()
        {
            CollisionList[CurrentIndex].Line.Selected = false;
            base.Incliment();
            SetTextBoxValue();
            CollisionList[CurrentIndex].Line.Selected = true;
        }
    }
}
