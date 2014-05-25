using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.DirectInput;

namespace SlimDxGame
{
    class Controller : List<Component.IOperableObject>
    {
        public class Component
        {
            static public Device.Input.Keyboard Keyboard { protected get; set; }
            static public Device.Input.Mouse Mouse { protected get; set; }
            static public Device.Input.JoyPad Joypad { protected get; set; }
        }

        public class Button : Component
        {
            private Key keyboard_key;

            public Button(SlimDX.DirectInput.Key key)
            {
                keyboard_key = key;
            }

            public bool IsPressed()
            {
                return Keyboard.IsPressed(keyboard_key) | Mouse.IsPressed() | Joypad.IsPressed();
            }
            public bool IsBeingPressed()
            {
                return Keyboard.IsBeingPressed(keyboard_key) | Mouse.IsBeingPressed() | Joypad.IsBeingPressed();
            }
            public bool IsReleased()
            {
                return Keyboard.IsReleased(keyboard_key) | Mouse.IsReleased() | Joypad.IsReleased();
            }
        }

        public class AnalogStick : Component
        {
            
        }

        /// <summary>
        /// Aボタン
        /// </summary>
        public Button AButton = new Button(Key.Z);
        /// <summary>
        /// Bボタン
        /// </summary>
        public Button BButton = new Button(Key.X);
        /// <summary>
        /// Cボタン
        /// </summary>
        public Button CButton = new Button(Key.C);
        /// <summary>
        /// Dボタン
        /// </summary>
        public Button DButton = new Button(Key.A);
        /// <summary>
        /// Eボタン
        /// </summary>
        public Button EButton = new Button(Key.S);
        /// <summary>
        /// Fボタン
        /// </summary>
        public Button FButton = new Button(Key.D);
        /// <summary>
        /// 十字キー上方向
        /// </summary>
        public Button UpButton = new Button(Key.UpArrow);
        /// <summary>
        /// 十字キー右方向
        /// </summary>
        public Button RightButton = new Button(Key.RightArrow);
        /// <summary>
        /// 十字キー下方向
        /// </summary>
        public Button DownButton = new Button(Key.DownArrow);
        /// <summary>
        /// 十字キー左方向
        /// </summary>
        public Button LeftButton = new Button(Key.LeftArrow);
        /// <summary>
        /// スタートボタン
        /// </summary>
        public Button StartButton = new Button(Key.Return);
        /// <summary>
        /// セレクトボタン
        /// </summary>
        public Button SelectButton = new Button(Key.Space);
        public AnalogStick Stick = new AnalogStick();

        public void Update(ref Device.Input.Keyboard keyboard, ref Device.Input.Mouse mouse, ref Device.Input.JoyPad joypad)
        {
            Component.Keyboard = keyboard;
            Component.Mouse = mouse;
            Component.Joypad = joypad;
            foreach (var item in this)
            {
                item.ControllerAction(this);
            }
        }
    }
}
