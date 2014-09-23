using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDxGame.Component;
using SlimDX;

namespace SlimDxGame.Object
{
    /// <summary>
    /// フィールド上のオブジェクトインターフェース
    /// </summary>
    interface IFieldObject : IUpdateObject, IDrawableObject, ICollisionObject
    {
        Vector3 Position { get; set; } 
    }
}
