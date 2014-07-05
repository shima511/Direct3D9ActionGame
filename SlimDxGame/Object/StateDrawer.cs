using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object
{
    class StateDrawer : Component.IDrawableObject, Component.IUpdateObject
    {
        public bool IsVisible { get; set; }
        public Asset.Font Font { private get; set; }
        Status.Charactor player_state;
        Status.Stage stage_state;
        Object.Base.String time_str = new Base.String();
        Object.Base.String score_str = new Base.String();
        Object.Base.String hp_str = new Base.String();

        public StateDrawer(Status.Stage s_state, Status.Charactor p_state)
        {
            stage_state = s_state;
            player_state = p_state;
            IsVisible = true;
            time_str.Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 2 / 5, 0);
            score_str.Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 3 / 5, 0);
            hp_str.Position = new SlimDX.Vector2(Core.Game.AppInfo.Width * 4 / 5, 0);
        }

        public void Update()
        {
            time_str.Text = "TIME:" + stage_state.Time.ToString();
            score_str.Text = "SCORE:" + stage_state.Score.ToString();
            hp_str.Text = "HP:" + player_state.HP.ToString();
            time_str.Font = Font;
            score_str.Font = Font;
            hp_str.Font = Font;
        }

        public void Draw3D(SlimDX.Direct3D9.Device dev)
        {

        }

        public void Draw2D(SlimDX.Direct3D9.Sprite dev)
        {
            time_str.Draw2D(dev);
            score_str.Draw2D(dev);
            hp_str.Draw2D(dev);
        }
    }
}
