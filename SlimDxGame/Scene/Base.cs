using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Scene
{
    abstract class Base
    {
        public virtual void ExitScene(GameRootObjects root_objects)
        {
            root_objects.Layers.Clear();
            root_objects.UpdateList.Clear();
            root_objects.InputManager.Clear();
            root_objects.FontContainer.DeleteAllObject();
            root_objects.TextureContainer.DeleteAllObject();
            root_objects.SoundContainer.DeleteAllObject();
            root_objects.ModelContainer.DeleteAllObject();
        }
        public abstract int Update(GameRootObjects root, ref Base new_scene);
    }
}
