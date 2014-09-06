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
        public int BGMVolume { get; set; }
        /// <summary>
        /// SEの音量
        /// </summary>
        public int SEVolume { get; set; }
        /// <summary>
        /// キー対応表
        /// </summary>
        public Dictionary<string, Key> ButtonKeys { get; set; }
    }
}