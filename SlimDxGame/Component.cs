using System;

namespace SlimDxGame.Component
{
    interface IUpdateObject
    {
        bool IsActive { get; set; }
        void Update();
    }

    interface IDrawableObject
    {
        bool IsVisible { get; set; }
        void Draw3D(SlimDX.Direct3D9.Device dev);
        void Draw2D(SlimDX.Direct3D9.Sprite dev);
    }

    interface IOperableObject
    {
        void ControllerAction(SlimDxGame.Controller controller);
    }
}
