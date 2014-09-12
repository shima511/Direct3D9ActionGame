using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using SlimDxGame.Utility;

namespace SlimDxGame.Scene
{
    class Title : Scene.Base
    {
        enum ReturnFlag
        {
            ExitGame,
            ToNextScene
        };
        // BackGround
        Object.Base.Model model_obj = new Object.Base.Model();
        // タイトル文字
        Object.Base.String title_label = new Object.Base.String();
        // BGM
        Asset.Music bgm;
        // 
        ReturnFlag game_flag;
        // コントローラー
        Controller controller = new Controller();
        // カメラ
        Object.Camera camera = new Object.Camera();
        // フェーダー
        Object.Fader fader = new Object.Fader();
        // メニュー
        Utility.Menu menu = new Utility.Menu();

        GameState<Title> CurrentState = new LoadingState();

        class LoadingState : GameState<Title>
        {
            bool thread_created = false;
            bool load_completed = false;
            Thread thread;
            Object.LoadingScreen loading_screen = new Object.LoadingScreen();

            void AddFont(AssetContainer<Asset.Font> font_container)
            {
                var font = AssetFactory.FontFactory.CreateFont(new System.Drawing.Font("Arial", 20));
                font_container.Add("SimpleFont", font);
            }

            void AddLoadingScreen(GameRootObjects root_objects)
            {
                Asset.Font font;
                root_objects.FontContainer.TryGetValue("SimpleFont", out font);
                loading_screen.Font = font;
                root_objects.UpdateList.Add(loading_screen);
                root_objects.Layers[1].Add(loading_screen);
            }

            void RemoveLoadingScreen(GameRootObjects root_objects)
            {
                root_objects.UpdateList.Remove(loading_screen);
                root_objects.Layers[1].Remove(loading_screen);
            }

            void LoadTextures(AssetContainer<Asset.Texture> tex_container)
            {
                Asset.Texture new_tex;

                // 読み込み
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                new_tex = AssetFactory.TextureFactory.CreateTextureFromFile(Path.Combine(baseDir, Path.Combine("toons", "pony_fluttershy.bmp")));
                tex_container.Add("test", new_tex);

                // フェーダー用のテクスチャを生成
                byte[] tex_info = { 255, 255, 255, 255 };
                new_tex = AssetFactory.TextureFactory.CreateFromRawData(1, 1, tex_info);
                tex_container.Add("BlackTexture", new_tex);
            }

            void LoadSounds(AssetContainer<Asset.Sound> sound_container)
            {
                Asset.Sound new_sound;
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                new_sound = AssetFactory.AudioMediaFactory.CreateSoundFromFile(Path.Combine(baseDir, Path.Combine("sounds", "MusicMono.wav")));
                sound_container.Add("test_sound", new_sound);
            }

            void LoadModels(AssetContainer<Asset.Model> model_container)
            {
                Asset.Model new_model;
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                new_model = AssetFactory.ModelFactory.CreateModelFromFile(Path.Combine(baseDir, Path.Combine("models", "Bus_Timetable01.x")));
                model_container.Add("test_model", new_model);
            }

            void LoadAssets(object args)
            {
                var root_objects = (GameRootObjects)args;

                LoadTextures( root_objects.TextureContainer);
                LoadSounds( root_objects.SoundContainer);
                LoadModels( root_objects.ModelContainer);

                Thread.Sleep(300);

                load_completed = true;
            }

            public int Update(GameRootObjects root_objects, Title parent, ref GameState<Title> new_state)
            {
                int ret_val = 0;
                // スレッドが作られていない場合
                if (!thread_created)
                {
                    // レイヤー追加
                    for (int i = 0; i < 3; i++)
                    {
                        root_objects.Layers.Add(new List<Component.IDrawableObject>());
                    }

                    // インプットマネージャクラスにコントローラーを追加
                    root_objects.InputManager.Add(parent.controller);

                    // フォント追加
                    AddFont(root_objects.FontContainer);

                    // ローディング画面を作成
                    AddLoadingScreen(root_objects);

                    // マルチスレッド開始
                    thread = new Thread(new ParameterizedThreadStart(LoadAssets));
                    thread.IsBackground = true;
                    thread.Start(root_objects);

                    thread_created = true;
                }

                // ローディング終了時
                if (load_completed)
                {
                    // スレッド終了
                    thread.Abort();

                    // ローディング画面を外す
                    RemoveLoadingScreen( root_objects);
                    List<string> object_names = new List<string>();
                    if (root_objects.IncludeInvalidAsset(ref object_names))
                    {
                        string err_objects = new string(new char[0]);
                        object_names.ForEach(delegate(string object_name)
                        {
                            err_objects += object_name;
                        });
                        System.Windows.Forms.MessageBox.Show("ロードに失敗しました。", "ローディング失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        System.Diagnostics.Debug.Assert(false, "ロードに失敗したアセットが存在しました。\n一覧：\n" + err_objects);
                        parent.game_flag = ReturnFlag.ExitGame;
                        ret_val = -1;
                    }
                    else
                    {
                        new_state = new InitState();
                    }
                }
                return ret_val;
            }
        }

        class InitState : GameState<Title>
        {
            void SetMenuCursor(GameRootObjects root_objects, Utility.Menu menu)
            {
                var cursor = new Object.Cursor();
                // テクスチャ貼り付け
                Asset.Texture tex;
                root_objects.TextureContainer.TryGetValue("test", out tex);
                cursor.Texture = tex;

                // カーソルの位置を決める
                var positions = new List<SlimDX.Vector2>();
                positions.Add(new SlimDX.Vector2(30.0f, 30.0f));
                positions.Add(new SlimDX.Vector2(30.0f, 60.0f));
                cursor.PositionList = positions;

                // カーソルを動かした時の効果
                Asset.Sound sound;
                root_objects.SoundContainer.TryGetValue("test_sound", out sound);
                cursor.OnMove += () =>
                {
                    sound.Play();
                };

                menu.Cursor = cursor;
            }

            void SetMenuFont(GameRootObjects root_objects, Utility.Menu menu)
            {
                Asset.Font font;
                root_objects.FontContainer.TryGetValue("SimpleFont", out font);
                menu.DefaultFont = font;
            }

            void SetMenuColumns(Utility.Menu menu)
            {
                List<Object.Base.String> columns = new List<Object.Base.String>();

                string[] column_texts = {"Game Start", "Quit Game"};

                for (int i = 0; i < column_texts.Length;i++ )
                {
                    var new_column = new Object.Base.String();
                    new_column.Text = column_texts[i];
                    columns.Add(new_column);
                }
                
                menu.Columns = columns;
            }

            void AddMenu(GameRootObjects root_objects, Title parent)
            {
                // 設定
                SetMenuFont( root_objects,  parent.menu);
                SetMenuCursor( root_objects,  parent.menu);
                SetMenuColumns( parent.menu);

                // リストへの追加
                root_objects.Layers[0].Add(parent.menu);
                root_objects.UpdateList.Add(parent.menu);
            }

            void InitBGM( GameRootObjects root_objects,  Title parent)
            {
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                parent.bgm = AssetFactory.AudioMediaFactory.CreateMusicFromFile(Path.Combine(baseDir, Path.Combine("sounds", "MusicMono.wav")), 100, 6000);
            }

            void InitBackGround( GameRootObjects root_objects,  Title parent)
            {
                Asset.Model model;
                root_objects.ModelContainer.TryGetValue("test_model", out model);
                parent.model_obj.Position = new SlimDX.Vector3(0.0f, 0.0f, -8.0f);
                parent.model_obj.ModelAsset = model;
                root_objects.Layers[0].Add(parent.model_obj);
            }

            void InitCamera( GameRootObjects root_objects,  Title parent)
            {
                root_objects.Layers[0].Add(parent.camera);
            }

            void InitTitleLabel( GameRootObjects root_objects,  Title parent)
            {
                Asset.Font font;
                root_objects.FontContainer.TryGetValue("SimpleFont", out font);
                parent.title_label.Font = font;
                parent.title_label.Text = "Sample Game";
                parent.title_label.Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 1 / 3, 10.0f);
                root_objects.Layers[0].Add(parent.title_label);
            }

            void InitVolume(GameRootObjects root_objects)
            {
                foreach (var item in root_objects.SoundContainer.Values)
                {
                    item.Volume = root_objects.Settings.SEVolume;
                }
            }

            public int Update( GameRootObjects root_objects,  Title parent, ref GameState<Title> new_state)
            {
                // メニュー画面を追加
                AddMenu( root_objects,  parent);
                // BGMを追加
                InitBGM( root_objects,  parent);
                // カメラを追加
                InitCamera( root_objects,  parent);
                // 背景を追加
                InitBackGround( root_objects,  parent);
                // タイトル文字を追加
                InitTitleLabel( root_objects,  parent);

                new_state = new FadeInState( root_objects,  parent);
                return 0;
            }
        }

        class FadeInState : GameState<Title>
        {
            public FadeInState( GameRootObjects root_objects,  Title parent)
            {
                AddFadeInEffect( root_objects,  parent);
            }

            void Exit( GameRootObjects root_objects,  Title parent)
            {
                RemoveFadeInEffect( root_objects,  parent);
            }

            void AddFadeInEffect( GameRootObjects root_objects,  Title parent)
            {
                Asset.Texture tex;
                root_objects.TextureContainer.TryGetValue("BlackTexture", out tex);
                parent.fader.Texture = tex;
                parent.fader.Scale = new SlimDX.Vector2(Core.Game.AppInfo.Width * 2, Core.Game.AppInfo.Height * 2);
                parent.fader.FadingTime = 120;
                parent.fader.Color = new SlimDX.Color4(1.0f, 0.0f, 0.0f, 0.0f);
                parent.fader.Effect = Object.Fader.Flag.FADE_IN;
                root_objects.Layers[2].Add(parent.fader);
                root_objects.UpdateList.Add(parent.fader);
            }

            void RemoveFadeInEffect( GameRootObjects root_objects,  Title parent)
            {
                root_objects.Layers[2].Remove(parent.fader);
                root_objects.UpdateList.Remove(parent.fader);
            }

            public int Update( GameRootObjects root_objects,  Title parent, ref GameState<Title> new_state)
            {
                // フェードのアルファ値が0.1以下になったら次のステートに
                if (parent.fader.Color.Alpha <= 0.1f)
                {
                    Exit( root_objects,  parent);
                    new_state = new MenuState(parent);
                }
                return 0;
            }
        }

        class MenuState : GameState<Title>
        {
            void RemoveMenu( GameRootObjects root_objects,  Title parent)
            {
                root_objects.Layers[0].Remove(parent.menu);
                root_objects.UpdateList.Remove(parent.menu);
                parent.controller.Add(parent.menu);
            }

            public MenuState(Title parent)
            {
                // メニューを操作可能に
                parent.controller.Add(parent.menu);
            }

            void Exit( GameRootObjects root_objects,  Title parent)
            {
                RemoveMenu( root_objects,  parent);
            }

            public int Update( GameRootObjects root_objects,  Title parent, ref GameState<Title> new_state)
            {

                if (parent.menu.Selected)
                {
                    Exit( root_objects,  parent);
                    switch (parent.menu.Cursor.Index)
                    {
                        case 0:
                            parent.game_flag = Title.ReturnFlag.ToNextScene;
                            new_state = new FadeOutState( root_objects,  parent);
                            break;
                        case 1:
                            parent.game_flag = Title.ReturnFlag.ExitGame;
                            new_state = new FadeOutState( root_objects,  parent);
                            break;
                        default:
                            break;
                    }
                }
                return 0;
            }

        }

        class FadeOutState : GameState<Title>
        {
            void AddFadeOutEffect( GameRootObjects root_objects,  Title parent)
            {
                Asset.Texture tex;
                root_objects.TextureContainer.TryGetValue("BlackTexture", out tex);
                parent.fader.Texture = tex;
                parent.fader.Scale = new SlimDX.Vector2(Core.Game.AppInfo.Width * 2, Core.Game.AppInfo.Height * 2);
                parent.fader.FadingTime = 120;
                parent.fader.Color = new SlimDX.Color4(0.0f, 0.0f, 0.0f, 0.0f);
                parent.fader.Effect = Object.Fader.Flag.FADE_OUT;
                root_objects.Layers[2].Add(parent.fader);
                root_objects.UpdateList.Add(parent.fader);
            }

            void RemoveFadeOutEffect( GameRootObjects root_objects,  Title parent)
            {
                root_objects.Layers[2].Remove(parent.fader);
                root_objects.UpdateList.Remove(parent.fader);
            }

            void Exit( GameRootObjects root_objects,  Title parent)
            {
                RemoveFadeOutEffect( root_objects,  parent);
            }

            public FadeOutState( GameRootObjects root_objects,  Title parent)
            {
                AddFadeOutEffect( root_objects,  parent);
            }

            public int Update( GameRootObjects root_objects,  Title parent, ref GameState<Title> new_state)
            {
                if (parent.fader.Color.Alpha >= 0.9f)
                {
                    Exit( root_objects,  parent);
                    return -1;
                }
                return 0;
            }
        }

        public override int Update( GameRootObjects root_objects, ref Scene.Base new_scene){
            int ret_val = 0;
            if (CurrentState.Update(root_objects, this, ref CurrentState) != 0)
            {
                base.ExitScene(root_objects);
                switch (game_flag)
                {
                    case Title.ReturnFlag.ExitGame:
                        ret_val = -1;
                        break;
                    case Title.ReturnFlag.ToNextScene:
                        new_scene = new Stage();
                        break;
                    default:
                        break;
                }
            }
            return ret_val;
        }
    }
}
