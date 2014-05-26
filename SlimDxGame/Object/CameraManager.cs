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
        private const float MoveStrength = 0.1f;
        private const float Distance = 2.0f;

        public void Update()
        {
            if (Player.IsTurning)
            {
                if (Player.FaceRight && Camera.EyePosition.X <= Player.Position.X + Distance)
                {
                    Camera.EyePosition = new SlimDX.Vector3(Camera.EyePosition.X + MoveStrength, Player.Position.Y, Camera.EyePosition.Z);
                    Camera.AtPosition = new SlimDX.Vector3(Camera.AtPosition.X + MoveStrength, Player.Position.Y, Camera.AtPosition.Z);
                }
                if (Player.FaceLeft && Camera.EyePosition.X >= Player.Position.X - Distance)
                {
                    Camera.EyePosition = new SlimDX.Vector3(Camera.EyePosition.X - MoveStrength, Player.Position.Y, Camera.EyePosition.Z);
                    Camera.AtPosition = new SlimDX.Vector3(Camera.AtPosition.X - MoveStrength, Player.Position.Y, Camera.AtPosition.Z);
                }
            }
            else
            {
                if (Player.FaceRight)
                {
                    Camera.EyePosition = new SlimDX.Vector3(Player.Position.X + Distance, Player.Position.Y, Camera.EyePosition.Z);
                    Camera.AtPosition = new SlimDX.Vector3(Player.Position.X + Distance, Player.Position.Y, Camera.AtPosition.Z);
                }
                else
                {
                    Camera.EyePosition = new SlimDX.Vector3(Player.Position.X - Distance, Player.Position.Y, Camera.EyePosition.Z);
                    Camera.AtPosition = new SlimDX.Vector3(Player.Position.X - Distance, Player.Position.Y, Camera.AtPosition.Z);
                }
            }
        }
    }
}
