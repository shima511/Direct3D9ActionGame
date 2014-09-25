using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object.Item
{
    class Coin : Object.Base.Model, IBase
    {
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
        public bool Spawnable { get; set; }
        public bool IsActive { get; set; }
        public Status.Stage StageState { private get; set; }
        public event Action<IBase> OnHit;
        Collision.Shape.Line hit_collision;
        public readonly uint _score = 10;
        public uint Score { get { return _score; } private set {} }

        public Coin(SlimDX.Vector2 pos)
        {
            hit_collision = new Collision.Shape.Line()
            {
                StartingPoint = new SlimDX.Vector2(pos.X - 1.0f, pos.Y + 1.0f),
                TerminalPoint = new SlimDX.Vector2(pos.X + 1.0f, pos.Y + 1.0f)
            };
            OnHit += Coin_OnHit;
            Spawnable = true;
        }

        void Coin_OnHit(IBase obj)
        {
            var score = StageState.Score;
            score += this._score;
            StageState.Score = score;
        }

        public void Update()
        {
            var rot = Rotation;
            rot.Y += (float)(Math.PI * 0.01);
            Rotation = rot;
        }

        public void DrawHitRange(SlimDX.Direct3D9.Device dev)
        {
            if (this.Spawnable) hit_collision.Draw3D(dev);
        }

        public bool IsCatched { get; set; }
        public void Dispatch(ICollisionObject obj)
        {

        }
        public void Hit(Player player)
        {
            if (player.FeetCollision.Hit(hit_collision))
            {
                OnHit(this);
            }
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
        public void Hit(Ground.LeftWall wall)
        {

        }
    }
}
