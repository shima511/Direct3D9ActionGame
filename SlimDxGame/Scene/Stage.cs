using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SlimDxGame.Scene
{
    class Stage : Scene.Base
    {
        enum ReturnFrag{
            ExitGame,
            ToTitle
        }
        /// <summary>
        /// ステージ番号
        /// </summary>
        int level_id = 0;
        Status.Stage StageState { get; set; }
        StageRW.Reader StageLoader { get; set; }
        ReturnFrag ReturnTo { get; set; }
        Collision.Manager CollisionManager { get; set; }
        Object.Camera Camera { get; set; }
        Object.Player Player { get; set; }
        Controller PlayerController { get; set; }
        Object.Fader Fader { get; set; }
        Object.StateDrawer StateDrawManager { get; set; }
        Object.Item.Factory ItemFactory { get; set; }
        /// <summary>
        /// 現在のゲームの状態
        /// </summary>
        GameState<Stage> CurrentState;
        /// <summary>
        /// ステージの構成要素(衝突有りの地形・アイテムなど)
        /// </summary>
        StageRW.Objects StageComponents { get; set; }
        /// <summary>
        /// ステージに存在するライト
        /// </summary>
        List<Effect.Light> Lights { get; set; }
        /// <summary>
        /// 影
        /// </summary>
        ShadowManager ShadowManage { get; set; }
        /// <summary>
        /// リソース情報
        /// </summary>
        ScriptRW.Properties AssetsList { get; set; }
        /// <summary>
        /// ステージの地形情報
        /// </summary>
        List<Object.Ground.Base> Grounds { get; set; }
        /// <summary>
        /// ポーズメニュー
        /// </summary>
        Menu PauseMenu { get; set; }

        public Stage()
        {
            CurrentState = new LoadingState();
            Lights = new List<Effect.Light>();
            Grounds = new List<Object.Ground.Base>();
        }

        // ステージの読み込みなどを行う
        class LoadingState : GameState<Stage>
        {
            bool ThreadCreated { get; set; }
            bool LoadCompleted { get; set; }

            void InitLayer(GameRootObjects root_objects)
            {
                root_objects.Layers.Capacity = 4;
                for (int i = 0; i < root_objects.Layers.Capacity; i++)
                {
                    root_objects.Layers.Add(new List<Component.IDrawableObject>());
                }
            }

            void LoadAssetList(Stage parent)
            {
                ScriptRW.Reader reader = new ScriptRW.Reader();
                var assets_list = parent.AssetsList;
                reader.Read(out assets_list, "obj_list.txt");
                parent.AssetsList = assets_list;
            }

            void LoadTextures(ScriptRW.Properties assets, AssetContainer<Asset.Texture> tex_container)
            {
                Asset.Texture new_tex;
                foreach (var item in assets.Textures)
                {
                    string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                    new_tex = AssetFactory.TextureFactory.CreateTextureFromFile(Path.Combine(baseDir, item.AssetPath));
                    tex_container.Add(item.Name, new_tex);
                }

                // フェーダー用のテクスチャを生成
                byte[] tex_info = { 255, 255, 255, 255 };
                new_tex = AssetFactory.TextureFactory.CreateFromRawData(1, 1, tex_info);
                tex_container.Add("BlackTexture", new_tex);
            }

            void LoadSounds(AssetContainer<Asset.Sound> sound_container)
            {
            }

            void LoadModels(ScriptRW.Properties properties, AssetContainer<Asset.Model> model_container)
            {
                Asset.Model new_model;
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                foreach (var item in properties.Items)
                {
                    new_model = AssetFactory.ModelFactory.CreateModelFromFile(Path.Combine(baseDir, item.AssetPath));
                    model_container.Add(item.Name, new_model);
                }
                foreach (var item in properties.Enemies)
                {
                    new_model = AssetFactory.ModelFactory.CreateModelFromFile(Path.Combine(baseDir, item.AssetPath));
                    model_container.Add(item.Name, new_model);
                }
                foreach (var item in properties.Decolations)
                {
                    new_model = AssetFactory.ModelFactory.CreateModelFromFile(Path.Combine(baseDir, item.AssetPath));
                    model_container.Add(item.Name, new_model);
                }
            }

            void LoadStage(GameRootObjects root, Stage parent)
            {
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                var components = parent.StageComponents;
                parent.StageLoader.Read(Path.Combine(baseDir, Path.Combine("levels", "stage" + parent.level_id.ToString() + ".dat")), out components);
                parent.StageComponents = components;
                parent.StageState = new Status.Stage()
                {
                    Score = 0,
                    Time = 100
                };
            }

            void LoadFont(GameRootObjects root)
            {
                var font = AssetFactory.FontFactory.CreateFont(new System.Drawing.Font("Arial", 20));
                root.FontContainer.Add("Arial", font);
            }

            void CreateInstance(Stage parent)
            {
                parent.StageLoader = new StageRW.Reader();
                parent.CollisionManager = new Collision.Manager();
                parent.Camera = new Object.Camera();
                parent.Player = new Object.Player();
                parent.PlayerController = new Controller();
                parent.Fader = new Object.Fader();
                parent.ItemFactory = new Object.Item.Factory();
                parent.Grounds = new List<Object.Ground.Base>();
            }

            public int Update(GameRootObjects root_objects, Stage parent, ref GameState<Stage> new_state)
            {
                try
                {
                    if (!ThreadCreated)
                    {
                        CreateInstance(parent);
                        InitLayer(root_objects);
                        ThreadCreated = true;
                    }
                    LoadAssetList(parent);
                    LoadTextures(parent.AssetsList, root_objects.TextureContainer);
                    LoadModels(parent.AssetsList, root_objects.ModelContainer);
                    LoadStage(root_objects, parent);
                    LoadFont(root_objects);

                    LoadCompleted = true;

                    if (LoadCompleted)
                    {
                        List<string> invalid_calls = new List<string>();
                        if (root_objects.IncludeInvalidAsset(ref invalid_calls) || !parent.StageLoader.Valid)
                        {
                            MessageBox.Show("ファイルの読み込みに失敗", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            parent.ReturnTo = ReturnFrag.ExitGame;
                            return -1;
                        }
                        new_state = new InitState();
                    }
                }catch(SystemException ex){
                    MessageBox.Show(ex.Message, "ファイルエラー", MessageBoxButtons.OK);
                    parent.ReturnTo = ReturnFrag.ExitGame;
                    new_state = new InitState();
                    return -1;
                }
                return 0;
            }
        }

        // ステージの配置など初期設定を行う
        class InitState : GameState<Stage>
        {
            void InitCamera(GameRootObjects root_objects,  Stage parent)
            {
                root_objects.Layers[3].Add(parent.Camera);
                parent.PlayerController.Add(parent.Camera);
                parent.Camera.Subject = parent.Player;
            }

            void InitLightEffect(GameRootObjects root_objects, Stage parent)
            {
                var right_light = new Effect.Light(){
                    Index = 0,
                    EnableLight = true,
                    Property = new SlimDX.Direct3D9.Light()
                    {
                        Type = SlimDX.Direct3D9.LightType.Directional,
                        Ambient = System.Drawing.Color.White,
                        Diffuse = System.Drawing.Color.White,
                        Direction = new SlimDX.Vector3(-1.0f, -1.0f, 1.0f)
                    }
                };

                parent.Lights.Add(right_light);

                var left_light = new Effect.Light()
                {
                    Index = 1,
                    EnableLight = true,
                    Property = new SlimDX.Direct3D9.Light()
                    {
                        Type = SlimDX.Direct3D9.LightType.Directional,
                        Ambient = System.Drawing.Color.White,
                        Diffuse = System.Drawing.Color.White,
                        Direction = new SlimDX.Vector3(1.0f, -1.0f, 1.0f)
                    }
                };

                parent.Lights.Add(left_light);

                foreach (var item in parent.Lights)
                {
                    root_objects.Layers[0].Add(item);
                }
            }

            void InitInputManager(GameRootObjects root_objects,  Stage parent)
            {
                root_objects.InputManager.Add(parent.PlayerController);
            }

            void InitPlayer(GameRootObjects root_objects, Stage parent)
            {
                Asset.Model model;
                root_objects.ModelContainer.TryGetValue("TestModel", out model);
                parent.Player.ModelAsset = model;
                var p_pos = parent.StageComponents.Player.Position;
                parent.Player.Position = new SlimDX.Vector3(p_pos.X, p_pos.Y, 0);
                root_objects.UpdateList.Add(parent.Player);
                root_objects.Layers[2].Add(parent.Player);
            }

            void InitDecoration(GameRootObjects root_objects, Stage parent)
            {
                Object.Decolation.Factory factory = new Object.Decolation.Factory();
                factory.ModelContainer = root_objects.ModelContainer;
                foreach (var item in parent.StageComponents.Decolations)
                {
                    Object.Base.Model new_item;
                    factory.Create(item, out new_item);
                    root_objects.Layers[1].Add(new_item);
                }
            }

            [System.Diagnostics.Conditional("DEBUG")]
            void AddCollisionsToDrawList(GameRootObjects root_objects, Stage parent)
            {
                root_objects.Layers[1].Add(parent.CollisionManager);
            }

            void InitCollisionObjects(GameRootObjects root_objects, Stage parent)
            {
                parent.CollisionManager.Player = parent.Player;

                // 地形の衝突判定を追加していく
                foreach (var item in parent.StageComponents.Collisions)
                {
                    Object.Ground.Base new_collision = Object.Ground.Base.CreateGround(item);
                    parent.CollisionManager.Add(new_collision);
                    parent.Grounds.Add(new_collision);
                }

                root_objects.UpdateList.Add(parent.CollisionManager);
                AddCollisionsToDrawList(root_objects, parent);
            }

            void InitStateDrawer(GameRootObjects root_objects, Stage parent)
            {
                parent.StateDrawManager = new Object.StateDrawer(parent.StageState, parent.Player.State);
                Asset.Font font;
                root_objects.FontContainer.TryGetValue("Arial", out font);
                parent.StateDrawManager.Font = font;
                root_objects.UpdateList.Add(parent.StateDrawManager);
                root_objects.Layers[1].Add(parent.StateDrawManager);
            }

            void InitItem(GameRootObjects root_objects, Stage parent)
            {
                parent.ItemFactory.ModelContainer = root_objects.ModelContainer;
                parent.ItemFactory.UpdateList = root_objects.UpdateList;
                parent.ItemFactory.Layers = root_objects.Layers;
                parent.ItemFactory.CollisionManager = parent.CollisionManager;
                parent.ItemFactory.StageStatus = parent.StageState;

                foreach (var item in parent.StageComponents.Items)
                {
                    Object.Item.IBase new_item;
                    parent.ItemFactory.Create(item, out new_item);
                    parent.CollisionManager.Add(new_item);
                    root_objects.UpdateList.Add(new_item);
                    root_objects.Layers[1].Add(new_item);
                }
            }

            void InitPauseMenu(GameRootObjects root_objects, Stage parent)
            {
                Asset.Texture tex;
                root_objects.TextureContainer.TryGetValue("BlackTexture", out tex);

                // メニューを表示するための黒背景
                var menu_bg = new Object.Base.Sprite()
                {
                    Texture = tex,
                    Scale = new SlimDX.Vector2(Core.Game.AppInfo.Width * 2, Core.Game.AppInfo.Height * 2),
                    Color = new SlimDX.Color4(0.8f, 0.0f, 0.0f, 0.0f),
                };

                root_objects.TextureContainer.TryGetValue("BlackTexture", out tex);

                var cursor_position_list = new List<SlimDX.Vector2>();
                cursor_position_list.Add(new SlimDX.Vector2(Core.Game.AppInfo.Width / 2 - 30, Core.Game.AppInfo.Height / 4));
                cursor_position_list.Add(new SlimDX.Vector2(Core.Game.AppInfo.Width / 2 - 30, Core.Game.AppInfo.Height / 2));
                cursor_position_list.Add(new SlimDX.Vector2(Core.Game.AppInfo.Width / 2 - 30, Core.Game.AppInfo.Height * 3 / 4));

                // メニュー用のカーソル
                var menu_cursor = new Object.Cursor()
                {
                    Texture = tex,
                    MoveAction = () => { },
                    Color = new SlimDX.Color4(1.0f, 1.0f, 1.0f, 1.0f),
                    Scale = new SlimDX.Vector2(30.0f, 30.0f),
                    Position = new SlimDX.Vector2(60.0f, 60.0f),
                    PositionList = cursor_position_list
                };

                // メニュー表示用のフォント
                Asset.Font font;
                root_objects.FontContainer.TryGetValue("Arial", out font);

                // 項目を追加
                List<Object.Base.String> columns = new List<Object.Base.String>();
                columns.AddRange(new []{
                    new Object.Base.String(){Text = "Return Game", Position = new SlimDX.Vector2(Core.Game.AppInfo.Width / 2, Core.Game.AppInfo.Height / 4)},
                    new Object.Base.String(){Text = "To Title", Position = new SlimDX.Vector2(Core.Game.AppInfo.Width / 2, Core.Game.AppInfo.Height / 2)},
                    new Object.Base.String(){Text = "Exit Game", Position = new SlimDX.Vector2(Core.Game.AppInfo.Width / 2, Core.Game.AppInfo.Height * 3 / 4)}
                });


                parent.PauseMenu = new Menu()
                {
                    Cursor = menu_cursor,
                    BackGround = menu_bg,
                    DefaultFont = font,
                    Columns = columns,
                    ColumnInterval = 30
                    
                };
            }

            void AddShadow(GameRootObjects root_objects, List<Object.Shadow> list, Stage parent)
            {
                Asset.Texture shadow_tex;
                root_objects.TextureContainer.TryGetValue("Shadow", out shadow_tex);
                Vertex vertex;
                PolygonFactory.CreateSquarePolygon(out vertex);
                // 影をつける
                var shadow = new Object.Shadow()
                {
                    Texture = shadow_tex,
                    Position = new SlimDX.Vector3(),
                    Scale = new SlimDX.Vector2(3.0f, 3.0f),
                    Rotation = new SlimDX.Vector3(),
                    Vertex = vertex,
                    Owner = parent.Player
                };
                list.Add(shadow);
            }

            void InitShadow(GameRootObjects root_objects, Stage parent)
            {
                parent.ShadowManage = new ShadowManager();

                var list = new List<Object.Shadow>();
                AddShadow(root_objects, list, parent);
                parent.ShadowManage.Shadows = list;

                parent.ShadowManage.Grounds = parent.Grounds;
                root_objects.UpdateList.Add(parent.ShadowManage);
                root_objects.Layers[2].Add(parent.ShadowManage);
            }

            public int Update(GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                InitCamera(root_objects,  parent);

                InitLightEffect(root_objects, parent);

                InitInputManager(root_objects,  parent);

                InitPlayer(root_objects,  parent);

                InitDecoration(root_objects, parent);

                InitItem(root_objects, parent);

                InitCollisionObjects(root_objects, parent);

                InitStateDrawer(root_objects, parent);

                InitShadow(root_objects, parent);

                InitPauseMenu(root_objects, parent);

                new_state = new FadeInState(root_objects,  parent);
                return 0;
            }
        }

        // フェードイン
        class FadeInState : GameState<Stage>
        {
            public FadeInState(GameRootObjects root_objects,  Stage parent)
            {
                Asset.Texture tex;
                root_objects.TextureContainer.TryGetValue("BlackTexture", out tex);
                parent.Fader.Texture = tex;
                parent.Fader.Scale = new SlimDX.Vector2(Core.Game.AppInfo.Width * 2, Core.Game.AppInfo.Height * 2);
                parent.Fader.FadingTime = 120;
                parent.Fader.Color = new SlimDX.Color4(1.0f, 0.0f, 0.0f, 0.0f);
                parent.Fader.Effect = Object.Fader.Flag.FADE_IN;
                root_objects.Layers[2].Add(parent.Fader);
                root_objects.UpdateList.Add(parent.Fader);
            }

            void RemoveFadeInEffect( GameRootObjects root_objects,  Stage parentects)
            {
                root_objects.Layers[2].Remove(parentects.Fader);
                root_objects.UpdateList.Remove(parentects.Fader);
            }

            public int Update( GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                if(parent.Fader.Color.Alpha <= 0.1f){
                    RemoveFadeInEffect( root_objects,  parent);
                    new_state = new CountDownState();
                }
                return 0;
            }            
        }

        // フェードイン終了後、操作可能になるまでカウントダウンを行う状態
        class CountDownState : GameState<Stage>
        {
            int time = 0;

            public int Update( GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                //time++;
                //if(time >= 120){
                    new_state = new PlayingState( root_objects,  parent);
                //}
                return 0;
            }
        }

        // プレイ中(操作可能)の状態
        class PlayingState : GameState<Stage>
        {
            PauseMenuDispatcher Dispatcher { get; set; }

            void EnableOperate(Stage parent)
            {
                // プレイヤーを操作可能に
                parent.PlayerController.Add(parent.Player);
                parent.PlayerController.Add(Dispatcher);
            }

            public PlayingState(GameRootObjects root_objects, Stage parent)
            {
                Dispatcher = new PauseMenuDispatcher();
                EnableOperate(parent);
            }

            public int Update(GameRootObjects root_objects, Stage parent, ref GameState<Stage> new_state)
            {
                if (Dispatcher.PauseButtonPushed)
                {
                    new_state = new PausingState(root_objects, parent);
                }
                return 0;
            }
        }

        // ステージクリアした状態
        class ClearedState : GameState<Stage>
        {
            public int Update( GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                return 0;
            }
        }

        // ミスした状態
        class MissedState : GameState<Stage>
        {
            public int Update( GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                return 0;
            }
        }

        // ポーズ状態
        class PausingState : GameState<Stage>
        {
            void ShowMenu(GameRootObjects root_objects, Stage parent)
            {
                parent.PlayerController.Add(parent.PauseMenu);
                root_objects.Layers[2].Add(parent.PauseMenu);
                root_objects.UpdateList.Add(parent.PauseMenu);
            }

            void CloseMenu(GameRootObjects root_objects, Stage parent)
            {
                parent.PlayerController.Remove(parent.PauseMenu);
                root_objects.Layers[2].Remove(parent.PauseMenu);
                root_objects.UpdateList.Remove(parent.PauseMenu);
            }

            public PausingState(GameRootObjects root_objects, Stage parent)
            {
                parent.PlayerController.Remove(parent.Player);
                ShowMenu(root_objects, parent);
            }

            public int Update(GameRootObjects root_objects, Stage parent, ref GameState<Stage> new_state)
            {
                if(parent.PauseMenu.Selected)
                {
                    CloseMenu(root_objects, parent);
                    switch (parent.PauseMenu.Cursor.Index)
                    {
                        case 0:
                            new_state = new PlayingState(root_objects, parent);
                            break;
                        case 1:
                            parent.ReturnTo = ReturnFrag.ToTitle;
                            new_state = new FadeOutState(root_objects, parent);
                            break;
                        case 2:
                            parent.ReturnTo = ReturnFrag.ExitGame;
                            new_state = new FadeOutState(root_objects, parent);
                            break;
                        default:
                            break;
                    }
                }
                return 0;
            }
        }

        // フェードアウト状態
        class FadeOutState : GameState<Stage>
        {
            void AddFadeEffect(GameRootObjects root_objects, Stage parent)
            {
                root_objects.Layers[2].Add(parent.Fader);
                root_objects.UpdateList.Add(parent.Fader);
            }

            void RemoveFadeEffect(GameRootObjects root_objects, Stage parent)
            {
                root_objects.Layers[2].Remove(parent.Fader);
                root_objects.UpdateList.Remove(parent.Fader);
            }

            public FadeOutState(GameRootObjects root_objects, Stage parent)
            {
                parent.Fader.FadingTime = 60;
                parent.Fader.Color = new SlimDX.Color4(0.0f, 0.0f, 0.0f, 0.0f);
                parent.Fader.Effect = Object.Fader.Flag.FADE_OUT;
                AddFadeEffect(root_objects, parent);
            }

            public int Update(GameRootObjects root_objects, Stage parent, ref GameState<Stage> new_state)
            {
                if (parent.Fader.Color.Alpha >= 0.9f)
                {
                    RemoveFadeEffect(root_objects, parent);
                    return -1;
                }
                return 0;
            }
        }

        public override int Update(GameRootObjects root_objects, ref Scene.Base new_scene)
        {
            int ret_val = 0;
            if (CurrentState.Update(root_objects, this, ref CurrentState) == -1)
            {
                this.ExitScene(root_objects);
                switch (ReturnTo)
                {
                    case ReturnFrag.ToTitle:
                        new_scene = new Scene.Title();
                        break;
                    case ReturnFrag.ExitGame:
                        ret_val = -1;
                        break;
                }
            }
            return ret_val;
        }
    }
}
