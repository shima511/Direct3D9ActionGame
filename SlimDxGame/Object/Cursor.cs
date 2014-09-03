using System;
using System.Collections.Generic;
using SlimDX;

namespace SlimDxGame.Object
{
    class Cursor : Base.Sprite, Component.IOperableObject, Component.IUpdateObject
    {
        /// <summary>
        /// カーソルの操作が行われていない時間
        /// </summary>
        int NonActionTime { get; set; }
        /// <summary>
        /// 現在のインデックス
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// カーソルが動く位置のリスト
        /// </summary>
        public List<Vector2> PositionList { get; set; }
        /// <summary>
        /// カーソルが操作された時に行うアクション
        /// </summary>
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
            if (Index == -1) Index += PositionList.Count;
            Index %= PositionList.Count;
            _position = PositionList[Index];
        }
    }
}
