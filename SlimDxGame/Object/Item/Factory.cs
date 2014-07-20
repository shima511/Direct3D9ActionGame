using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object.Item
{
    class Factory
    {
        public AssetContainer<Asset.Model> ModelContainer { private get; set; }
        public Collision.Manager CollisionManager { private get; set; }
        public List<Component.IUpdateObject> UpdateList { private get; set; }
        public List<List<Component.IDrawableObject>> Layers { private get; set; }
        public Status.Stage StageStatus { private get; set; }

        public void Create(BinaryParser.Property.Item obj, out IBase new_item)
        {
            new_item = null;
            switch (obj.TypeId)
            {
                case 0:
                    new_item = new Item.Coin()
                    {
                        StageState = this.StageStatus,
                        ModelAsset = ModelContainer.GetValue("TestModel")
                    };
                    break;
                case 1:
                    new_item = new Item.Portion();
                    break;
            }
            new_item.Position2D = obj.Position;
            new_item.OnHit += new_item_OnHit;
        }

        void new_item_OnHit(IBase obj)
        {
            CollisionManager.Remove(obj);
            UpdateList.Remove(obj);
            Layers[0].Remove(obj);
        }
    }
}
