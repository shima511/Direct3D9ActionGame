using SlimDX.DirectInput;
using System.Collections.Generic;

namespace SlimDxGame
{
    /// <summary>
    /// ゲームの設定値
    /// </summary>
    public struct Config
    {
        /// <summary>
        /// BGMの音量
        /// </summary>
        public float BGMVolume { get; set; }
        /// <summary>
        /// SEの音量
        /// </summary>
        public float SEVolume { get; set; }
        /// <summary>
        /// キー対応表
        /// </summary>
        public Dictionary<string, Key> ButtonKeys { get; set; }
    }
}