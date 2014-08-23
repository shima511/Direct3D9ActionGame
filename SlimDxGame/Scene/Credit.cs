using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Scene
{
    class Credit : Base
    {
        GameState<Credit> CurrentState { get; set; }
        Effect.Light Light { get; set; }

        public Credit()
        {
            CurrentState = new InitState();
            Light = new Effect.Light();
        }

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
                parent.Light.Index = 0;
                parent.Light.EnableLight = true;
                parent.Light.Property = new SlimDX.Direct3D9.Light()
                {
                    Type = SlimDX.Direct3D9.LightType.Directional,
                    Diffuse = System.Drawing.Color.White,
                    Ambient = System.Drawing.Color.GhostWhite,
                    Direction = new SlimDX.Vector3(0.0f, -1.0f, 0.0f)
                };
                root.Layers[0].Add(parent.Light);
                return 0;
            }
        }

        public override int Update(GameRootObjects root, ref Base new_scene)
        {
            int val = CurrentState.Update(root, this, CurrentState);
            if (val != 0)
            {
                new_scene = new Scene.Title();
            }
            return 0;
        }
    }
}
