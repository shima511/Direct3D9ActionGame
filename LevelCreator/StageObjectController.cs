using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator
{
    public abstract class StageObjectController
    {
        public delegate void LostFocusEvent(object sender, EventArgs e);

        public event LostFocusEvent OnLostFocus;

        protected void GrobalLostFocusEvent(object sender, EventArgs e)
        {
            OnLostFocus(sender, e);
        }

        protected int _current_index = 0;
        public int CurrentIndex { get { return _current_index; } set { _current_index = value; } }
        public virtual int CurrentSize { get; set; }
        public Asset.Factory.ModelFactory ModelFactory { protected get; set; }

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
            CurrentIndex++;
            if (CurrentIndex >= CurrentSize)
            {
                CurrentIndex = 0;
            }
        }

        protected virtual void Decliment()
        {
            CurrentIndex--;
            if (CurrentIndex <= -1)
            {
                CurrentIndex = CurrentSize - 1;
            }
        }

        protected virtual void Add()
        {

        }

        protected virtual void Delete()
        {

        }

    }
}
