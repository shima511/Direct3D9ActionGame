using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator
{
    public class StageObjectController
    {
        protected int _current_index = 0;
        public int CurrentIndex { get { return _current_index; } set { _current_index = value; } }
        public Asset.Factory.ModelFactory ModelFactory { private get; set; }

        public void KeyAction(KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        Add();
                        break;
                    case Keys.D:
                        Delete();
                        break;
                }
            }
            if (e.KeyCode == Keys.Tab && !e.Shift)
            {
                Incliment();
            }
            if (e.KeyCode == Keys.Tab && e.Shift)
            {
                Decliment();
            }
        }

        public virtual void Initialize()
        {

        }

        protected virtual void Incliment()
        {

        }

        protected virtual void Decliment()
        {

        }

        protected virtual void Add()
        {

        }

        protected virtual void Delete()
        {

        }

    }
}
