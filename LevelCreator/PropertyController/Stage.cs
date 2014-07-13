using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.PropertyController
{
    class Stage : StageObjectController
    {
        public MaskedTextBox LimitLineLeft { private get; set; }
        public MaskedTextBox LimitLineTop { private get; set; }
        public MaskedTextBox LimitLineRight { private get; set; }
        public MaskedTextBox LimitLineBottom { private get; set; }
        public MaskedTextBox LimitTime { private get; set; }

        public override void Initialize()
        {
        }
    
    }
}
