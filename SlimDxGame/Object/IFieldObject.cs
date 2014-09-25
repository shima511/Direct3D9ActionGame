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
        /// <summary>
        /// 出現可能・不可能の設定を行います。
        /// </summary>
        bool Spawnable { get; set; }
        Vector3 Position { get; set; } 
    }
}
