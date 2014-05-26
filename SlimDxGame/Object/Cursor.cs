using System;
using System.Collections.Generic;
using SlimDX;

namespace SlimDxGame.Object
{
    class Cursor : Base.Sprite, Component.IOperableObject, Component.IUpdateObject
    {
        public int NonActionTime { get; set; }
        public int Index { get; set; }
        public List<Vector2> Positions { get; set; }
        public Action MoveAction { get; set; }

        public void ControllerAction(SlimDxGame.Controller controller)
        {
            if (controller.DownButton.IsPressed())
            {
                MoveAction();
                NonActionTime = 0;
                Index++;
            }
            if (controller.UpButton.IsPressed())
            {
                MoveAction();
                NonActionTime = 0;
                Index--;
            }
        }

        public void Update()
        {
            NonActionTime++;
            if (Index == -1) Index += Positions.Count;
            Index %= Positions.Count;
            _position = Positions[Index];
        }
    }
}
