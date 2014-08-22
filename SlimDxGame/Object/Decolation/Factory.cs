using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object.Decolation
{
    class Factory
    {
        public AssetContainer<Asset.Model> ModelContainer { private get; set; }

        public void Create(StageRW.Property.Decolation obj, out Object.Base.Model new_item)
        {
            new_item = null;
            switch (obj.TypeId)
            {
                case 0:
                    new_item = new Base.Model()
                    {
                        Position = obj.Position,
                        ModelAsset = ModelContainer.GetValue("Field0")
                    };
                    break;
            }
        }
    }
}
