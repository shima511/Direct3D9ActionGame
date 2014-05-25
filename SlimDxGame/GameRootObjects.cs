using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame
{
    class GameRootObjects
    {
        public List<Component.IUpdateObject> update_list = new List<Component.IUpdateObject>();
        public List<List<Component.IDrawableObject>> layers = new List<List<Component.IDrawableObject>>();
        public InputManager input_manager = new InputManager();
        public AssetContainer<Asset.Font> font_container = new AssetContainer<Asset.Font>();
        public AssetContainer<Asset.Texture> tex_container = new AssetContainer<Asset.Texture>();
        public AssetContainer<Asset.Sound> sound_container = new AssetContainer<Asset.Sound>();
        public AssetContainer<Asset.Model> model_container = new AssetContainer<Asset.Model>();

        /// <summary>
        ///  アセットに不正なデータが存在していた場合trueを返します
        ///  また、不正なデータの名前を取得します。
        /// </summary>
        /// <returns></returns>
        public bool IncludeInvalidAsset(ref List<string> object_names)
        {
            return font_container.IncludeInvalidObject(ref object_names) | tex_container.IncludeInvalidObject(ref object_names) | sound_container.IncludeInvalidObject(ref object_names) | model_container.IncludeInvalidObject(ref object_names);
        }
    }
}
