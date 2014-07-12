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
        public List<BinaryParser.Property.Collision> CollisionList { get; set; }
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
            var col = CollisionList[CurrentIndex];
            col.TypeId = TypeId.SelectedIndex;
            CollisionList[CurrentIndex] = col;
        }

        void TerminatePointYAxis_LostFocus(object sender, EventArgs e)
        {
            var col = CollisionList[CurrentIndex];
            var pos = col.TerminatePoint;
            pos.Y = float.Parse(TerminatePointYAxis.Text);
            col.TerminatePoint = pos;
            CollisionList[CurrentIndex] = col;
        }

        void TerminatePointXAxis_LostFocus(object sender, EventArgs e)
        {
            var col = CollisionList[CurrentIndex];
            var pos = col.TerminatePoint;
            pos.X = float.Parse(TerminatePointXAxis.Text);
            col.TerminatePoint = pos;
            CollisionList[CurrentIndex] = col;
        }

        void StartPointYAxis_LostFocus(object sender, EventArgs e)
        {
            var col = CollisionList[CurrentIndex];
            var pos = col.StartingPoint;
            pos.Y = float.Parse(StartPointYAxis.Text);
            col.StartingPoint = pos;
            CollisionList[CurrentIndex] = col;
        }

        void StartPointXAxis_LostFocus(object sender, EventArgs e)
        {
            var col = CollisionList[CurrentIndex];
            var pos = col.StartingPoint;
            pos.X = float.Parse(StartPointXAxis.Text);
            col.StartingPoint = pos;
            CollisionList[CurrentIndex] = col;
        }

        protected override void Add()
        {
            BinaryParser.Property.Collision new_collision = new BinaryParser.Property.Collision();
            new_collision.StartingPoint = new SlimDX.Vector2(-1.0f, 0.0f);
            new_collision.TerminatePoint = new SlimDX.Vector2(1.0f, 0.0f);
            new_collision.TypeId = 0;
            CollisionList.Add(new_collision);
        }

        protected override void Decliment()
        {
            base.Decliment();
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
        }
    }
}
