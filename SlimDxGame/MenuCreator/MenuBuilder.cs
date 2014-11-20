using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.MenuCreator
{
    abstract class MenuBuilder
    {
        protected Utility.Menu menu;

        public MenuBuilder(Utility.Menu m)
        {
            menu = m;
        }

        abstract public void SetBG(Utility.AssetContainer<Asset.Texture> tex_container);
        abstract public void SetCursor(Utility.AssetContainer<Asset.Texture> tex_container, Utility.AssetContainer<Asset.Sound> sound_container);
        abstract public void SetColumns(Utility.AssetContainer<Asset.Font> font_container);
        abstract public void SetReflectionButton(Controller controller);
        abstract public void SetFunction(GameRootObjects root_objects);

        public Utility.Menu GetMenu()
        {
            return menu;
        }
    }
}
