using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SlimDxGame.Scene
{
    class Stage : Scene.Base
    {
        private enum ReturnFrag{
            ExitGame,
            ToTitle
        }
        /// <summary>
        /// ステージ番号
        /// </summary>
        int level_id = 0;
        Status.Stage StageState;
        StageRW.Reader stage_loader;
        ReturnFrag ret_frag;
        Collision.Manager collision_manager;
        Object.CameraManager camera_manager;
        Object.Camera camera;
        Object.Player player;
        Controller controller;
        Object.Fader fader;
        Object.StateDrawer state_drawer;
        Object.Item.Factory item_factory;
        GameState<Stage> now_state = new LoadingState();
        StageRW.Objects stage_objects = new StageRW.Objects();
        List<Effect.Light> lights = new List<Effect.Light>();
        Object.Shadow shadow = new Object.Shadow();

        // ステージの読み込みなどを行う
        private class LoadingState : GameState<Stage>
        {
            private bool thread_created = false;

            private bool load_completed = false;

            private void InitLayer( GameRootObjects root_objects)
            {
                root_objects.Layers.Capacity = 4;
                for (int i = 0; i < root_objects.Layers.Capacity; i++)
                {
                    root_objects.Layers.Add(new List<Component.IDrawableObject>());
                }
            }

            private void LoadTextures( AssetContainer<Asset.Texture> tex_container)
            {
                Asset.Texture new_tex;

                //// 読み込み
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                new_tex = AssetFactory.TextureFactory.CreateTextureFromFile(Path.Combine(baseDir, Path.Combine("toons", "shadow.png")));
                tex_container.Add("Shadow", new_tex);

                // フェーダー用のテクスチャを生成
                byte[] tex_info = { 255, 255, 255, 255 };
                new_tex = AssetFactory.TextureFactory.CreateFromRawData(1, 1, tex_info);
                tex_container.Add("BlackTexture", new_tex);
            }

            private void LoadSounds( AssetContainer<Asset.Sound> sound_container)
            {
                //Asset.Sound new_sound;
                //string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                //new_sound = AssetFactory.AudioMediaFactory.CreateSoundFromFile(Path.Combine(baseDir, Path.Combine("sounds", "MusicMono.wav")));
                //sound_container.Add("test_sound", new_sound);
            }

            private void LoadModels(AssetContainer<Asset.Model> model_container)
            {
                Asset.Model new_model;
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                new_model = AssetFactory.ModelFactory.CreateModelFromFile(Path.Combine(baseDir, Path.Combine("models", "test.x")));
                model_container.Add("TestModel", new_model);

                new_model = AssetFactory.ModelFactory.CreateModelFromFile(Path.Combine(baseDir, Path.Combine("models", "test_obj.x")));
                model_container.Add("StageObject", new_model);

                new_model = AssetFactory.ModelFactory.CreateModelFromFile(Path.Combine(baseDir, Path.Combine("models", "Floor.x")));
                model_container.Add("NormalFloor", new_model);

                new_model = AssetFactory.ModelFactory.CreateModelFromFile(Path.Combine(baseDir, Path.Combine("models", "coins.x")));
                model_container.Add("Coins", new_model);

                ScriptRW.Reader reader = new ScriptRW.Reader();
                ScriptRW.Properties properties;
                reader.Read(out properties, "obj_list.txt");
                foreach (var item in properties.Decolations)
                {
                    new_model = AssetFactory.ModelFactory.CreateModelFromFile(Path.Combine(baseDir, item.AssetPath));
                    model_container.Add(item.Name, new_model);
                }
            }

            void LoadStage(GameRootObjects root, Stage parent)
            {
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                parent.stage_loader.Read(Path.Combine(baseDir, Path.Combine("levels", "stage" + parent.level_id.ToString() + ".dat")), out parent.stage_objects);
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

            private void CreateInstance(Stage parent)
            {
                parent.stage_loader = new StageRW.Reader();
                parent.collision_manager = new Collision.Manager();
                parent.camera_manager = new Object.CameraManager();
                parent.camera = new Object.Camera();
                parent.player = new Object.Player();
                parent.controller = new Controller();
                parent.fader = new Object.Fader();
                parent.item_factory = new Object.Item.Factory();
            }

            public int Update(GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                if (!thread_created)
                {
                    CreateInstance(parent);

                    InitLayer( root_objects);

                    thread_created = true;
                }

                LoadTextures(root_objects.TextureContainer);
                LoadModels(root_objects.ModelContainer);
                LoadStage(root_objects, parent);
                LoadFont(root_objects);

                load_completed = true;

                if (load_completed)
                {
                    List<string> invalid_calls = new List<string>();
                    if (root_objects.IncludeInvalidAsset(ref invalid_calls) || !parent.stage_loader.Valid)
                    {
                        MessageBox.Show("ファイルの読み込みに失敗", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        parent.ret_frag = ReturnFrag.ExitGame;
                        return -1;
                    }
                    new_state = new InitState();
                }

                return 0;
            }
        }

        // ステージの配置など初期設定を行う
        private class InitState : GameState<Stage>
        {
            private void InitCamera(GameRootObjects root_objects,  Stage parent)
            {
                root_objects.Layers[3].Add(parent.camera);
                parent.controller.Add(parent.camera);
                parent.camera_manager.Camera = parent.camera;
                parent.camera_manager.Player = parent.player;
                root_objects.UpdateList.Add(parent.camera_manager);
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

                parent.lights.Add(right_light);

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

                parent.lights.Add(left_light);

                foreach (var item in parent.lights)
                {
                    root_objects.Layers[0].Add(item);
                }
            }

            private void InitInputManager( GameRootObjects root_objects,  Stage parent)
            {
                root_objects.InputManager.Add(parent.controller);
            }

            private void InitPlayer(GameRootObjects root_objects, Stage parent)
            {
                Asset.Model model;
                root_objects.ModelContainer.TryGetValue("TestModel", out model);
                parent.player.ModelAsset = model;
                var p_pos = parent.stage_objects.Player.Position;
                parent.player.Position = new SlimDX.Vector3(p_pos.X, p_pos.Y, 0);
                root_objects.UpdateList.Add(parent.player);
                root_objects.Layers[2].Add(parent.player);

                // 影をつける
                Asset.Texture shadow_tex;
                root_objects.TextureContainer.TryGetValue("Shadow", out shadow_tex);
                Vertex vertex;
                PolygonFactory.CreateSquarePolygon(out vertex);
                parent.shadow = new Object.Shadow()
                {
                    Texture = shadow_tex,
                    Position = new SlimDX.Vector3(),
                    Scale = new SlimDX.Vector2(3.0f, 3.0f),
                    Rotation = new SlimDX.Vector3(),
                    Vertex = vertex,
                    Owner = parent.player,
                    Line = new Collision.Shape.Line()
                    {
                        StartingPoint = new SlimDX.Vector2(-6.0f, 0.0f),
                        TerminalPoint = new SlimDX.Vector2(6.0f, 6.0f)
                    }
                };
                root_objects.UpdateList.Add(parent.shadow);
                root_objects.Layers[2].Add(parent.shadow);
            }

            private void InitDecoration(GameRootObjects root_objects, Stage parent)
            {
                Object.Decolation.Factory factory = new Object.Decolation.Factory();
                factory.ModelContainer = root_objects.ModelContainer;
                foreach (var item in parent.stage_objects.Decolations)
                {
                    Object.Base.Model new_item;
                    factory.Create(item, out new_item);
                    root_objects.Layers[1].Add(new_item);
                }
            }

            [System.Diagnostics.Conditional("DEBUG")]
            void AddCollisionsToDrawList(GameRootObjects root_objects, Stage parent)
            {
                root_objects.Layers[1].Add(parent.collision_manager);
            }

            private void InitCollisionObjects(GameRootObjects root_objects, Stage parent)
            {
                parent.collision_manager.Player = parent.player;

                // 地形の衝突判定を追加していく
                foreach (var item in parent.stage_objects.Collisions)
                {
                    Object.Ground.Base new_collision = Object.Ground.Base.CreateGround(item);
                    parent.collision_manager.Add(new_collision);
                }

                root_objects.UpdateList.Add(parent.collision_manager);
                AddCollisionsToDrawList(root_objects, parent);
            }

            void InitStateDrawer(GameRootObjects root_objects, Stage parent)
            {
                parent.state_drawer = new Object.StateDrawer(parent.StageState, parent.player.State);
                Asset.Font font;
                root_objects.FontContainer.TryGetValue("Arial", out font);
                parent.state_drawer.Font = font;
                root_objects.UpdateList.Add(parent.state_drawer);
                root_objects.Layers[1].Add(parent.state_drawer);
            }

            void InitItem(GameRootObjects root_objects, Stage parent)
            {
                parent.item_factory.ModelContainer = root_objects.ModelContainer;
                parent.item_factory.UpdateList = root_objects.UpdateList;
                parent.item_factory.Layers = root_objects.Layers;
                parent.item_factory.CollisionManager = parent.collision_manager;
                parent.item_factory.StageStatus = parent.StageState;

                Asset.Model new_model;
                root_objects.ModelContainer.TryGetValue("Coins", out new_model);

                Object.Item.IBase new_item = new Object.Item.Coin()
                {
                    StageState = parent.StageState,
                    ModelAsset = new_model
                };
                parent.collision_manager.Add(new_item);
                root_objects.UpdateList.Add(new_item);
                root_objects.Layers[1].Add(new_item);

                /*
                foreach (var item in parent.stage_objects.Items)
                {
                    Object.Item.IBase new_item;
                    parent.item_factory.Create(item, out new_item);
                    parent.collision_manager.Add(new_item);
                    root_objects.UpdateList.Add(new_item);
                    root_objects.Layers[1].Add(new_item);
                }
                 * */
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

                new_state = new FadeInState( root_objects,  parent);
                return 0;
            }
        }

        // フェードイン
        private class FadeInState : GameState<Stage>
        {
            public FadeInState( GameRootObjects root_objects,  Stage parent)
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

            private void RemoveFadeInEffect( GameRootObjects root_objects,  Stage parentects)
            {
                root_objects.Layers[2].Remove(parentects.fader);
                root_objects.UpdateList.Remove(parentects.fader);
            }

            public int Update( GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                if(parent.fader.Color.Alpha <= 0.1f){
                    RemoveFadeInEffect( root_objects,  parent);
                    new_state = new CountDownState();
                }
                return 0;
            }            
        }

        // フェードイン終了後、操作可能になるまでカウントダウンを行う状態
        private class CountDownState : GameState<Stage>
        {
            private int time = 0;

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
        private class PlayingState : GameState<Stage>
        {
            private void EnableOperate(Stage parent)
            {
                // プレイヤーを操作可能に
                parent.controller.Add(parent.player);
            }

            public PlayingState( GameRootObjects root_objects,  Stage parent)
            {
                EnableOperate(parent);
            }

            public int Update( GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                return 0;
            }
        }

        // ステージクリアした状態
        private class ClearedState : GameState<Stage>
        {
            public int Update( GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                return 0;
            }
        }

        // ミスした状態
        private class MissedState : GameState<Stage>
        {
            public int Update( GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                return 0;
            }
        }

        // ポーズ状態
        private class PausingState : GameState<Stage>
        {
            public int Update( GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                return 0;
            }
        }

        // フェードアウト状態
        private class FadeOutState : GameState<Stage>
        {
            public int Update( GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                return -1;
            }
        }

        public override int Update( GameRootObjects root_objects, ref Scene.Base new_scene)
        {
            int ret_val = 0;
            if (now_state.Update(root_objects, this, ref now_state) == -1)
            {
                switch (ret_frag)
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
