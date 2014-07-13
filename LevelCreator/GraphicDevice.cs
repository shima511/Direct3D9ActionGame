using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.Direct3D9;

namespace LevelCreator
{
    class GraphicDevice : IDisposable
    {
        Direct3D direct3D;
        public SlimDX.Direct3D9.Device D3DDevice { get; private set; }
        public SlimDX.Direct3D9.PresentParameters SettingParam { get; private set; }

        private System.Windows.Forms.Form form;

        public GraphicDevice(System.Windows.Forms.Form f)
        {
            form = f;
        }

        public void Initialize()
        {
            direct3D = new Direct3D();
            try
            {
                SettingParam = new SlimDX.Direct3D9.PresentParameters
                    {
                        BackBufferFormat = Format.X8R8G8B8,
                        BackBufferCount = 1,
                        BackBufferWidth = form.Width,
                        BackBufferHeight = form.Height,
                        Multisample = MultisampleType.None,
                        SwapEffect = SwapEffect.Discard,
                        EnableAutoDepthStencil = true,
                        AutoDepthStencilFormat = Format.D16,
                        PresentFlags = PresentFlags.DiscardDepthStencil,
                        PresentationInterval = PresentInterval.Default,
                        Windowed = true,
                        DeviceWindowHandle = form.Handle
                    };
                D3DDevice = new SlimDX.Direct3D9.Device(direct3D, 0, DeviceType.Hardware, form.Handle, CreateFlags.HardwareVertexProcessing, SettingParam);
            }catch(Direct3D9Exception ex){
                System.Diagnostics.Debug.Assert(false, ex.Message + "初期化失敗");
            }
        }

        public void Dispose()
        {
            D3DDevice.Dispose();
            direct3D.Dispose();
        }

    }
}
