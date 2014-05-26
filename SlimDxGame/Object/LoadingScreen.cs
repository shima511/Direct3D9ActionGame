using System;
using SlimDX;

namespace SlimDxGame.Object
{
    class LoadingScreen : Component.IDrawableObject, Component.IUpdateObject
    {
        private bool _is_visible = true;
        public bool IsVisible { get { return _is_visible; } set { _is_visible = value; } }
        private string text;
        private const int CHANGE_INTERBAL = 20;
        private const int TEXT_CHANGE_TIMES = 3;
        private int time = 0;
        private Color4 _color = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
        public Asset.Font Font { private get; set; }

        private void ChangeText()
        {
            if (time < CHANGE_INTERBAL) { text = "Now Loading."; }
            else if (time < CHANGE_INTERBAL * 2) { text = "Now Loading.."; }
            else if (time < CHANGE_INTERBAL * 3) { text = "Now Loading..."; }
        }

        public void Update()
        {
            time++;
            time %= CHANGE_INTERBAL * TEXT_CHANGE_TIMES;
            ChangeText();
        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {
            Font.Resource.DrawString(dev, text, 10, 10, _color);
        }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {

        }
    }
}
