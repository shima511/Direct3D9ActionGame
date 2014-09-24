using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDxGame.Utility;

namespace SlimDxGame.Object.Decolation
{
    class Factory
    {
        public AssetContainer<Asset.Model> ModelContainer { private get; set; }

        public void Create(StageRW.Property.Decolation obj, out IFieldObject new_item)
        {
            new_item = null;
            switch (obj.TypeId)
            {
                case 0:
                    new_item = new Field()
                    {
                        Position = obj.Position,
                        ModelAsset = ModelContainer.GetValue("Field0")
                    };
                    break;
            }
        }
    }
}
