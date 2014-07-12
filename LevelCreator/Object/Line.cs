using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;

namespace LevelCreator.Object
{
    class Line : Model
    {
        public Vector2 StartPoint { get; set; }
        public Vector2 TerminatePoint { get; set; }
        public override void Update()
        {
            var center_x = (StartPoint.X + TerminatePoint.X) / 2;
            var center_y = (StartPoint.Y + TerminatePoint.Y) / 2;
            _position.X = center_x;
            _position.Y = center_y;
            _scale.Y = 0.1f;
            double diff_x = TerminatePoint.X - StartPoint.X;
            double diff_y = TerminatePoint.Y - StartPoint.Y;
            _scale.X = (float)Math.Sqrt( Math.Pow(diff_x, 2.0) + Math.Pow(diff_y, 2.0) );
            _rotation.Z = (float)Math.Atan2(diff_y, diff_x);
            if (Selected)
            {
                var mat_list = ModelAsset.Materials;
                mat_list[0] = new SlimDX.Direct3D9.ExtendedMaterial()
                {
                    MaterialD3D = new SlimDX.Direct3D9.Material()
                    {
                        Diffuse = new Color4(System.Drawing.Color.Red),
                        Emissive = new Color4(System.Drawing.Color.Red)
                    }                    
                };
                ModelAsset.Materials = mat_list;
            }
            else
            {
                var mat_list = ModelAsset.Materials;
                mat_list[0] = new SlimDX.Direct3D9.ExtendedMaterial()
                {
                    MaterialD3D = new SlimDX.Direct3D9.Material()
                    {
                        Diffuse = new Color4(System.Drawing.Color.White),
                        Emissive = new Color4(System.Drawing.Color.White)
                    }
                };
                ModelAsset.Materials = mat_list;
            }
        }
    }
}
