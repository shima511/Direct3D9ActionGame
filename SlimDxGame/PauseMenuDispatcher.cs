using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame
{
    class PauseMenuDispatcher : Component.IOperableObject
    {
        /// <summary>
        /// 
        /// </summary>
        public bool PauseButtonPushed { get; private set; }

        public PauseMenuDispatcher()
        {
            PauseButtonPushed = false;
        }

        public void ControllerAction(SlimDxGame.Controller controller)
        {
            if (controller.StartButton.IsPressed())
            {
                PauseButtonPushed = true;
            }
        }
    }
}
