using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Core
{
    class FPSManager
    {
        private int time_begin = 0;
        private int ideal_time = (int)(1000.0F / 60.0F);

        public void Begin()
        {
            time_begin = Environment.TickCount;
        }

        public void End()
        {
            var past_time = Environment.TickCount - time_begin;
            var wait_time = ideal_time - past_time;
            if (wait_time > 0) System.Threading.Thread.Sleep(wait_time);
        }
    }
}
