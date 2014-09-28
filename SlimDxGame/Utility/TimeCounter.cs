using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Utility
{
    /// <summary>
    /// FPSに応じた時間をカウントするクラス
    /// </summary>
    class TimeCounter
    {
        public readonly uint FPS;
        uint time = 0;

        public TimeCounter(uint fps)
        {
            FPS = fps;
        }

        /// <summary>
        /// 時間を更新します。
        /// </summary>
        public void UpdateTime()
        {
            time++;
        }

        /// <summary>
        /// カウントしてからの秒数を返します。
        /// </summary>
        /// <returns>秒数</returns>
        public uint GetSeconds()
        {
            return time / FPS;
        }

        /// <summary>
        /// カウントしてからの分数を返します。
        /// </summary>
        /// <returns></returns>
        public uint GetMinutes()
        {
            return time / (FPS * 60);
        }

        /// <summary>
        /// タイマーをリセットします。
        /// </summary>
        public void ResetTime()
        {
            time = 0;
        }
    }
}
