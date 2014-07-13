using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.PropertyController
{
    class Enemies : StageObjectController
    {
        public MaskedTextBox PositionXAxis { private get; set; }
        public MaskedTextBox PositionYAxis { private get; set; }
        public ComboBox TypeId { private get; set; }

        public override void Initialize()
        {

        }
    }
}
