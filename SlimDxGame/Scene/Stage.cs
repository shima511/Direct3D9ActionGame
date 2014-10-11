using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SlimDxGame.Utility;

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
        /// <summary>
        /// ステージの状態
        /// </summary>
        Status.Stage StageState { get; set; }
        /// <summary>
        /// ステージ読み込みクラス
        /// </summary>
        StageRW.Reader StageLoader { get; set; }
        /// <summary>
        /// 状態の戻り先
        /// </summary>
        ReturnFrag ReturnTo { get; set; }
        /// <summary>
        /// 衝突判定管理クラス
        /// </summary>
        Collision.Manager CollisionManager { get; set; }
        /// <summary>
        /// カメラ
        /// </summary>
        Object.Camera Camera { get; set; }
        /// <summary>
        /// プレイヤークラス
        /// </summary>
        Object.Player Player { get; set; }
        /// <summary>
        /// コントローラー
        /// </summary>
        Controller PlayerController { get; set; }
        /// <summary>
        /// フェーダー
        /// </summary>
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
        Utility.Menu PauseMenu { get; set; }
        /// <summary>
        /// オブジェクト表示・非表示マネージャ
        /// </summary>
        SpawnManager SpawnManage { get; set; }
        /// <summary>
        /// ステージの境界線
        /// </summary>
        Object.Boundary Boundary { get; set; }
        /// <summary>
        /// タイムカウンター
        /// </summary>
        TimeCounter Counter { get; set; }
        /// <summary>
        /// 背景
        /// </summary>
        Object.Base.SquarePolygon BackGround { get; set; }

        public Stage()
        {
            CurrentState = new LoadingState();
            Lights = new List<Effect.Light>();
            Grounds = new List<Object.Ground.Base>();
            Counter = new TimeCounter(Core.FPSManager.FPS);
        }

        // ステージの読み込みなどを行う
        class LoadingState : GameState<Stage>
        {
            bool ThreadCreated { get; set; }
            bool LoadCompleted { get; set; }

            public LoadingState()
            {
                ThreadCreated = false;
                LoadCompleted = false;
            }

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
                Asset.Sound new_sound;
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                byte[] data_array = File.ReadAllBytes(Path.Combine(baseDir, Path.Combine("sounds", "MusicMono.wav")));
                new_sound = AssetFactory.AudioMediaFactory.CreateSoundFromMemory(data_array);
                sound_container.Add("test_sound", new_sound);
            }

            void LoadMMDModel(Stage parent)
            {
                var model = MikuMikuDance.SlimDX.SlimMMDXCore.Instance.LoadModelFromFile("models/human/human.pmd");
                var motion1 = MikuMikuDance.SlimDX.SlimMMDXCore.Instance.LoadMotionFromFile("models/human/motion/Run.vmd");
                model.AnimationPlayer.AddMotion("Run", motion1, MikuMikuDance.Core.Motion.MMDMotionTrackOptions.None);
                var motion2 = MikuMikuDance.SlimDX.SlimMMDXCore.Instance.LoadMotionFromFile("models/human/motion/JumpStart.vmd");
                model.AnimationPlayer.AddMotion("JumpStart", motion2, MikuMikuDance.Core.Motion.MMDMotionTrackOptions.None);
                model.AnimationPlayer["JumpStart"].Stop();
                var motion3 = MikuMikuDance.SlimDX.SlimMMDXCore.Instance.LoadMotionFromFile("models/human/motion/Jump.vmd");
                model.AnimationPlayer.AddMotion("Jump", motion3, MikuMikuDance.Core.Motion.MMDMotionTrackOptions.None);
                model.AnimationPlayer["Jump"].Stop();
                parent.Player.MMDModel = model;
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
                parent.SpawnManage = new SpawnManager()
                {
                    CenterObject = parent.Player,
                    CollisionManager = parent.CollisionManager
                };
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
                    LoadSounds(root_objects.SoundContainer);
                    LoadTextures(parent.AssetsList, root_objects.TextureContainer);
                    LoadMMDModel(parent);
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
                }
                catch (NullReferenceException ex)
                {
                    MessageBox.Show(ex.Message, "Nullを参照しています", MessageBoxButtons.OK);
                    parent.ReturnTo = ReturnFrag.ExitGame;
                    new_state = new InitState();
                    return -1;
                }
                catch (SystemException ex)
                {
                    MessageBox.Show(ex.Message, "ファイルエラー", MessageBoxButtons.OK);
                    parent.ReturnTo = ReturnFrag.ExitGame;
                    new_state = new InitState();
                    return -1;
                }

                return 0;
            }
        }

        // ステージのオブジェクトを初期化する
        class InitState : GameState<Stage>
        {
            void InitVolume(GameRootObjects root_objects)
            {
                foreach (var item in root_objects.SoundContainer.Values)
                {
                    if (item != null) item.Volume = root_objects.Settings.SEVolume;
                }
            }

            void InitCamera(GameRootObjects root_objects, Stage parent)
            {
                root_objects.UpdateList.Add(parent.Camera);
                root_objects.Layers[3].Add(parent.Camera);
                parent.PlayerController.Add(parent.Camera);
                parent.Camera.Subject = parent.Player;
                parent.Camera.IsActive = true;
            }

            void InitLightEffect(GameRootObjects root_objects, Stage parent)
            {
                var right_light = new Effect.Light()
                {
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

            void InitInputManager(GameRootObjects root_objects, Stage parent)
            {
                root_objects.InputManager.Add(parent.PlayerController);
            }

            void InitPlayer(GameRootObjects root_objects, Stage parent)
            {
                Asset.Model model;
                root_objects.ModelContainer.TryGetValue("TestModel", out model);
                parent.Player.ModelAsset = model;
                root_objects.UpdateList.Add(parent.Player);
                root_objects.Layers[2].Add(parent.Player);
            }

            void InitDecoration(GameRootObjects root_objects, Stage parent)
            {
                Object.Decolation.Factory factory = new Object.Decolation.Factory();
                factory.ModelContainer = root_objects.ModelContainer;
                foreach (var item in parent.StageComponents.Decolations)
                {
                    Object.IFieldObject new_item;
                    factory.Create(item, out new_item);
                    new_item.IsActive = true;
                    new_item.IsVisible = false;
                    root_objects.UpdateList.Add(new_item);
                    root_objects.Layers[0].Add(new_item);
                    parent.SpawnManage.Add(new_item);
                }
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
                parent.ShadowManage.IsActive = true;
                root_objects.UpdateList.Add(parent.ShadowManage);
                root_objects.Layers[2].Add(parent.ShadowManage);
            }

            void InitDialogMenu(GameRootObjects root_objects, Stage parent)
            {
                Utility.Menu dialog_menu = new Utility.Menu();
                MenuCreator.MenuDirector m_director = new MenuCreator.MenuDirector()
                {
                    RootObjects = root_objects,
                    Controller = parent.PlayerController
                };
                dialog_menu.ParentMenu = parent.PauseMenu;
                dialog_menu.ChildMenus.Add(null);
                dialog_menu.ChildMenus.Add(null);
                parent.PauseMenu.ChildMenus.AddRange(new[]{
                    null,
                    m_director.Create(new MenuCreator.DialogMenuBuilder(dialog_menu)),
                    m_director.Create(new MenuCreator.DialogMenuBuilder(dialog_menu))
                });
            }

            void InitPauseMenu(GameRootObjects root_objects, Stage parent)
            {
                Utility.Menu pause_menu = new Utility.Menu();
                pause_menu.IsActive = true;
                MenuCreator.MenuDirector m_director = new MenuCreator.MenuDirector()
                {
                    RootObjects = root_objects,
                    Controller = parent.PlayerController
                };
                parent.PauseMenu = m_director.Create(new MenuCreator.PauseMenuBuilder(pause_menu));
                InitDialogMenu(root_objects, parent);
            }

            [System.Diagnostics.Conditional("DEBUG")]
            void AddCollisionsToDrawList(GameRootObjects root_objects, Stage parent)
            {
                root_objects.Layers[1].Add(parent.CollisionManager);
            }

            void InitCollisionObjects(GameRootObjects root_objects, Stage parent)
            {
                parent.CollisionManager.Player = parent.Player;
                parent.CollisionManager.IsActive = true;

                // 地形の衝突判定を追加していく
                foreach (var item in parent.StageComponents.Collisions)
                {
                    Object.Ground.Base new_collision = Object.Ground.Base.CreateGround(item);
                    new_collision.IsVisible = false;
                    parent.SpawnManage.Add(new_collision);
                    parent.CollisionManager.Add(new_collision);
                    parent.Grounds.Add(new_collision);
                }

                root_objects.UpdateList.Add(parent.CollisionManager);
                AddCollisionsToDrawList(root_objects, parent);
            }

            void InitStateDrawer(GameRootObjects root_objects, Stage parent)
            {
                parent.StateDrawManager = new Object.StateDrawer(parent.StageState, parent.Player);
                Asset.Font font;
                root_objects.FontContainer.TryGetValue("Arial", out font);
                parent.StateDrawManager.Font = font;
                parent.StateDrawManager.IsVisible = true;
                parent.StateDrawManager.IsActive = true;
                root_objects.UpdateList.Add(parent.StateDrawManager);
                root_objects.Layers[1].Add(parent.StateDrawManager);
            }

            void InitItem(GameRootObjects root_objects, Stage parent)
            {
                parent.ItemFactory.ModelContainer = root_objects.ModelContainer;
                parent.ItemFactory.StageStatus = parent.StageState;

                foreach (var item in parent.StageComponents.Items)
                {
                    Object.Item.IBase new_item;
                    parent.ItemFactory.Create(item, out new_item);
                    new_item.IsActive = true;
                    new_item.IsVisible = false;
                    root_objects.UpdateList.Add(new_item);
                    root_objects.Layers[0].Add(new_item);
                    parent.SpawnManage.Add(new_item);
                }
            }

            void InitSpawnManager(GameRootObjects root_objects, Stage parent)
            {
                parent.SpawnManage.IsActive = true;
                root_objects.UpdateList.Add(parent.SpawnManage);
            }

            void InitStageBoundary(Stage parent)
            {
                parent.Boundary = new Object.Boundary(parent.StageComponents.Stage.LimitLine);

                parent.CollisionManager.Add(parent.Boundary);
            }

            void InitBackGround(GameRootObjects root_objects, Stage parent)
            {
                var tex = AssetFactory.TextureFactory.CreateTextureFromFile("tex/Desert.jpg");
                root_objects.TextureContainer.Add("BackGround", tex);
                Asset.Texture shadow_tex;
                root_objects.TextureContainer.TryGetValue("BackGround", out shadow_tex);
                Vertex vertex;
                PolygonFactory.CreateSquarePolygon(out vertex);
                parent.BackGround = new Object.Base.SquarePolygon()
                {
                    Texture = shadow_tex,
                    Position = new SlimDX.Vector3(0.0f, 5.0f, 0.0f),
                    Scale = new SlimDX.Vector2(10.0f, 10.0f),
                    Rotation = new SlimDX.Vector3(- (float) Math.PI / 2 , 0.0f, 0.0f),
                    Vertex = vertex
                };
                parent.BackGround.IsVisible = true;
                root_objects.Layers[0].Add(parent.BackGround);
            }

            public int Update(GameRootObjects root_objects, Stage parent, ref GameState<Stage> new_state)
            {
                InitVolume(root_objects);

                InitCamera(root_objects, parent);

                InitLightEffect(root_objects, parent);

                InitInputManager(root_objects, parent);

                InitPlayer(root_objects, parent);

                InitDecoration(root_objects, parent);

                InitShadow(root_objects, parent);

                InitPauseMenu(root_objects, parent);

                InitCollisionObjects(root_objects, parent);

                InitStateDrawer(root_objects, parent);

                InitItem(root_objects, parent);

                InitSpawnManager(root_objects, parent);

                InitStageBoundary(parent);

                InitBackGround(root_objects, parent);

                new_state = new ArrangeState();
                return 0;
            }
        }

        // ステージの配置を行う
        class ArrangeState : GameState<Stage>
        {
            void InitPlayer(Stage parent)
            {
                var p_pos = parent.StageComponents.Player.Position;
                parent.Player.ResetState();
                parent.Player.Position = new SlimDX.Vector3(p_pos.X, p_pos.Y, 0);
                parent.Player.IsActive = true;
            }

            void InitLimitTime(GameRootObjects root_objects, Stage parent)
            {
                parent.StageState.Score = 0;
                parent.StageState.Time = 0;
                parent.Counter.ResetTime();
            }

            public int Update(GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                InitPlayer(parent);

                InitLimitTime(root_objects, parent);

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
                parent.Fader.IsActive = true;
                root_objects.Layers[2].Add(parent.Fader);
                root_objects.UpdateList.Add(parent.Fader);
            }

            void RemoveFadeInEffect(GameRootObjects root_objects, Stage parentects)
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
                time++;
                if(time >= 0){
                    // ポーズメニューを操作できるようにする
                    parent.PlayerController.Add(parent.PauseMenu);
                    new_state = new PlayingState(root_objects,  parent);
                }
                return 0;
            }
        }

        // プレイ中(操作可能)の状態
        class PlayingState : GameState<Stage>
        {
            void EnableOperate(Stage parent)
            {
                // プレイヤーを操作可能に
                parent.PlayerController.Add(parent.Player);
            }

            void DisableOperate(Stage parent)
            {
                parent.PlayerController.Remove(parent.Player);
            }

            public PlayingState(GameRootObjects root_objects, Stage parent)
            {
                EnableOperate(parent);
            }

            public int Update(GameRootObjects root_objects, Stage parent, ref GameState<Stage> new_state)
            {
                parent.Counter.UpdateTime();
                parent.StageState.Time = parent.Counter.GetSeconds();
                if (parent.PauseMenu.Showing)
                {
                    DisableOperate(parent);
                    new_state = new PausingState(root_objects, parent);
                }
                else if (parent.Player.Parameter.HP <= 0)
                {
                    DisableOperate(parent);
                    new_state = new MissedState(root_objects, parent);
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
            int time = 0;
            readonly int RequiredTime = 30;

            void InitFader(GameRootObjects root_objects, Stage parent)
            {
                parent.Fader.Effect = Object.Fader.Flag.FADE_OUT;
                parent.Fader.FadingTime = RequiredTime;
                parent.Fader.Color = new SlimDX.Color4(0.0f, 0.0f, 0.0f, 0.0f);
                root_objects.UpdateList.Add(parent.Fader);
                root_objects.Layers[3].Add(parent.Fader);
            }

            void RemoveFader(GameRootObjects root_objects, Stage parent)
            {
                root_objects.UpdateList.Remove(parent.Fader);
                root_objects.Layers[3].Remove(parent.Fader);
            }

            void StopObjects(GameRootObjects root_objects, Stage parent)
            {
                root_objects.UpdateList.Remove(parent.Player);
                parent.PlayerController.Remove(parent.PauseMenu);
                root_objects.UpdateList.Remove(parent.Camera);
                root_objects.UpdateList.Remove(parent.CollisionManager);
            }

            void ReStartObjects(GameRootObjects root_objects, Stage parent)
            {
                root_objects.UpdateList.Add(parent.Player);
                parent.PlayerController.Add(parent.PauseMenu);
                root_objects.UpdateList.Add(parent.Camera);
                root_objects.UpdateList.Add(parent.CollisionManager);
            }

            public MissedState(GameRootObjects root_objects, Stage parent)
            {
                var life = --parent.Player.Life;
                parent.Player.Life = life;
                InitFader(root_objects, parent);
                StopObjects(root_objects, parent);
            }

            public int Update(GameRootObjects root_objects, Stage parent, ref GameState<Stage> new_state)
            {
                if (parent.Fader.Color.Alpha >= 0.9f)
                {
                    RemoveFader(root_objects, parent);
                    if (parent.Player.Life > 0)
                    {
                        ReStartObjects(root_objects, parent);
                        new_state = new ArrangeState();
                    }
                    else
                    {
                        new_state = new GameOverState(root_objects, parent);
                    }
                }
                return 0;
            }
        }

        // ポーズ状態
        class PausingState : GameState<Stage>
        {
            void PausePlayer(GameRootObjects root_objects, Stage parent)
            {
                parent.PlayerController.Remove(parent.Player);
                root_objects.UpdateList.Remove(parent.Player);
            }

            void EnablePlayer(GameRootObjects root_objects, Stage parent)
            {
                root_objects.UpdateList.Add(parent.Player);
            }

            public PausingState(GameRootObjects root_objects, Stage parent)
            {
                PausePlayer(root_objects, parent);
            }

            public int Update(GameRootObjects root_objects, Stage parent, ref GameState<Stage> new_state)
            {
                if (parent.PauseMenu.Fixed)
                {
                    EnablePlayer(root_objects, parent);
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
                            new_state = new PlayingState(root_objects, parent);
                            break;
                    }
                }
                return 0;
            }
        }

        class GameOverState : GameState<Stage>
        {
            MenuCreator.MenuDirector menuDirector = new MenuCreator.MenuDirector();
            Utility.Menu gameOverMenu = new Utility.Menu();

            public GameOverState(GameRootObjects root_objects, Stage parent)
            {
                menuDirector.Controller = parent.PlayerController;
                menuDirector.RootObjects = root_objects;
                menuDirector.Create(new MenuCreator.GameOverMenuBuilder(gameOverMenu));
                gameOverMenu.IsActive = true;
                gameOverMenu.ChildMenus.Add(null);
                gameOverMenu.ChildMenus.Add(null);
                gameOverMenu.ChildMenus.Add(null);
                gameOverMenu.Show();
                parent.PlayerController.Add(gameOverMenu);
            }

            public int Update(GameRootObjects root_objects, Stage parent, ref GameState<Stage> new_state)
            {
                if (gameOverMenu.Fixed)
                {
                    parent.PlayerController.Remove(gameOverMenu);
                    switch (gameOverMenu.Cursor.Index)
                    {
                        case 0:
                            parent.Player.Life = 1;
                            new_state = new ArrangeState();
                            break;
                        case 1:
                            parent.ReturnTo = ReturnFrag.ToTitle;
                            new_state = new FadeOutState(root_objects, parent);
                            break;
                        case 2:
                            parent.ReturnTo = ReturnFrag.ExitGame;
                            new_state = new FadeOutState(root_objects, parent);
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
