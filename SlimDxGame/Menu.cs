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
        /// <summary>
        /// 項目が選択された場合、trueを返します。
        /// </summary>
        public bool Selected { get; private set; }
        /// <summary>
        /// メニューの枠
        /// </summary>
        public Object.Base.Sprite Frame { private get; set; }
        /// <summary>
        /// メニュー表示時の背景
        /// </summary>
        public Object.Base.Sprite BackGround { private get; set; }
        /// <summary>
        /// メニューのカーソル
        /// </summary>
        public Object.Cursor Cursor { get; set; }
        /// <summary>
        /// メニューのデフォルトフォント
        /// </summary>
        public Asset.Font DefaultFont { private get; set; }
        public List<Object.Base.String> Columns { private get; set; }
        public Vector2 StartPosition { private get; set; }
        /// <summary>
        /// 項目と項目間の間
        /// </summary>
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
            if (BackGround != null)
            {
                BackGround.Draw2D(dev);
            }
            Cursor.Draw2D(dev);
            for (int i = 0; i < Columns.Count; i++)
            {
                if (Columns[i].Font == null)
                {
                    Columns[i].Font = DefaultFont;
                }
                Columns[i].Draw2D(dev);
            }
        }

        public void ControllerAction(SlimDxGame.Controller controller)
        {
            Cursor.ControllerAction(controller);
            Selected = controller.AButton.IsPressed();
        }
    }
}
