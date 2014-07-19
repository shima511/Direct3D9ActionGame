using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object.Item
{
    delegate void OnHitAction(IBase obj);

    interface IBase : Component.IDrawableObject, Component.IUpdateObject, ICollisionObject
    {
        SlimDX.Vector2 Position2D { get; set; }
        event OnHitAction OnHit;
    }
}
