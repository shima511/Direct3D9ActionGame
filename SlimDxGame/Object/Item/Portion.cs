using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object.Item
{
    class Portion : Object.Base.Model, IBase
    {
        public bool Spawnable { get; set; }
        public SlimDX.Vector2 Position2D
        {
            get 
            {
                return new SlimDX.Vector2(Position.X, Position.Y);
            }
            set 
            {
                var pos = Position;
                pos.X = Position2D.X;
                pos.Y = Position2D.Y;
                Position = pos;
            }
        }
        public bool IsActive { get; set; }
        public event Action<IBase> OnHit;
        public void Update()
        {

        }
        public void DrawHitRange(SlimDX.Direct3D9.Device dev)
        {

        }
        public void Dispatch(ICollisionObject obj)
        {

        }
        public void Hit(Player player)
        {

        }
        public void Hit(Ground.Floor floor)
        {

        }
        public void Hit(Ground.Ceiling ceiling)
        {

        }
        public void Hit(Ground.RightWall wall)
        {

        }
    }
}
