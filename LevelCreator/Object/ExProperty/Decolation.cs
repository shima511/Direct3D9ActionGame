using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.Object.ExProperty
{
    public class Decolation : Object.Model
    {
        public BinaryParser.Property.Decolation DecolationInfo { get; set; }

        public override void Update()
        {
            Position = DecolationInfo.Position;
            if (Selected)
            {
                this.ModelAsset.Materials[0] = new SlimDX.Direct3D9.ExtendedMaterial()
                {
                    MaterialD3D = new SlimDX.Direct3D9.Material()
                    {
                        Diffuse = new SlimDX.Color4(System.Drawing.Color.Green),
                        Emissive = new SlimDX.Color4(System.Drawing.Color.Green)
                    }
                };
            }
            else
            {
                ModelAsset.Materials = new List<SlimDX.Direct3D9.ExtendedMaterial>(ModelAsset.LoadedMaterials);
            }
        }
    }
}
