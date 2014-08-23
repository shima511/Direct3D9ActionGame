using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Scene
{
    class Credit : Base
    {
        GameState<Credit> current_state = new InitState();
        Effect.Light light = new Effect.Light();

        class InitState : GameState<Credit>
        {
            void InitLayer(GameRootObjects root)
            {
                root.Layers = new List<List<Component.IDrawableObject>>();
                root.Layers.Capacity = 3;
                for (int i = 0; i < root.Layers.Capacity; i++)
                {
                    root.Layers.Add(new List<Component.IDrawableObject>());
                }
            }

            public int Update(GameRootObjects root, Credit parent, GameState<Credit> new_state)
            {
                InitLayer(root);
                parent.light.Index = 0;
                parent.light.EnableLight = true;
                parent.light.Property = new SlimDX.Direct3D9.Light()
                {
                    Type = SlimDX.Direct3D9.LightType.Directional,
                    Diffuse = System.Drawing.Color.White,
                    Ambient = System.Drawing.Color.GhostWhite,
                    Direction = new SlimDX.Vector3(0.0f, -1.0f, 0.0f)
                };
                root.Layers[0].Add(parent.light);
                return 0;
            }
        }

        public override int Update(GameRootObjects root, ref Base new_scene)
        {
            int val = current_state.Update(root, this, current_state);
            if (val != 0)
            {
                new_scene = new Scene.Title();
            }
            return 0;
        }
    }
}
