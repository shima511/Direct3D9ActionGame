using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.Object.ExProperty
{
    public class Player : Object.Model
    {
        public StageRW.Property.Player PlayerInfo { get; set; }

        public override void Update()
        {
            _position.X = PlayerInfo.Position.X;
            _position.Y = PlayerInfo.Position.Y;
            if (Selected)
            {
                ModelAsset.Materials[0] = new SlimDX.Direct3D9.ExtendedMaterial()
                {
                    MaterialD3D = new SlimDX.Direct3D9.Material()
                    {
                        Emissive = new SlimDX.Color4(System.Drawing.Color.Silver),
                        Diffuse = new SlimDX.Color4(System.Drawing.Color.Silver)
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
