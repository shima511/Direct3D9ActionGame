using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.PropertyController
{
    class Items : StageObjectController
    {
        public List<Object.ExProperty.Item> ItemList { get; set; }
        public TextBox PositionXAxis { private get; set; }
        public TextBox PositionYAxis { private get; set; }
        public ComboBox TypeId { private get; set; }
        public override int CurrentSize
        {
            get
            {
                return ItemList.Count;
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
            TypeId.SelectedIndexChanged -= TypeId_SelectedIndexChanged;
            TypeId.SelectedIndexChanged -= GrobalLostFocusEvent;
        }

        public override void Initialize()
        {
            PositionXAxis.LostFocus += PositionXAxis_LostFocus;
            PositionXAxis.LostFocus += GrobalLostFocusEvent;
            PositionYAxis.LostFocus += PositionYAxis_LostFocus;
            PositionYAxis.LostFocus += GrobalLostFocusEvent;
            TypeId.SelectedIndexChanged += TypeId_SelectedIndexChanged;
            TypeId.SelectedIndexChanged += GrobalLostFocusEvent;
        }

        void TypeId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ItemList.Count != 0)
            {
                var item = ItemList[CurrentIndex].ItemInfo;
                item.TypeId = TypeId.SelectedIndex;
                ItemList[CurrentIndex].ItemInfo = item;
                ItemList[CurrentIndex].ModelAsset = ModelFactory.FindModel(TypeId.Text);
            }
        }

        void PositionYAxis_LostFocus(object sender, EventArgs e)
        {
            if (ItemList.Count != 0)
            {
                var dec = ItemList[CurrentIndex].ItemInfo;
                var pos = dec.Position;
                pos.Y = TextBoxParser.ToSingle(PositionYAxis.Text);
                PositionYAxis.Text = pos.Y.ToString();
                dec.Position = pos;
                ItemList[CurrentIndex].ItemInfo = dec;
            }
        }

        void PositionXAxis_LostFocus(object sender, EventArgs e)
        {
            if (ItemList.Count != 0)
            {
                var dec = ItemList[CurrentIndex].ItemInfo;
                var pos = dec.Position;
                pos.X = TextBoxParser.ToSingle(PositionXAxis.Text);
                PositionXAxis.Text = pos.X.ToString();
                dec.Position = pos;
                ItemList[CurrentIndex].ItemInfo = dec;
            }
        }

        protected override void Add()
        {
            if (ItemList.Count != 0)
            {
                ItemList[CurrentIndex].Selected = false;
            }
            Object.ExProperty.Item new_Item = new Object.ExProperty.Item()
            {
                ItemInfo = new StageRW.Property.Item()
                {
                    Position = new SlimDX.Vector2(),
                    TypeId = 0
                },
                ModelAsset = ModelFactory.FindModel("Sphere")
            };
            new_Item.Selected = true;
            ItemList.Add(new_Item);
            CurrentIndex = ItemList.IndexOf(new_Item);
            SetTextBoxValue();
        }

        void SetTextBoxValue()
        {
            if (CurrentSize != 0)
            {
                var col = ItemList[CurrentIndex].ItemInfo;
                PositionXAxis.Text = col.Position.X.ToString();
                PositionYAxis.Text = col.Position.Y.ToString();
                TypeId.SelectedIndex = col.TypeId;
            }
        }

        protected override void Decliment()
        {
            ItemList[CurrentIndex].Selected = false;
            base.Decliment();
            SetTextBoxValue();
            ItemList[CurrentIndex].Selected = true;
        }

        protected override void Delete()
        {
            ItemList.RemoveAt(CurrentIndex);
            CurrentIndex = 0;
        }

        protected override void Incliment()
        {
            ItemList[CurrentIndex].Selected = false;
            base.Incliment();
            SetTextBoxValue();
            ItemList[CurrentIndex].Selected = true;
        }
    }
}
