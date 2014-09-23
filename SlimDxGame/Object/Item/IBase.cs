using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object.Item
{
    interface IBase : IFieldObject
    {
        SlimDX.Vector2 Position2D { get; set; }
        event Action<IBase> OnHit;
    }
}
