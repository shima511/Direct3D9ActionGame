﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.PropertyController
{
    class Enemies : StageObjectController
    {
        public List<Object.ExProperty.Enemy> EnemyList { get; set; }
        public MaskedTextBox PositionXAxis { private get; set; }
        public MaskedTextBox PositionYAxis { private get; set; }
        public ComboBox TypeId { private get; set; }
        public override int CurrentSize
        {
            get
            {
                return EnemyList.Count;
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
        }

        public override void Initialize()
        {
            PositionXAxis.LostFocus += PositionXAxis_LostFocus;
            PositionXAxis.LostFocus += GrobalLostFocusEvent;
            PositionYAxis.LostFocus += PositionYAxis_LostFocus;
            PositionYAxis.LostFocus += GrobalLostFocusEvent;
            TypeId.SelectedIndexChanged += TypeId_SelectedIndexChanged;
        }

        void TypeId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EnemyList.Count != 0)
            {
                var dec = EnemyList[CurrentIndex].EnemyInfo;
                dec.TypeId = TypeId.SelectedIndex;
                EnemyList[CurrentIndex].EnemyInfo = dec;
            }
        }

        void PositionYAxis_LostFocus(object sender, EventArgs e)
        {
            if (EnemyList.Count != 0)
            {
                var dec = EnemyList[CurrentIndex].EnemyInfo;
                var pos = dec.Position;
                pos.Y = MaskedTextParser.ToSingle(PositionYAxis.Text);
                dec.Position = pos;
                EnemyList[CurrentIndex].EnemyInfo = dec;
            }
        }

        void PositionXAxis_LostFocus(object sender, EventArgs e)
        {
            if (EnemyList.Count != 0)
            {
                var dec = EnemyList[CurrentIndex].EnemyInfo;
                var pos = dec.Position;
                pos.X = MaskedTextParser.ToSingle(PositionXAxis.Text);
                dec.Position = pos;
                EnemyList[CurrentIndex].EnemyInfo = dec;
            }
        }

        protected override void Add()
        {
            if (EnemyList.Count != 0)
            {
                EnemyList[CurrentIndex].Selected = false;
            }
            Object.ExProperty.Enemy new_Enemy = new Object.ExProperty.Enemy()
            {
                EnemyInfo = new BinaryParser.Property.Enemy()
                {
                    Position = new SlimDX.Vector2(),
                    TypeId = 0
                },
                ModelAsset = ModelFactory.FindModel("Sphere")
            };
            new_Enemy.Selected = true;
            EnemyList.Add(new_Enemy);
            CurrentIndex = EnemyList.IndexOf(new_Enemy);
            SetTextBoxValue();
        }

        void SetTextBoxValue()
        {
            if (CurrentSize != 0)
            {
                var col = EnemyList[CurrentIndex].EnemyInfo;
                PositionXAxis.Text = col.Position.X.ToString();
                PositionYAxis.Text = col.Position.Y.ToString();
                TypeId.SelectedIndex = col.TypeId;
            }
        }

        protected override void Decliment()
        {
            EnemyList[CurrentIndex].Selected = false;
            base.Decliment();
            SetTextBoxValue();
            EnemyList[CurrentIndex].Selected = true;
        }

        protected override void Delete()
        {
            EnemyList.RemoveAt(CurrentIndex);
            CurrentIndex = 0;
        }

        protected override void Incliment()
        {
            EnemyList[CurrentIndex].Selected = false;
            base.Incliment();
            SetTextBoxValue();
            EnemyList[CurrentIndex].Selected = true;
        }
    }
}