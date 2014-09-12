using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDxGame.Utility;

namespace SlimDxGame
{
    class GameRootObjects
    {
        /// <summary>
        /// 更新リスト
        /// Component.IUpdateObjectインターフェースを継承したオブジェクトを格納する
        /// </summary>
        public List<Component.IUpdateObject> UpdateList { get; set; }
        /// <summary>
        /// レイヤー
        /// Component.IDrawableObjectインターフェースを継承したオブジェクトを格納する
        /// </summary>
        public List<List<Component.IDrawableObject>> Layers { get; set; }
        /// <summary>
        /// 入力管理クラス
        /// コントローラーの追加などを行う
        /// </summary>
        public InputManager InputManager { get; set; }
        /// <summary>
        /// フォントコンテナ
        /// </summary>
        public AssetContainer<Asset.Font> FontContainer { get; set; }
        /// <summary>
        /// テクスチャコンテナ
        /// </summary>
        public AssetContainer<Asset.Texture> TextureContainer { get; set; }
        /// <summary>
        /// サウンドコンテナ
        /// </summary>
        public AssetContainer<Asset.Sound> SoundContainer { get; set; }
        /// <summary>
        /// モデルコンテナ
        /// </summary>
        public AssetContainer<Asset.Model> ModelContainer { get; set; }
        /// <summary>
        /// ゲームの設定
        /// </summary>
        public Config Settings { get; set; }

        public GameRootObjects()
        {
            UpdateList = new List<Component.IUpdateObject>();
            Layers = new List<List<Component.IDrawableObject>>();
            InputManager = new InputManager();
            FontContainer = new AssetContainer<Asset.Font>();
            TextureContainer = new AssetContainer<Asset.Texture>();
            SoundContainer = new AssetContainer<Asset.Sound>();
            ModelContainer = new AssetContainer<Asset.Model>();
            Settings = new Config();
        }

        /// <summary>
        ///  アセットに不正なデータが存在していた場合trueを返します
        ///  また、不正なデータの名前を取得します。
        /// </summary>
        /// <returns></returns>
        public bool IncludeInvalidAsset(ref List<string> object_names)
        {
            return FontContainer.IncludeInvalidObject(ref object_names) | TextureContainer.IncludeInvalidObject(ref object_names) | SoundContainer.IncludeInvalidObject(ref object_names) | ModelContainer.IncludeInvalidObject(ref object_names);
        }
    }
}
