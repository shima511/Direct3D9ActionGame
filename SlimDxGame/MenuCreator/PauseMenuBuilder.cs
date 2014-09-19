using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.MenuCreator
{
    class PauseMenuBuilder : MenuBuilder
    {
        public PauseMenuBuilder(Utility.Menu m)
            : base(m)
        {
        }

        public override void SetBG(Utility.AssetContainer<Asset.Texture> tex_container)
        {
            // メニューを表示するための黒背景
            var menu_bg = new Object.Base.Sprite()
            {
                Texture = tex_container.GetValue("BlackTexture"),
                Scale = new SlimDX.Vector2(Core.Game.AppInfo.Width * 2, Core.Game.AppInfo.Height * 2),
                Color = new SlimDX.Color4(0.8f, 0.0f, 0.0f, 0.0f),
            };

            menu.BackGround = menu_bg;
        }

        public override void SetCursor(Utility.AssetContainer<Asset.Texture> tex_container, Utility.AssetContainer<Asset.Sound> sound_container)
        {
            var cursor_position_list = new List<SlimDX.Vector2>();
            cursor_position_list.Add(new SlimDX.Vector2(Core.Game.AppInfo.Width / 2 - 30, Core.Game.AppInfo.Height / 4));
            cursor_position_list.Add(new SlimDX.Vector2(Core.Game.AppInfo.Width / 2 - 30, Core.Game.AppInfo.Height / 2));
            cursor_position_list.Add(new SlimDX.Vector2(Core.Game.AppInfo.Width / 2 - 30, Core.Game.AppInfo.Height * 3 / 4));

            // メニュー用のカーソル
            var menu_cursor = new Object.Cursor()
            {
                Texture = tex_container.GetValue("BlackTexture"),
                Color = new SlimDX.Color4(1.0f, 1.0f, 1.0f, 1.0f),
                Scale = new SlimDX.Vector2(30.0f, 30.0f),
                Position = new SlimDX.Vector2(60.0f, 60.0f),
                PositionList = cursor_position_list
            };

            var sound = sound_container.GetValue("test_sound");
            menu_cursor.OnMove += () => { sound.Play(); };

            menu.Cursor = menu_cursor;
        }

        public override void SetColumns(Utility.AssetContainer<Asset.Font> font_container)
        {
            // 項目を追加
            List<Object.Base.String> columns = new List<Object.Base.String>();
            columns.AddRange(new[]{
                    new Object.Base.String(){Text = "Return Game", Position = new SlimDX.Vector2(Core.Game.AppInfo.Width / 2, Core.Game.AppInfo.Height / 4)},
                    new Object.Base.String(){Text = "To Title", Position = new SlimDX.Vector2(Core.Game.AppInfo.Width / 2, Core.Game.AppInfo.Height / 2)},
                    new Object.Base.String(){Text = "Exit Game", Position = new SlimDX.Vector2(Core.Game.AppInfo.Width / 2, Core.Game.AppInfo.Height * 3 / 4)}
                });
            menu.Columns = columns;

            menu.DefaultFont = font_container.GetValue("Arial");
        }

        public override void SetReflectionButton(Controller controller)
        {
            menu.CancellButton = controller.BButton;
            menu.ShowButton = controller.StartButton;
            menu.CloseButton = controller.StartButton;
            menu.FixButton = controller.AButton;
        }

        public override void SetFunction(GameRootObjects root_objects)
        {
            menu.OnShown += () =>
            {
                root_objects.Layers[2].Add(menu);
                root_objects.UpdateList.Add(menu);
            };
            menu.OnClose += () =>
            {
                root_objects.Layers[2].Remove(menu);
                root_objects.UpdateList.Remove(menu);
            };
        }

    }
}
