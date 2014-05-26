using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object
{
    class Fader : Base.Sprite, Component.IUpdateObject
    {
        public enum Flag
        {
            FADE_IN,
            FADE_OUT
        };

        public Flag Effect { private get; set; }
        public int FadingTime { private get; set; }
        public void Update()
        {
            float strength = 1.0f / FadingTime;
            switch (Effect)
            {
                case Flag.FADE_IN:
                    _color.Alpha -= strength;
                    break;
                case Flag.FADE_OUT:
                    _color.Alpha += strength;
                    break;
            }
        }
    }
}
