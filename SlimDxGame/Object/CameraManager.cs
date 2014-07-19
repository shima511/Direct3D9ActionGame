using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object
{
    class CameraManager : Component.IUpdateObject
    {
        public Camera Camera { private get; set; }
        public Player Player { private get; set; }
        public BinaryParser.Property.Stage StageInfo { private get; set; }
        private const float MoveStrength = 0.1f;
        private const float Distance = 2.0f;

        public void Update()
        {
            var eye_pos = Camera.EyePosition;
            var at_pos = Camera.AtPosition;
            eye_pos.X = Player.Position.X;
            at_pos.X = Player.Position.X;
            eye_pos.Y = Player.Position.Y;
            at_pos.Y = Player.Position.Y;
            Camera.EyePosition = eye_pos;
            Camera.AtPosition = at_pos;
        }
    }
}
