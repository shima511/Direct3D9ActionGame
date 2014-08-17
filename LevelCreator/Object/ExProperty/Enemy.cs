using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator.Object.ExProperty
{
    public class Enemy : Object.Model
    {
        public StageRW.Property.Enemy EnemyInfo { get; set; }

        public override void Update()
        {
            _position.X = EnemyInfo.Position.X;
            _position.Y = EnemyInfo.Position.Y;
            if (Selected)
            {
                for (int i = 0; i < ModelAsset.Materials.Count; i++)
                {
                    ModelAsset.Materials[i] = new SlimDX.Direct3D9.ExtendedMaterial()
                    {
                        MaterialD3D = new SlimDX.Direct3D9.Material()
                        {
                            Diffuse = System.Drawing.Color.Yellow,
                            Emissive = System.Drawing.Color.Yellow
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
