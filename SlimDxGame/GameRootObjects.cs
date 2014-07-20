using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame
{
    class GameRootObjects
    {
        public List<Component.IUpdateObject> UpdateList { get; set; }
        public List<List<Component.IDrawableObject>> Layers { get; set; }
        public InputManager InputManager { get; set; }
        public AssetContainer<Asset.Font> FontContainer { get; set; }
        public AssetContainer<Asset.Texture> TextureContainer { get; set; }
        public AssetContainer<Asset.Sound> SoundContainer { get; set; }
        public AssetContainer<Asset.Model> ModelContainer { get; set; }

        public GameRootObjects()
        {
            UpdateList = new List<Component.IUpdateObject>();
            Layers = new List<List<Component.IDrawableObject>>();
            InputManager = new InputManager();
            FontContainer = new AssetContainer<Asset.Font>();
            TextureContainer = new AssetContainer<Asset.Texture>();
            SoundContainer = new AssetContainer<Asset.Sound>();
            ModelContainer = new AssetContainer<Asset.Model>();
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
