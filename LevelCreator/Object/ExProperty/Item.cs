using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.Object.ExProperty
{
    public class Item : Object.Model
    {
        public StageRW.Property.Item ItemInfo { get; set; }

        public override void Update()
        {
            _position.X = ItemInfo.Position.X;
            _position.Y = ItemInfo.Position.Y;
            if (Selected)
            {
                for (int i = 0; i < ModelAsset.Materials.Count; i++)
                {
                    ModelAsset.Materials[i] = new SlimDX.Direct3D9.ExtendedMaterial() 
                    {
                        MaterialD3D = new SlimDX.Direct3D9.Material()
                        {
                            Diffuse = System.Drawing.Color.HotPink,
                            Emissive = System.Drawing.Color.HotPink
                        }
                    };
                }
            }
            else
            {
                ModelAsset.Materials = new List<SlimDX.Direct3D9.ExtendedMaterial>(ModelAsset.LoadedMaterials);
            }
        }
    }
}
