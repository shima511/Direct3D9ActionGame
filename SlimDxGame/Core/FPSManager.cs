using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Core
{
    class FPSManager
    {
        const int FPS = 60;
        float fps = 0;
        int count = 0;
        int time_begin = 0;
        int ideal_time = (int)(1000.0F / FPS);
        const int Sample = FPS;

        public void Begin()
        {
            if (count == 0)
            {
                time_begin = Environment.TickCount;
            }
            if (count == Sample)
            {
                var t = Environment.TickCount;
                fps = 1000.0f/((t-time_begin)/(float)Sample);
                count = 0;
                time_begin = t;
            }
            count++;
        }

        public void Draw(SlimDX.Direct3D9.Sprite dev, SlimDX.Direct3D9.Font font)
        {
            font.DrawString(dev, fps.ToString(), 0, 0, System.Drawing.Color.White);
        }

        public void End()
        {
            var past_time = Environment.TickCount - time_begin;
            var wait_time = count * ideal_time - past_time;
            if (wait_time > 0) System.Threading.Thread.Sleep(wait_time);
        }
    }
}
