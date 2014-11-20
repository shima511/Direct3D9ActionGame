using System;
using SlimDX;

namespace SlimDxGame.Object.Base
{
    abstract class PlaneObject : Component.IDrawableObject
    {
        protected bool _is_visible = true;
        protected Vector2 _scale = new Vector2(1.0f, 1.0f);
        protected Vector2 _position = new Vector2(0.0f, 0.0f);
        protected Color4 _color = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
        public float Angle { get; set; }
        public bool IsVisible { get { return _is_visible; } set { _is_visible = value; } }
        public Vector2 Scale { get { return this._scale; } set { this._scale = value; } }
        public Vector2 Position { get { return this._position; } set { this._position = value; } }
        public Color4 Color { get { return this._color; } set { this._color = value; } }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {

        }

        public abstract void Draw2D(SlimDX.Direct3D9.Sprite dev);
    }
}
