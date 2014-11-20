using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.MenuCreator
{
    class MenuDirector
    {
        public GameRootObjects RootObjects { private get; set; }
        public Controller Controller { private get; set; }

        public Utility.Menu Create(MenuBuilder builder)
        {
            builder.SetBG(RootObjects.TextureContainer);
            builder.SetColumns(RootObjects.FontContainer);
            builder.SetCursor(RootObjects.TextureContainer, RootObjects.SoundContainer);
            builder.SetFunction(RootObjects);
            builder.SetReflectionButton(Controller);
            return builder.GetMenu();
        }
    }
}
