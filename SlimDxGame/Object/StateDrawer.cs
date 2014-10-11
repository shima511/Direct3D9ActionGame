using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object
{
    class StateDrawer : Component.IDrawableObject, Component.IUpdateObject
    {
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }
        public Asset.Font Font { private get; set; }
        Object.Player player;
        Status.Stage stage_state;
        Object.Base.String life_str = new Base.String();
        Object.Base.String time_str = new Base.String();
        Object.Base.String score_str = new Base.String();
        Object.Base.String state_str = new Base.String();

        public StateDrawer(Status.Stage s_state, Object.Player p)
        {
            stage_state = s_state;
            player = p;
            IsVisible = true;
            time_str.Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 2 / 6, 0);
            score_str.Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 3 / 6, 0);
            life_str.Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 5 / 6, 0);
            state_str.Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 1 / 6, 0);
        }

        public void Update()
        {
            time_str.Text = "TIME:" + stage_state.Time.ToString();
            score_str.Text = "SCORE:" + stage_state.Score.ToString();
            life_str.Text = "LIFE:" + player.Life.ToString();
            state_str.Text = player.State.ToString();
            time_str.Font = Font;
            score_str.Font = Font;
            life_str.Font = Font;
            state_str.Font = Font;
        }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {

        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {
            time_str.Draw2D(dev);
            score_str.Draw2D(dev);
            life_str.Draw2D(dev);
            state_str.Draw2D(dev);
        }
    }
}
