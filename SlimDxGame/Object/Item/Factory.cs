using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDxGame.Utility;

namespace SlimDxGame.Object.Item
{
    class Factory
    {
        public AssetContainer<Asset.Sound> SoundContainer { private get; set; }
        public AssetContainer<Asset.Model> ModelContainer { private get; set; }
        public Status.Stage StageStatus { private get; set; }

        public void Create(StageRW.Property.Item obj, out IBase new_item)
        {
            new_item = null;
            switch (obj.TypeId)
            {
                case 0:
                    new_item = new Item.Coin(obj.Position)
                    {
                        StageState = this.StageStatus,
                        ModelAsset = ModelContainer.GetValue("Coin")
                    };
                    break;
                case 1:
                    new_item = new Item.Portion();
                    break;
            }
            new_item.IsVisible = true;
            new_item.IsActive = true;
            new_item.Spawnable = true;
            new_item.OnHit += new_item_OnHit;
        }

        void new_item_OnHit(IBase obj)
        {
            obj.IsVisible = false;
            obj.IsActive = false;
            obj.Spawnable = false;
        }
    }
}
