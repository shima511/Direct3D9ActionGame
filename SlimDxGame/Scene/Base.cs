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
            root_objects.layers.Clear();
            root_objects.update_list.Clear();
            root_objects.input_manager.Clear();
            root_objects.font_container.DeleteAllObject();
            root_objects.tex_container.DeleteAllObject();
            root_objects.sound_container.DeleteAllObject();
            root_objects.model_container.DeleteAllObject();
        }
        public abstract int Update(GameRootObjects root, ref Base new_scene);
    }
}
