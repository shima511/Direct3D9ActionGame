using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

namespace SlimDxGame.Utility
{
    class Menu : Component.IUpdateObject, Component.IDrawableObject, Component.IOperableObject
    {
        private bool _is_visible = true;
        public bool IsVisible { get { return _is_visible; } set { _is_visible = value; } }
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
        /// <summary>
        /// 表示する項目リスト
        /// </summary>
        public List<Object.Base.String> Columns { get; set; }
        /// <summary>
        /// メニューの子供
        /// </summary>
        public List<Menu> ChildMenus { get; set; }
        /// <summary>
        /// メニューの親
        /// </summary>
        public Menu ParentMenu { get; set; }
        /// <summary>
        /// メニューが表示中の場合、trueを返します。
        /// </summary>
        public bool Showing { get; private set; }
        /// <summary>
        /// 表示した瞬間のアクション
        /// </summary>
        public event Action OnShown;
        /// <summary>
        /// 閉じる瞬間のアクション
        /// </summary>
        public event Action OnClose;
        /// <summary>
        /// 現在のメニュー
        /// </summary>
        Menu CurrentMenu { get; set; }

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

        void ChangeCurrentMenu(SlimDxGame.Controller controller)
        {
            if (controller.AButton.IsPressed())
            {
                if (ChildMenus[Cursor.Index] != null)
                {
                    CurrentMenu = ChildMenus[Cursor.Index];
                }
            }
            else if (controller.BButton.IsPressed())
            {
                if (ParentMenu != null)
                {
                    CurrentMenu = ParentMenu;
                }
            }
        }

        public void ControllerAction(SlimDxGame.Controller controller)
        {
            if (Showing)
            {
                Cursor.ControllerAction(controller);
                Selected = controller.AButton.IsPressed();
                ChangeCurrentMenu(controller);
                if (controller.StartButton.IsPressed())
                {
                    Selected = true;
                    Showing = false;
                }
                if(OnClose != null) OnClose();
            }
            else
            {
                if (controller.StartButton.IsPressed())
                {
                    Selected = false;
                    Showing = true;
                }
                if(OnShown != null) OnShown();
            }
        }
    }
}
