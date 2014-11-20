using System;
using System.Collections.Generic;

namespace SlimDxGame
{
    class InputManager : List<Controller>
    {
        private Device.Input.Keyboard keyboard = new Device.Input.Keyboard();
        private Device.Input.Mouse mouse = new Device.Input.Mouse();
        private Device.Input.JoyPad joypad = new Device.Input.JoyPad();

        public void Update()
        {
            // キーボードとマウスの状態を取得
            keyboard.AcquireState();
            mouse.AcquireState();

            foreach (var controller in this)
            {
                // ジョイパッドはジョイパッドの数だけ状態を取得する
                if (Device.Input.JoypadDevices.Count != 0) Device.Input.JoyPad.Device = Device.Input.JoypadDevices[this.IndexOf(controller)];
                joypad.AcquireState();

                // コントローラーを更新
                controller.Update(ref keyboard, ref mouse, ref joypad);

                joypad.End();
            }

            // キーボードとマウスの入力が終了したことを伝える
            keyboard.End();
            mouse.End();
        }
    }
}
