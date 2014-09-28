using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object
{
    /// <summary>
    /// ステージの境界線を示すクラス
    /// </summary>
    class Boundary : IFieldObject
    {
        public bool IsVisible { get; set; }
        public bool IsActive { get; set; }
        public bool Spawnable { get; set; }
        public SlimDX.Vector3 Position { get; set; }
        List<Collision.Shape.Line> Lines { get; set; }

        public Boundary(System.Drawing.Rectangle r)
        {
            IsActive = true;
            IsVisible = true;
            Spawnable = true;
            Lines = new List<Collision.Shape.Line>();
            Lines.AddRange(new Collision.Shape.Line[] 
            {
                new Collision.Shape.Line() { StartingPoint = new SlimDX.Vector2(r.Left, r.Top), TerminalPoint = new SlimDX.Vector2(r.Left, r.Bottom)},
                new Collision.Shape.Line() { StartingPoint = new SlimDX.Vector2(r.Left, r.Top), TerminalPoint = new SlimDX.Vector2(r.Right, r.Top) },
                new Collision.Shape.Line() { StartingPoint = new SlimDX.Vector2(r.Right, r.Bottom), TerminalPoint = new SlimDX.Vector2(r.Left, r.Bottom)},
                new Collision.Shape.Line() { StartingPoint = new SlimDX.Vector2(r.Right, r.Bottom), TerminalPoint = new SlimDX.Vector2(r.Right, r.Top) }
            });
        }

        public void Update()
        {

        }

        public void DrawHitRange(SlimDX.Direct3D9.Device dev)
        {
            foreach (var item in Lines)
            {
                item.Draw3D(dev);
            }
        }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {

        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {

        }

        public void Dispatch(Object.ICollisionObject obj)
        {

        }

        public void Hit(Player player)
        {
            foreach (var item in Lines)
            {
                if (player.HeadCollision.Hit(item))
                {
                    var state = player.State;
                    state.HP = 0;
                    player.State = state;
                }
            }
        }

        public void Hit(Ground.Floor floor)
        {
            throw new NotImplementedException();
        }

        public void Hit(Ground.Ceiling ceiling)
        {
            throw new NotImplementedException();
        }

        public void Hit(Ground.RightWall wall)
        {
            throw new NotImplementedException();
        }

        public void Hit(Ground.LeftWall wall)
        {
            throw new NotImplementedException();
        }
    }
}
