﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;

namespace LevelCreator.Object
{
    public class Line : Model
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
                for (int i = 0; i < this.ModelAsset.Materials.Count; i++)
                {
                    ModelAsset.Materials[i] = new SlimDX.Direct3D9.ExtendedMaterial()
                    {
                        MaterialD3D = new SlimDX.Direct3D9.Material()
                        {
                            Diffuse = new SlimDX.Color4(System.Drawing.Color.Red),
                            Emissive = new SlimDX.Color4(System.Drawing.Color.Red)
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
