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
        public bool Fixed { get; private set; }
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
        public bool Showing { get; set; }
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
        public Menu CurrentMenu { get; set; }
        /// <summary>
        /// メニューを開くボタン
        /// </summary>
        public Controller.Button ShowButton { get; set; }
        /// <summary>
        /// メニューを閉じるボタン
        /// </summary>
        public Controller.Button CloseButton { get; set; }
        /// <summary>
        /// 項目を選択するボタン
        /// </summary>
        public Controller.Button FixButton { get; set; }
        /// <summary>
        /// キャンセルボタン
        /// </summary>
        public Controller.Button CancellButton { get; set; }

        public Menu()
        {
            ChildMenus = new List<Menu>();
            Fixed = false;
            Showing = false;
        }

        public void Update()
        {
            if (CurrentMenu == null && Cursor != null) Cursor.Update();
            else CurrentMenu.Update();
        }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {

        }

        void DrawMenu(SlimDX.Direct3D9.Sprite dev)
        {
            if (BackGround != null)
            {
                BackGround.Draw2D(dev);
            }
            if(Cursor != null) Cursor.Draw2D(dev);
            for (int i = 0; i < Columns.Count; i++)
            {
                if (Columns[i].Font == null)
                {
                    Columns[i].Font = DefaultFont;
                }
                Columns[i].Draw2D(dev);
            }
        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {
            if (CurrentMenu == null) DrawMenu(dev);
            else CurrentMenu.Draw2D(dev);
        }

        void ChangeCurrentMenu(SlimDxGame.Controller controller)
        {
            if (controller.IsPressed(FixButton) && ChildMenus[Cursor.Index] != null)
            {
                CurrentMenu = ChildMenus[Cursor.Index];
                CurrentMenu.Showing = true;
            }
            else if (controller.IsPressed(CancellButton) && ParentMenu != null)
            {
                ParentMenu.CurrentMenu = null;
            }
        }

        void Show()
        {
            Fixed = false;
            Showing = true;
            OnShown();
        }

        public void Close()
        {
            Fixed = true;
            Showing = false;
            OnClose();
        }

        void OperateMenu(SlimDxGame.Controller controller)
        {
            if (Showing)
            {
                Cursor.ControllerAction(controller);
                ChangeCurrentMenu(controller);
                // 子供メニューが存在しない状態で選択ボタンが押された場合、メニューを閉じる
                if (controller.IsPressed(FixButton) && ChildMenus[Cursor.Index] == null)
                {
                    Close();
                }
            }
            else if (controller.IsPressed(ShowButton) && ParentMenu == null)
            {
                Show();
            }
        }

        public void ControllerAction(SlimDxGame.Controller controller)
        {
            if (CurrentMenu == null) OperateMenu(controller);
            else CurrentMenu.ControllerAction(controller);
        }
    }
}
