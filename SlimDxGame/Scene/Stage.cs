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
        StageLoader stg_loader;
        ReturnFrag ret_frag;
        Collision.Manager collision_manager;
        List<Object.Base.Model> model_decoration;
        Object.CameraManager camera_manager;
        Object.Camera camera;
        Object.Player player;
        Controller controller;
        Object.Fader fader;
        Object.StateDrawer state_drawer;
        GameState<Stage> now_state = new LoadingState();

        // ステージの読み込みなどを行う
        private class LoadingState : GameState<Stage>
        {
            private bool thread_created = false;

            private bool load_completed = false;

            private void InitLayer( GameRootObjects root_objects)
            {
                for (int i = 0; i < 3; i++)
                {
                    root_objects.layers.Add(new List<Component.IDrawableObject>());
                }
            }

            private void LoadTextures( AssetContainer<Asset.Texture> tex_container)
            {
                Asset.Texture new_tex;

                //// 読み込み
                //string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                //new_tex = AssetFactory.TextureFactory.CreateTextureFromFile(Path.Combine(baseDir, Path.Combine("toons", "pony_fluttershy.bmp")));
                //tex_container.Add("test", new_tex);

                // フェーダー用のテクスチャを生成
                byte[] tex_info = { 255, 255, 255, 255 };
                new_tex = AssetFactory.TextureFactory.CreateFromMemory(1, 1, tex_info);
                tex_container.Add("BlackTexture", new_tex);
            }

            private void LoadSounds( AssetContainer<Asset.Sound> sound_container)
            {
                //Asset.Sound new_sound;
                //string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                //new_sound = AssetFactory.AudioMediaFactory.CreateSoundFromFile(Path.Combine(baseDir, Path.Combine("sounds", "MusicMono.wav")));
                //sound_container.Add("test_sound", new_sound);
            }

            private void LoadModels( AssetContainer<Asset.Model> model_container)
            {
                Asset.Model new_model;
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                new_model = AssetFactory.ModelFactory.CreateModelFromFile(Path.Combine(baseDir, Path.Combine("models", "test.x")));
                model_container.Add("TestModel", new_model);

                new_model = AssetFactory.ModelFactory.CreateModelFromFile(Path.Combine(baseDir, Path.Combine("models", "test_obj.x")));
                model_container.Add("StageObject", new_model);

                new_model = AssetFactory.ModelFactory.CreateModelFromFile(Path.Combine(baseDir, Path.Combine("models", "Floor.x")));
                model_container.Add("NormalFloor", new_model);
            }

            void LoadStage(GameRootObjects root, Stage parent)
            {
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                parent.stg_loader.Load(Path.Combine(baseDir, Path.Combine("levels", "level" + parent.level_id.ToString() + ".csv")));
                parent.StageState = new Status.Stage()
                {
                    Score = 0,
                    Time = 100
                };
            }

            void LoadFont(GameRootObjects root)
            {
                var font = AssetFactory.FontFactory.CreateFont(new System.Drawing.Font("Arial", 20));
                root.font_container.Add("Arial", font);
            }

            private void CreateInstance(Stage parent)
            {
                parent.stg_loader = new StageLoader();
                parent.collision_manager = new Collision.Manager();
                parent.model_decoration = new List<Object.Base.Model>();
                parent.camera_manager = new Object.CameraManager();
                parent.camera = new Object.Camera();
                parent.player = new Object.Player();
                parent.controller = new Controller();
                parent.fader = new Object.Fader();
            }

            public int Update( GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                if (!thread_created)
                {
                    CreateInstance(parent);

                    InitLayer( root_objects);

                    thread_created = true;
                }

                LoadTextures( root_objects.tex_container);
                LoadModels( root_objects.model_container);
                LoadStage(root_objects, parent);
                LoadFont(root_objects);

                load_completed = true;

                if (load_completed)
                {
                    List<string> invalid_calls = new List<string>();
                    if (root_objects.IncludeInvalidAsset(ref invalid_calls) || parent.stg_loader.LoadFailed)
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
                root_objects.layers[2].Add(parent.camera);
                parent.controller.Add(parent.camera);
                parent.camera_manager.Camera = parent.camera;
                parent.camera_manager.Player = parent.player;
                root_objects.update_list.Add(parent.camera_manager);
            }

            private void InitInputManager( GameRootObjects root_objects,  Stage parent)
            {
                root_objects.input_manager.Add(parent.controller);
            }

            private void InitPlayer( GameRootObjects root_objects,  Object.Player player){
                Asset.Model model;
                root_objects.model_container.TryGetValue("TestModel", out model);
                player.ModelAsset = model;
                player.Position = new SlimDX.Vector3(0.0f, 0.0f, 0.0f);
                root_objects.update_list.Add(player);
                root_objects.layers[0].Add(player);
            }

            private void InitDecoration(GameRootObjects root_objects, Stage parent)
            {
                Asset.Model decoration_model;
                root_objects.model_container.TryGetValue("StageObject", out decoration_model);
                Random rand = new Random();
                for(int i = 0; i < 10; i++){
                    var new_obj = new Object.Base.Model();
                    new_obj.Position = new SlimDX.Vector3(rand.Next(12) - 6, rand.Next(12) - 6, rand.Next(12) - 6);
                    new_obj.ModelAsset = decoration_model;
                    parent.model_decoration.Add(new_obj);
                    root_objects.layers[0].Add(new_obj);
                }
            }

            private void InitCollisionObjects(GameRootObjects root_objects, Stage parent)
            {
                parent.collision_manager.Player = parent.player;

                // 地形の衝突判定を追加していく
                foreach (var item in parent.stg_loader.GroundCollisions)
                {
                    parent.collision_manager.Add(item);
                }

                root_objects.update_list.Add(parent.collision_manager);
#if DEBUG
                root_objects.layers[0].Add(parent.collision_manager);
#endif
            }

            void InitStateDrawer(GameRootObjects root_objects, Stage parent)
            {
                parent.state_drawer = new Object.StateDrawer(parent.StageState, parent.player.State);
                Asset.Font font;
                root_objects.font_container.TryGetValue("Arial", out font);
                parent.state_drawer.Font = font;
                root_objects.update_list.Add(parent.state_drawer);
                root_objects.layers[0].Add(parent.state_drawer);
            }

            public int Update(GameRootObjects root_objects,  Stage parent, ref GameState<Stage> new_state)
            {
                InitCamera( root_objects,  parent);

                InitInputManager( root_objects,  parent);

                InitPlayer( root_objects,  parent.player);

                InitDecoration(root_objects, parent);

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
                root_objects.tex_container.TryGetValue("BlackTexture", out tex);
                parent.fader.Texture = tex;
                parent.fader.Scale = new SlimDX.Vector2(Core.Game.AppInfo.Width * 2, Core.Game.AppInfo.Height * 2);
                parent.fader.FadingTime = 120;
                parent.fader.Color = new SlimDX.Color4(1.0f, 0.0f, 0.0f, 0.0f);
                parent.fader.Effect = Object.Fader.Flag.FADE_IN;
                root_objects.layers[2].Add(parent.fader);
                root_objects.update_list.Add(parent.fader);
            }

            private void RemoveFadeInEffect( GameRootObjects root_objects,  Stage parentects)
            {
                root_objects.layers[2].Remove(parentects.fader);
                root_objects.update_list.Remove(parentects.fader);
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
