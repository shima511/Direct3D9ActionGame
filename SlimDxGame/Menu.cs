using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

namespace SlimDxGame
{
    class Menu : Component.IUpdateObject, Component.IDrawableObject, Component.IOperableObject
    {
        private bool _is_visible = true;
        public bool IsVisible { get { return _is_visible; } set { _is_visible = value; } }
        private Menu now_menu;
        public bool Fixed { get; private set; }
        public Object.Base.Sprite Frame { private get; set; }
        public Object.Cursor Cursor { get; set; }
        public Asset.Font Font { private get; set; }
        public List<Object.Base.String> Columns { private get; set; }
        public Vector2 StartPosition { private get; set; }
        public int ColumnInterval { private get; set; }
        public List<Menu> ChildMenus { get; set; }

        public Menu()
        {
            now_menu = this;
        }

        public void Update()
        {
            Cursor.Update();
        }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {

        }
        
        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {
            Cursor.Draw2D(dev);
            for (int i = 0; i < Columns.Count; i++)
            {
                SlimDX.Color4 color = new Color4(1.0f, 1.0f, 1.0f);
                Columns[i].Font = Font;
                Columns[i].Position = new Vector2(StartPosition.X, StartPosition.Y + ColumnInterval * i);
                Columns[i].Draw2D(dev);
            }
        }

        public void ControllerAction(SlimDxGame.Controller controller)
        {
            Cursor.ControllerAction(controller);
            Fixed = controller.AButton.IsPressed();
        }
    }
}
