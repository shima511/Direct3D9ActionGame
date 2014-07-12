using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelCreator.Object
{
    sealed class Player : Model
    {
        public BinaryParser.Property.Player PlayerInfo { private get; set; }

        public override void Update()
        {
            _position.X = PlayerInfo.Position.X;
            _position.Y = PlayerInfo.Position.Y;
        }
    }
}
