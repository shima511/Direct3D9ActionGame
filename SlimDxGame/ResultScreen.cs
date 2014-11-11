using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame
{
    class ResultScreen : Component.IDrawableObject, Component.IUpdateObject, Component.IOperableObject
    {
        int time = 0;
        /// <summary>
        /// フェードの強さ
        /// </summary>
        readonly float FadeStrength = 0.8f;
        /// <summary>
        /// フェードを行う時間
        /// </summary>
        readonly int FadeTime = 60;
        /// <summary>
        /// 残り時間
        /// </summary>
        public uint LeftTime { get; set; }
        int coin_count = 0;
        /// <summary>
        /// コインのスコアへの倍率
        /// </summary>
        readonly int CoinPower = 100;
        int time_count = 0;
        /// <summary>
        /// スコア結果
        /// </summary>
        int result_score = 0;
        /// <summary>
        /// ステージに存在するコインの数
        /// </summary>
        public int MaxCoinNum { get; set; }
        /// <summary>
        /// 集めたコインの数
        /// </summary>
        public uint CollectedCoinNum { get; set; }
        /// <summary>
        /// 使用するフォント
        /// </summary>
        public Asset.Font Font { get; set; }
        /// <summary>
        /// フェード用背景
        /// </summary>
        public Object.Base.Sprite BackGround { get; set; }
        /// <summary>
        /// ラベル
        /// </summary>
        List<Object.Base.String> labels = new List<Object.Base.String>();
        /// <summary>
        /// メニュー用のラベル
        /// </summary>
        List<Object.Base.String> menu_labels = new List<Object.Base.String>();
        Object.Base.String coin_score_str = new Object.Base.String();
        SlimDX.Color4 color = new SlimDX.Color4();
        /// <summary>
        /// カーソル
        /// </summary>
        public Object.Cursor Cursor { get; set; }
        /// <summary>
        /// 選択肢が選ばれた状態
        /// </summary>
        public bool CursorFixed { get; private set; }
        /// <summary>
        /// カウントが終わった時に実行するイベント
        /// </summary>
        public event Action OnCountFinished;
        bool isVisible = true;
        bool isActive = true;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
            }
        }

        public ResultScreen()
        {
            CursorFixed = false;
            Cursor = new Object.Cursor()
            {
                Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 5 / 20 - 30, Core.Game.AppInfo.Height * 4 / 5),
                PositionList = new List<SlimDX.Vector2>()
                {
                    new SlimDX.Vector2(Core.Game.AppInfo.Width * 5 / 20 - 30, Core.Game.AppInfo.Height * 4 / 5),
                    new SlimDX.Vector2(Core.Game.AppInfo.Width * 12 / 20 - 30, Core.Game.AppInfo.Height * 4 / 5)
                }
            };
            Cursor.IsActive = false;
            Cursor.IsVisible = false;
            labels.AddRange(new[] 
            {
                new Object.Base.String(){Text = "CoinScore:", Color = new SlimDX.Color4(1.0f, 1.0f, 1.0f, 1.0f), Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 7 / 20, Core.Game.AppInfo.Height / 5), IsVisible = false},
                new Object.Base.String(){Text = "TimeBonus:", Color = new SlimDX.Color4(1.0f, 1.0f, 1.0f, 1.0f), Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 7 / 20, Core.Game.AppInfo.Height * 2 / 5), IsVisible = false},
                new Object.Base.String(){Text = "TotalScore:", Color = new SlimDX.Color4(1.0f, 1.0f, 1.0f, 1.0f), Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 7 / 20, Core.Game.AppInfo.Height * 3 / 5), IsVisible = false},
                new Object.Base.String(){Text = "0", Color = new SlimDX.Color4(1.0f, 1.0f, 1.0f, 1.0f), Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 11 / 20, Core.Game.AppInfo.Height / 5), IsVisible = false},
                new Object.Base.String(){Text = "0", Color = new SlimDX.Color4(1.0f, 1.0f, 1.0f, 1.0f), Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 11 / 20, Core.Game.AppInfo.Height * 2 / 5), IsVisible = false},
                new Object.Base.String(){Text = "0", Color = new SlimDX.Color4(1.0f, 1.0f, 1.0f, 1.0f), Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 11 / 20, Core.Game.AppInfo.Height * 3 / 5), IsVisible = false},
            });
            menu_labels.AddRange(new[]
            {
                new Object.Base.String(){Text = "リトライ", Color = new SlimDX.Color4(1.0f, 1.0f, 1.0f, 1.0f), Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 5 / 20, Core.Game.AppInfo.Height * 4 / 5), IsVisible = false},
                new Object.Base.String(){Text = "ゲーム終了", Color = new SlimDX.Color4(1.0f, 1.0f, 1.0f, 1.0f), Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 12 / 20, Core.Game.AppInfo.Height * 4 / 5), IsVisible = false}
            });
        }

        void CountCoinScore()
        {
            if (coin_count < CollectedCoinNum)
            {
                coin_count++;
                result_score += CoinPower;
            }
            if (coin_count == CollectedCoinNum - 1)
            {
                OnCountFinished();
            }
            labels[3].Text = (coin_count * CoinPower).ToString();
        }

        void CountTimeBonus()
        {
            if(coin_count == CollectedCoinNum && time_count != LeftTime)
            {
                time_count++;
                labels[4].Text = (time_count * 10).ToString();
            }
            if (time_count == LeftTime - 1)
            {
                OnCountFinished();
                labels[5].Text = (LeftTime * 10 + coin_count * CoinPower).ToString();
            }
        }

        public void Update()
        {
            time++;
            if (FadeStrength > color.Alpha)
            {
                // フェードを行う
                color.Alpha += FadeStrength / FadeTime;
            }
            else
            {
                CountCoinScore();
                CountTimeBonus();
                if (CountEnd()) {
                    foreach (var item in menu_labels)
                    {
                        item.IsVisible = true;
                    }
                    Cursor.IsVisible = true;
                    Cursor.IsActive = true;
                    Cursor.Update();
                }
                foreach (var item in labels)
                {
                    item.IsVisible = true;
                }
            }
            BackGround.Color = color;
        }

        bool CountEnd()
        {
            return coin_count == CollectedCoinNum && time_count == LeftTime;
        }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {
            BackGround.Draw2D(dev);
            foreach (var item in labels)
            {
                if (item.IsVisible)
                {
                    item.Font = Font;
                    item.Draw2D(dev);
                }
            }
            foreach (var item in menu_labels)
            {
                if (item.IsVisible)
                {
                    item.Font = Font;
                    item.Draw2D(dev);
                }
            }
            if (Cursor.IsVisible) Cursor.Draw2D(dev);
        }

        public void ControllerAction(Controller controller)
        {
            if (!CountEnd())
            {
                if (controller.AButton.IsPressed() && coin_count != CollectedCoinNum) { coin_count = (int)CollectedCoinNum - 2; }
                if (controller.AButton.IsPressed() && time_count != LeftTime) { time_count = (int)LeftTime - 2; }
            }
            if (controller.AButton.IsPressed() && CountEnd()) { CursorFixed = true; }
            if (Cursor.IsActive)
            {
                Cursor.ControllerAction(controller);
            }
        }
    }
}
