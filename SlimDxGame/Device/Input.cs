using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Device
{
    class Input : Device
    {
        private System.Windows.Forms.Form form;
        private SlimDX.DirectInput.DirectInput dip_dev;
        private SlimDX.DirectInput.Keyboard keyboard_dev;
        private SlimDX.DirectInput.Mouse mouse_dev;
        static private List<SlimDX.DirectInput.Joystick> joypad_devs = new List<SlimDX.DirectInput.Joystick>();
        static public List<SlimDX.DirectInput.Joystick> JoypadDevices { get { return joypad_devs; } private set { joypad_devs = value; } }

        public class Keyboard
        {
            static public SlimDX.DirectInput.Keyboard Device { private get; set; }
            private SlimDX.DirectInput.Key[] now_pressed_keys = new SlimDX.DirectInput.Key[256];
            private SlimDX.DirectInput.Key[] pre_pressed_keys = new SlimDX.DirectInput.Key[256];

            public void AcquireState()
            {
                if (Device == null) return;
                if (Device.Acquire().IsSuccess)
                {
                    var state = Device.GetCurrentState();
                    now_pressed_keys = state.PressedKeys.ToArray();
                }
                else
                {
                    now_pressed_keys.Initialize();
                }
            }

            public bool IsPressed(SlimDX.DirectInput.Key key)
            {
                return Array.IndexOf(now_pressed_keys, key) != -1 && Array.IndexOf(pre_pressed_keys, key) == -1;
            }

            public bool IsBeingPressed(SlimDX.DirectInput.Key key)
            {
                return Array.IndexOf(now_pressed_keys, key) != -1 && Array.IndexOf(pre_pressed_keys, key) != -1;
            }

            public bool IsReleased(SlimDX.DirectInput.Key key)
            {
                return Array.IndexOf(now_pressed_keys, key) == -1 && Array.IndexOf(pre_pressed_keys, key) != -1;
            }

            public void End()
            {
                pre_pressed_keys = now_pressed_keys;
                now_pressed_keys.Initialize();
            }

            public void Terminate(){
                if (Device != null)
                {
                    Device.Unacquire();
                    Device.Dispose();
                }
            }
        }

        public class Mouse
        {
            static public SlimDX.DirectInput.Mouse Device { private get; set; }
            public void AcquireState()
            {
                if (Device == null) return;
                if (Device.Acquire().IsSuccess)
                {
                    
                }
            }
            public bool IsPressed()
            {
                return false;
            }
            public bool IsBeingPressed()
            {
                return false;
            }
            public bool IsReleased()
            {
                return false;
            }

            public void End()
            {

            }

            public void Terminate()
            {
                if (Device != null)
                {
                    Device.Unacquire();
                    Device.Dispose();
                }
            }
        }

        public class JoyPad
        {
            SlimDX.DirectInput.JoystickObjects key;
            static public SlimDX.DirectInput.Joystick Device { private get; set; }
            public void AcquireState()
            {
                if (Device == null) return;
                if (Device.Acquire().IsSuccess)
                {
                    var state = Device.GetCurrentState();
                    
                }
            }
            public bool IsPressed()
            {
                return false;
            }
            public bool IsBeingPressed()
            {
                return false;
            }
            public bool IsReleased()
            {
                return false;
            }

            public void End()
            {

            }

            public void Terminate()
            {
                if (Device != null)
                {
                    Device.Unacquire();
                    Device.Dispose();
                }
            }
        }

        public Input(System.Windows.Forms.Form f)
        {
            form = f;
        }

        private void InitializeKeyboardDevice()
        {
            var device = new SlimDX.DirectInput.Keyboard(dip_dev);
            device.SetCooperativeLevel(form.Handle, SlimDX.DirectInput.CooperativeLevel.Foreground | SlimDX.DirectInput.CooperativeLevel.Nonexclusive);
            device.Properties.BufferSize = 1;
            Keyboard.Device = device;
            keyboard_dev = device;
        }

        private void InitializeMouseDevice()
        {
            var device = new SlimDX.DirectInput.Mouse(dip_dev);
            device.SetCooperativeLevel(form.Handle, SlimDX.DirectInput.CooperativeLevel.Foreground | SlimDX.DirectInput.CooperativeLevel.Nonexclusive);
            device.Properties.BufferSize = 1;
            Mouse.Device = device;
            mouse_dev = device;
        }

        private void InitializeJoyPadDevice()
        {
            foreach (var instance in dip_dev.GetDevices(SlimDX.DirectInput.DeviceClass.GameController, SlimDX.DirectInput.DeviceEnumerationFlags.AttachedOnly))
            {
                var device = new SlimDX.DirectInput.Joystick(dip_dev, instance.InstanceGuid);
                device.SetCooperativeLevel(form.Handle, SlimDX.DirectInput.CooperativeLevel.Foreground | SlimDX.DirectInput.CooperativeLevel.Exclusive);
                device.Properties.BufferSize = 1;
                joypad_devs.Add(device);
            }
        }

        public void Initialize()
        {
            try
            {
                dip_dev = new SlimDX.DirectInput.DirectInput();
                InitializeKeyboardDevice();
                InitializeMouseDevice();
                InitializeJoyPadDevice();
            }catch(SlimDX.DirectInput.DirectInputException){
                throw new Core.InitializeException("DirectInputの初期化に失敗しました。");
            }
        }

        public void Terminate()
        {
            if(keyboard_dev != null) keyboard_dev.Dispose();
            if(mouse_dev != null) mouse_dev.Dispose();
            joypad_devs.ForEach(delegate(SlimDX.DirectInput.Joystick joypad_dev) { joypad_dev.Dispose(); });
            if(dip_dev != null) dip_dev.Dispose();
        }
    }
}
