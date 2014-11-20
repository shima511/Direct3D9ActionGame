using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame
{
    class DescriptionScreen : Component.IDrawableObject, Component.IUpdateObject
    {
        int time = 0;
        /// <summary>
        /// 
        /// </summary>
        readonly int AffordTime = 30;
        /// <summary>
        /// 必要な時間
        /// </summary>
        readonly int RequiredTime = 120;
        /// <summary>
        /// カウントダウンを行う回数
        /// </summary>
        readonly int CountTimes = 3;
        /// <summary>
        /// 現在のカウント回数
        /// </summary>
        int currentCounts = 0;
        /// <summary>
        /// カウントが終わった場合、trueを返します。
        /// </summary>
        public bool CountEnd { get; private set; }
        public Asset.Font Font { get; set; }
        Object.Base.String countDownLabel = new Object.Base.String();
        /// <summary>
        /// カウントするときに実行されるイベント
        /// </summary>
        public event Action OnCount;
        /// <summary>
        /// カウントダウンが終了した時に実行されるイベント
        /// </summary>
        public event Action OnCountEnd;

        public DescriptionScreen()
        {
            CountEnd = false;
            countDownLabel.Text = CountTimes.ToString();
            countDownLabel.Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 9 / 20, Core.Game.AppInfo.Height / 2);
        }

        public bool IsVisible
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {
        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {
            if (countDownLabel.IsVisible)
            {
                countDownLabel.Font = Font;
                countDownLabel.Draw2D(dev);
            }
        }

        public bool IsActive
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public void Update()
        {
            time++;
            if (time >= RequiredTime + AffordTime)
            {
                CountEnd = true;
            }
            else if (time == RequiredTime)
            {
                OnCountEnd();
                countDownLabel.Text = "GO!";
            }
            else if (time % (RequiredTime / CountTimes) == 0)
            {
                OnCount();
                currentCounts++;
                countDownLabel.Text = (CountTimes - currentCounts).ToString();
            }
        }
    }
}
