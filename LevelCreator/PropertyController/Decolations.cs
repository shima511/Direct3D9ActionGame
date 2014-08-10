using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.PropertyController
{
    class Decolations : StageObjectController
    {
        public List<Object.ExProperty.Decolation> DecolationList { get; set; }
        public MaskedTextBox PositionXAxis { private get; set; }
        public MaskedTextBox PositionYAxis { private get; set; }
        public MaskedTextBox PositionZAxis { private get; set; }
        public ComboBox TypeId { private get; set; }
        public override int CurrentSize
        {
            get
            {
                return DecolationList.Count;
            }
            set
            {
            }
        }

        public override void Clean()
        {
            PositionXAxis.LostFocus -= PositionXAxis_LostFocus;
            PositionXAxis.LostFocus -= GrobalLostFocusEvent;
            PositionYAxis.LostFocus -= PositionYAxis_LostFocus;
            PositionYAxis.LostFocus -= GrobalLostFocusEvent;
            PositionZAxis.LostFocus -= PositionZAxis_LostFocus;
            PositionZAxis.LostFocus -= GrobalLostFocusEvent;
            TypeId.SelectedIndexChanged -= TypeId_SelectedIndexChanged;
        }

        public override void Initialize()
        {
            PositionXAxis.LostFocus += PositionXAxis_LostFocus;
            PositionXAxis.LostFocus += GrobalLostFocusEvent;
            PositionYAxis.LostFocus += PositionYAxis_LostFocus;
            PositionYAxis.LostFocus += GrobalLostFocusEvent;
            PositionZAxis.LostFocus += PositionZAxis_LostFocus;
            PositionZAxis.LostFocus += GrobalLostFocusEvent;
            TypeId.SelectedIndexChanged += TypeId_SelectedIndexChanged;
        }

        void TypeId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DecolationList.Count != 0)
            {
                var dec = DecolationList[CurrentIndex].DecolationInfo;
                dec.TypeId = TypeId.SelectedIndex;
                DecolationList[CurrentIndex].DecolationInfo = dec;
            }
        }

        void PositionZAxis_LostFocus(object sender, EventArgs e)
        {
            if (DecolationList.Count != 0)
            {
                var dec = DecolationList[CurrentIndex].DecolationInfo;
                var pos = dec.Position;
                pos.Z = MaskedTextParser.ToSingle(PositionZAxis.Text);
                dec.Position = pos;
                DecolationList[CurrentIndex].DecolationInfo = dec;              
            }
        }

        void PositionYAxis_LostFocus(object sender, EventArgs e)
        {
            if (DecolationList.Count != 0)
            {
                var dec = DecolationList[CurrentIndex].DecolationInfo;
                var pos = dec.Position;
                pos.Y = MaskedTextParser.ToSingle(PositionYAxis.Text);
                dec.Position = pos;
                DecolationList[CurrentIndex].DecolationInfo = dec;
            }
        }

        void PositionXAxis_LostFocus(object sender, EventArgs e)
        {
            if (DecolationList.Count != 0)
            {
                var dec = DecolationList[CurrentIndex].DecolationInfo;
                var pos = dec.Position;
                pos.X = MaskedTextParser.ToSingle(PositionXAxis.Text);
                dec.Position = pos;
                DecolationList[CurrentIndex].DecolationInfo = dec;
            }
        }

        protected override void Add()
        {
            if (DecolationList.Count != 0)
            {
                DecolationList[CurrentIndex].Selected = false;
            }
            Object.ExProperty.Decolation new_decolation = new Object.ExProperty.Decolation()
            {
                DecolationInfo = new StageRW.Property.Decolation(){
                    Position = new SlimDX.Vector3(),
                    TypeId = 0
                },
                ModelAsset = ModelFactory.FindModel("Sphere")
            };
            new_decolation.Selected = true;
            DecolationList.Add(new_decolation);
            CurrentIndex = DecolationList.IndexOf(new_decolation);
            SetTextBoxValue();
        }

        void SetTextBoxValue()
        {
            if (CurrentSize != 0)
            {
                var col = DecolationList[CurrentIndex].DecolationInfo;
                PositionXAxis.Text = col.Position.X.ToString();
                PositionYAxis.Text = col.Position.Y.ToString();
                PositionZAxis.Text = col.Position.Z.ToString();
                TypeId.SelectedIndex = col.TypeId;
            }
        }

        protected override void Decliment()
        {
            if (CurrentSize != 0)
            {
                DecolationList[CurrentIndex].Selected = false;
                base.Decliment();
                SetTextBoxValue();
                DecolationList[CurrentIndex].Selected = true;
            }
        }

        protected override void Delete()
        {
            DecolationList.RemoveAt(CurrentIndex);
            CurrentIndex = 0;
        }

        protected override void Incliment()
        {
            if (CurrentSize != 0)
            {
                DecolationList[CurrentIndex].Selected = false;
                base.Incliment();
                SetTextBoxValue();
                DecolationList[CurrentIndex].Selected = true;
            }
        }

    }
}
